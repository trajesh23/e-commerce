using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Business.DTOs.AuthDtos.LoginDtos
{
    public class LoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid e-mail adress.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DefaultValue("Enter your password.")]
        public string Password { get; set; } = string.Empty;
    }
}
