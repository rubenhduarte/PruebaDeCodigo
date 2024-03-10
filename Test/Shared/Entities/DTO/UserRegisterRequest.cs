using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Shared.Entities.DTO
{
    public class UserRegisterRequest
    {
        [Required(ErrorMessage = "Email es requerido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password es requerido.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "El password y su confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}
