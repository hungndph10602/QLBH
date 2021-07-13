using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Assignment.Models
{
    public partial class Products
    {
        public Products()
        {
            CartDetail = new HashSet<CartDetail>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("category_id")]
        [Display(Name="Category Name")]
        public int CategoryId { get; set; }
        [Column("price", TypeName = "money")]
        public decimal? Price { get; set; }
        [Column("img")]
        [Display(Name ="Image Name")]
        public string image { get; set; }
        [NotMapped]
        [Display(Name = "Upload File")]
        public IFormFile imagefile { get; set; }
        [Column("disable")]
        public bool Disable { get; set; }
        

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty(nameof(Categories.Products))]
        public virtual Categories Category { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<CartDetail> CartDetail { get; set; }

        public List<Products> _lstPro = new List<Products>();
    }
}
