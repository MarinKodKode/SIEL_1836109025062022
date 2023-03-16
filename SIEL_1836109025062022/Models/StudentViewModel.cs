using System.ComponentModel.DataAnnotations;

namespace SIEL_1836109025062022.Models
{
    public class StudentViewModel
    {
        public int id_student { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string stdt_name { get; set; } = "Sindatos";
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string stdt_surname { get; set; } = "Sindatos";
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string stdt_phone_1 { get; set; } = "Sindatos";
        public string stdt_phone_2 { get; set; } = "Sindatos";
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo deber ser un correo electrónico válido")]
        public string stdt_personal_email { get; set; } = "Sindatos";
        public string stdt_institutional_email { get; set; } = "Sindatos";
        public string stdt_avatar { get; set; } = "Sindatos";
        public int stdt_id_institution { get; set; } = 10;
        public int stdt_isLogged { get; set; }
        public int stdt_id_class { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Password)]
        public string stdt_hash_password { get; set; } = "Sindatos";
        public int stdt_id_program { get; set; }
        public int stdt_age { get; set; }
        public string stdt_normalized_i_email { get; set; } = "Sindatos";
        public string stdt_nomalized_p_email { get; set; } = "Sindatos";
        public string stdt_control_number { get; set; } = "Sindatos";
    }
}