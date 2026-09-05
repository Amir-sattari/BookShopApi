using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.ShoppingCart
{
    public class CreateMultipleShoppingCartItemsDto
    {
        /// <summary>از claims پر می‌شود؛ کلاینت نباید بفرستد.</summary>
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int BookId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
