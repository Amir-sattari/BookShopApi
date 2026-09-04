using BookShopApi.Data;
using BookShopApi.Dtos.Coupon;
using BookShopApi.Helpers;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookShopApi.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _context;

        public CouponRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<IEnumerable<Coupon>> GetAllAsync()
        {
            return await _context.Coupons
                .Include(c => c.ApplicableBooks)
                .OrderByDescending(c => c.Id)
                .ToListAsync();
        }

        public async Task<Coupon?> GetByIdAsync(int id)
        {
            return await _context.Coupons
                .Include(c => c.ApplicableBooks)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Coupon> CreateAsync(CreateCouponDto dto)
        {
            var code = CouponMappers.NormalizeCode(dto.Code);

            if (await _context.Coupons.AnyAsync(c => c.Code == code))
                throw new ArgumentException("A coupon with this code already exists.");

            var coupon = dto.ToCouponFromCreateDto();
            await AttachApplicableBooksAsync(coupon, dto.ApplicableBookIds);

            await _context.Coupons.AddAsync(coupon);
            await _context.SaveChangesAsync();
            return coupon;
        }

        public async Task<Coupon?> UpdateAsync(int id, UpdateCouponDto dto)
        {
            var coupon = await GetByIdAsync(id);
            if (coupon == null)
                return null;

            var code = CouponMappers.NormalizeCode(dto.Code);
            if (await _context.Coupons.AnyAsync(c => c.Code == code && c.Id != id))
                throw new ArgumentException("A coupon with this code already exists.");

            coupon.SetDataToCouponFromUpdateDto(dto);
            coupon.ApplicableBooks.Clear();
            await AttachApplicableBooksAsync(coupon, dto.ApplicableBookIds);

            await _context.SaveChangesAsync();
            return coupon;
        }

        public async Task<Coupon?> DeactivateAsync(int id)
        {
            var coupon = await GetByIdAsync(id);
            if (coupon == null)
                return null;

            coupon.IsActive = false;
            await _context.SaveChangesAsync();
            return coupon;
        }

        public async Task<CouponValidationResult> ValidateForUseAsync(string code, string userId)
        {
            var normalized = CouponMappers.NormalizeCode(code);
            var now = DateTime.UtcNow;

            var coupon = await _context.Coupons
                .Include(c => c.ApplicableBooks)
                .FirstOrDefaultAsync(c => c.Code == normalized);

            if (coupon == null)
                return CouponValidationResult.Fail("Coupon not found.");

            if (!coupon.IsActive)
                return CouponValidationResult.Fail("Coupon is inactive.");

            if (now < coupon.StartDate)
                return CouponValidationResult.Fail("Coupon is not yet valid.");

            if (now > coupon.EndDate)
                return CouponValidationResult.Fail("Coupon has expired.");

            if (coupon.MaxUsageCount.HasValue && coupon.UsedCount >= coupon.MaxUsageCount.Value)
                return CouponValidationResult.Fail("Coupon has reached its usage limit.");

            if (coupon.MaxUsagePerUser.HasValue)
            {
                var userUsageCount = await _context.CouponUsages
                    .CountAsync(u => u.CouponId == coupon.Id && u.UserId == userId);

                if (userUsageCount >= coupon.MaxUsagePerUser.Value)
                    return CouponValidationResult.Fail("You have already used this coupon the maximum number of times.");
            }

            return CouponValidationResult.Success(coupon);
        }

        public async Task RedeemAsync(string code, string userId)
        {
            var ownsTransaction = _context.Database.CurrentTransaction == null;
            IDbContextTransaction? transaction = null;
            if (ownsTransaction)
                transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var validation = await ValidateForUseAsync(code, userId);
                if (!validation.IsValid || validation.Coupon == null)
                    throw new CouponValidationException(validation.ErrorMessage ?? "Coupon is not valid.");

                var coupon = await _context.Coupons.FirstAsync(c => c.Id == validation.Coupon.Id);

                if (coupon.MaxUsageCount.HasValue && coupon.UsedCount >= coupon.MaxUsageCount.Value)
                    throw new CouponValidationException("Coupon has reached its usage limit.");

                coupon.UsedCount++;
                _context.CouponUsages.Add(new CouponUsage
                {
                    CouponId = coupon.Id,
                    UserId = userId,
                    UsedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();

                if (ownsTransaction && transaction != null)
                    await transaction.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (ownsTransaction && transaction != null)
                    await transaction.RollbackAsync();
                throw new CouponValidationException("Coupon usage limit was reached. Please try another code.");
            }
            catch
            {
                if (ownsTransaction && transaction != null)
                    await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                if (ownsTransaction && transaction != null)
                    await transaction.DisposeAsync();
            }
        }

        private async Task AttachApplicableBooksAsync(Coupon coupon, List<int> bookIds)
        {
            if (bookIds == null || bookIds.Count == 0)
                return;

            var distinctIds = bookIds.Distinct().ToList();
            var books = await _context.Books.Where(b => distinctIds.Contains(b.Id)).ToListAsync();

            if (books.Count != distinctIds.Count)
                throw new ArgumentException("One or more applicable book IDs are invalid.");

            foreach (var book in books)
                coupon.ApplicableBooks.Add(book);
        }
    }
}
