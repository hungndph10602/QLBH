using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Assignment.Models
{
    public partial class Users
    {
        public Users()
        {
            Carts = new HashSet<Carts>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("phone")]
        [StringLength(13)]
        public string Phone { get; set; }
        [Column("email")]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Column("password")]
        public string Password { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Carts> Carts { get; set; }
    }
}
