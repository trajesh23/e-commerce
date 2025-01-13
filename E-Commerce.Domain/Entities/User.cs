using E_Commerce.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class User : IdentityUser
    {
        //public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        //public string Email { get; set; } = string.Empty;
        //public string PhoneNumber { get; set; } = string.Empty;
        //public string Password { get; set; }
        public UserRole Role { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
