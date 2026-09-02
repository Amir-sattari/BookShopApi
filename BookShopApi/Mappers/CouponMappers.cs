using BookShopApi.Dtos.Coupon;
using BookShopApi.Models;

namespace BookShopApi.Mappers
{
    public static class CouponMappers
    {
        public static string NormalizeCode(string code) =>
            code.Trim().ToUpperInvariant();

        public static CouponDto ToCouponDto(this Coupon coupon)
        {
            return new CouponDto
            {
                Id = coupon.Id,
                Code = coupon.Code,
                Percentage = coupon.Percentage,
                StartDate = coupon.StartDate,
                EndDate = coupon.EndDate,
                MaxUsageCount = coupon.MaxUsageCount,
                UsedCount = coupon.UsedCount,
                MaxUsagePerUser = coupon.MaxUsagePerUser,
                IsActive = coupon.IsActive,
                ApplicableBookIds = coupon.ApplicableBooks?.Select(b => b.Id).ToList() ?? new List<int>()
            };
        }

        public static Coupon ToCouponFromCreateDto(this CreateCouponDto dto)
        {
            return new Coupon
            {
                Code = NormalizeCode(dto.Code),
                Percentage = dto.Percentage,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                MaxUsageCount = dto.MaxUsageCount,
                UsedCount = 0,
                MaxUsagePerUser = dto.MaxUsagePerUser,
                IsActive = dto.IsActive
            };
        }

        public static void SetDataToCouponFromUpdateDto(this Coupon coupon, UpdateCouponDto dto)
        {
            coupon.Code = NormalizeCode(dto.Code);
            coupon.Percentage = dto.Percentage;
            coupon.StartDate = dto.StartDate;
            coupon.EndDate = dto.EndDate;
            coupon.MaxUsageCount = dto.MaxUsageCount;
            coupon.MaxUsagePerUser = dto.MaxUsagePerUser;
            coupon.IsActive = dto.IsActive;
        }
    }
}
