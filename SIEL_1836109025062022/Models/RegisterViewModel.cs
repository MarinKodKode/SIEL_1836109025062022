using System.ComponentModel.DataAnnotations;

namespace SIEL_1836109025062022.Models
{
    public class RegisterViewModel
    {
        [Required (ErrorMessage = "El nombre de usuario es requerido")]
        //[DataType(DataType.Text)]
        public string user_name { get; set; }
        [Required(ErrorMessage = "El apellido de usuario es requerido")]
        public string user_surname { get; set; }
        [Required(ErrorMessage = "El correo de usuario es requerido")]
        
        public string user_personal_email { get; set; }
        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        public string user_hash_password { get; set; }
        [Required(ErrorMessage = "El telefono del usuario es requerido")]
        public string user_phone_1 { get; set; }
    }
}
