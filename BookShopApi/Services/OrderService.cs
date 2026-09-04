using BookShopApi.Data;
using BookShopApi.Dtos.Order;
using BookShopApi.Helpers;
using BookShopApi.Interfaces;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IShoppingCartRepository _shoppingCartRepo;
        private readonly IShippingAddressRepository _shippingAddressRepo;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly ICouponRepository _couponRepo;
        private readonly IPaymentService _paymentService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            ApplicationDbContext context,
            IShoppingCartRepository shoppingCartRepository,
            IShippingAddressRepository shippingAddressRepository,
            IPriceCalculationService priceCalculationService,
            ICouponRepository couponRepository,
            IPaymentService paymentService,
            ILogger<OrderService> logger)
        {
            _context = context;
            _shoppingCartRepo = shoppingCartRepository;
            _shippingAddressRepo = shippingAddressRepository;
            _priceCalculationService = priceCalculationService;
            _couponRepo = couponRepository;
            _paymentService = paymentService;
            _logger = logger;
        }

        public async Task<CheckoutResult> CheckoutAsync(
            string userId,
            CheckoutRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var cartItems = (await _shoppingCartRepo.GetCartItemsAsync(userId)).ToList();
            if (cartItems.Count == 0)
                return CheckoutResult.Failed(CheckoutErrorStatus.EmptyCart, "Cart is empty.");

            var address = await _shippingAddressRepo.GetShippingAddressByIdAsync(request.ShippingAddressId);
            if (address == null)
                return CheckoutResult.Failed(CheckoutErrorStatus.ShippingAddressNotFound, "The Shipping Address Not Found.");

            if (address.UserId != userId)
                return CheckoutResult.Failed(CheckoutErrorStatus.ShippingAddressForbidden, "The Shipping Address does not belong to the current user.");

            var insufficient = cartItems
                .Where(c => c.Book.Quantity < c.Quantity)
                .ToList();
            if (insufficient.Count > 0)
            {
                var details = string.Join(", ", insufficient.Select(c =>
                    $"{c.Book.Title} (requested {c.Quantity}, available {c.Book.Quantity})"));
                return CheckoutResult.Failed(
                    CheckoutErrorStatus.InsufficientStock,
                    $"Insufficient stock: {details}");
            }

            Coupon? coupon = null;
            if (!string.IsNullOrWhiteSpace(request.CouponCode))
            {
                var validation = await _couponRepo.ValidateForUseAsync(request.CouponCode, userId);
                if (!validation.IsValid || validation.Coupon == null)
                {
                    return CheckoutResult.Failed(
                        CheckoutErrorStatus.InvalidCoupon,
                        validation.ErrorMessage ?? "Coupon is not valid.");
                }

                coupon = validation.Coupon;
            }

            var price = _priceCalculationService.CalculateCartTotal(
                cartItems.Select(c => new CartPriceItem { Book = c.Book, Quantity = c.Quantity }),
                coupon);

            var lineByBookId = price.Items.ToDictionary(l => l.BookId);
            var order = new Order
            {
                UserId = userId,
                Status = OrderStatus.PendingPayment,
                SubTotal = price.Subtotal,
                BookDiscountTotal = price.BookDiscountAmount,
                CouponDiscountTotal = price.CouponDiscountAmount,
                TotalAmount = price.FinalTotal,
                CouponCode = price.CouponCode,
                ShippingAddressId = address.Id,
                ShippingRecipientName = address.UserName,
                ShippingPhoneNumber = address.PhoneNumber,
                ShippingAddressLine = address.Address,
                ShippingPostCode = address.PostCode,
                ShippingProvinceName = address.Province?.Name ?? string.Empty,
                ShippingCityName = address.City?.Name ?? string.Empty,
                Items = cartItems.Select(c =>
                {
                    var line = lineByBookId[c.BookId];
                    return new OrderItem
                    {
                        BookId = c.BookId,
                        Quantity = c.Quantity,
                        UnitPrice = line.UnitEffectivePrice,
                        BookTitle = c.Book.Title
                    };
                }).ToList()
            };

            await _context.Orders.AddAsync(order, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            PaymentResult payment;
            try
            {
                payment = await _paymentService.ProcessPaymentAsync(order, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Payment processing threw for order {OrderId}.", order.Id);
                await MarkPaymentFailedAsync(order.Id, cancellationToken);
                order.Status = OrderStatus.PaymentFailed;
                return CheckoutResult.Failed(
                    CheckoutErrorStatus.PaymentFailed,
                    "Payment processing failed.",
                    order);
            }

            if (!payment.IsSuccessful)
            {
                await MarkPaymentFailedAsync(order.Id, cancellationToken);
                order.Status = OrderStatus.PaymentFailed;
                return CheckoutResult.Failed(
                    CheckoutErrorStatus.PaymentFailed,
                    payment.ErrorMessage ?? "Payment failed.",
                    order);
            }

            try
            {
                await FinalizePaidOrderAsync(order.Id, payment.ReferenceId, cancellationToken);
            }
            catch (Exception ex) when (ex is InvalidOperationException or CouponValidationException or DbUpdateConcurrencyException)
            {
                _logger.LogWarning(ex, "Finalization failed for order {OrderId} after a successful payment.", order.Id);
                var pending = await LoadOrderAsync(order.Id, cancellationToken);
                return CheckoutResult.Failed(
                    CheckoutErrorStatus.FinalizationFailed,
                    ex.Message,
                    pending ?? order);
            }

            var paid = await LoadOrderAsync(order.Id, cancellationToken);
            return CheckoutResult.Succeeded(paid ?? order);
        }

        public async Task FinalizePaidOrderAsync(
            int orderId,
            string? paymentReferenceId,
            CancellationToken cancellationToken = default)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var order = await LoadOrderAsync(orderId, cancellationToken);
                if (order == null)
                    throw new InvalidOperationException("Order not found.");

                if (order.Status == OrderStatus.Paid)
                {
                    await transaction.CommitAsync(cancellationToken);
                    return;
                }

                if (order.Status != OrderStatus.PendingPayment)
                    throw new InvalidOperationException($"Order cannot be finalized from status {order.Status}.");

                var claimed = await _context.Orders
                    .Where(o => o.Id == orderId && o.Status == OrderStatus.PendingPayment)
                    .ExecuteUpdateAsync(
                        s => s
                            .SetProperty(o => o.Status, OrderStatus.Paid)
                            .SetProperty(o => o.PaymentReferenceId, paymentReferenceId)
                            .SetProperty(o => o.UpdatedAt, DateTime.UtcNow),
                        cancellationToken);

                if (claimed == 0)
                {
                    await _context.Entry(order).ReloadAsync(cancellationToken);
                    if (order.Status == OrderStatus.Paid)
                    {
                        await transaction.CommitAsync(cancellationToken);
                        return;
                    }

                    throw new InvalidOperationException($"Order cannot be finalized from status {order.Status}.");
                }

                foreach (var item in order.Items)
                {
                    var affected = await _context.Books
                        .Where(b => b.Id == item.BookId && b.Quantity >= item.Quantity)
                        .ExecuteUpdateAsync(
                            s => s.SetProperty(b => b.Quantity, b => b.Quantity - item.Quantity),
                            cancellationToken);

                    if (affected == 0)
                    {
                        throw new InvalidOperationException(
                            $"Insufficient stock for book '{item.BookTitle}'.");
                    }
                }

                if (!string.IsNullOrWhiteSpace(order.CouponCode) && order.CouponDiscountTotal > 0)
                    await _couponRepo.RedeemAsync(order.CouponCode, order.UserId);

                await _shoppingCartRepo.ClearCartAsync(order.UserId);

                await transaction.CommitAsync(cancellationToken);

                order.Status = OrderStatus.Paid;
                order.PaymentReferenceId = paymentReferenceId;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<IReadOnlyList<Order>> GetAllOrdersAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByUserIdAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .Include(o => o.User)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await LoadOrderAsync(id, cancellationToken, asNoTracking: true);
        }

        private async Task MarkPaymentFailedAsync(int orderId, CancellationToken cancellationToken)
        {
            await _context.Orders
                .Where(o => o.Id == orderId && o.Status == OrderStatus.PendingPayment)
                .ExecuteUpdateAsync(
                    s => s
                        .SetProperty(o => o.Status, OrderStatus.PaymentFailed)
                        .SetProperty(o => o.UpdatedAt, DateTime.UtcNow),
                    cancellationToken);
        }

        private async Task<Order?> LoadOrderAsync(
            int orderId,
            CancellationToken cancellationToken,
            bool asNoTracking = false)
        {
            var query = _context.Orders
                .Include(o => o.Items)
                .Include(o => o.User)
                .AsQueryable();
            if (asNoTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        }
    }
}
