using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.ShoppingCart
{
    public class CreateShoppingCartDto
    {
        /// <summary>از claims پر می‌شود؛ کلاینت نباید بفرستد.</summary>
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int BookId { get; set; }
    }
}
