using BookShopApi.Dtos.Coupon;
using BookShopApi.Helpers;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface ICouponRepository
    {
        Task<IEnumerable<Coupon>> GetAllAsync();
        Task<Coupon?> GetByIdAsync(int id);
        Task<Coupon> CreateAsync(CreateCouponDto dto);
        Task<Coupon?> UpdateAsync(int id, UpdateCouponDto dto);
        Task<Coupon?> DeactivateAsync(int id);
        Task<CouponValidationResult> ValidateForUseAsync(string code, string userId);
        Task RedeemAsync(string code, string userId);
    }
}
