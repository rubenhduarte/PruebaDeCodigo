using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Shared.Entities.DTO
{
    public class UserAuthenticationRequest
    {

        [Required(ErrorMessage = "Email es requerido.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password es requerido.")]
        public string? Password { get; set; }
    }
   
}
