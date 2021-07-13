using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Assignment.Models
{
    [Table("Cart_Detail")]
    public partial class CartDetail
    {
        [Key]
        [Column("cart_id")]
        public int CartId { get; set; }
        [Key]
        [Column("product_id")]
        public int ProductId { get; set; }
        [Column("quantity")]
        public int? Quantity { get; set; }

        [ForeignKey(nameof(CartId))]
        [InverseProperty(nameof(Carts.CartDetail))]
        public virtual Carts Cart { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(Products.CartDetail))]
        public virtual Products Product { get; set; }
    }
}
