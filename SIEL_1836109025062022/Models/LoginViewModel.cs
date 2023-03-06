
using System.ComponentModel.DataAnnotations;

namespace SIEL_1836109025062022.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Debe de colocar un correo electrónico válido")]
        [EmailAddress(ErrorMessage = " Parece que este correo no existe")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Por favor ingresa la contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool Rememberme { get; set; }
    }
}
