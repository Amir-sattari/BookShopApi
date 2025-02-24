﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace BookShopApi.Dtos.ShoppingCart
{
    public class CreateShoppingCartDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int BookId { get; set; }
    }
}
