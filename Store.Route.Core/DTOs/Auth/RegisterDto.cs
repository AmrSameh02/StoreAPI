using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Core.DTOs.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "DisplayName is required")]
        public string DisplayName { get; set; }
        [Required(ErrorMessage = "PhoneNumber is required")]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }    
    }
}
