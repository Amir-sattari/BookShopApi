using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.ShoppingCart
{
    public class UpdateShoppingCartDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int BookId { get; set; }
    }
}
