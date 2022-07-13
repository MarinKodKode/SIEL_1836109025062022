using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SIEL_1836109025062022.Models
{
    public class Student
    {
        public int id_student { get; set; } 
        public string stdt_name { get; set; } = "Sindatos";
        public string stdt_surname { get; set; } = "Sindatos";
        public string stdt_phone_1 { get; set; } = "Sindatos";
        public string stdt_phone_2 { get; set; } = "Sindatos";
        public string stdt_personal_email { get; set; } = "Sindatos";
        public string stdt_institutional_email { get; set; } = "Sindatos";
        public string stdt_avatar { get; set; } = "Sindatos";
        public int stdt_id_institution { get; set; } = 1;
        public int stdt_isLogged { get; set; } = 1;
        public int stdt_id_class { get; set; } = 1;
        public string stdt_hash_password { get; set; } = "hash";
        public int stdt_id_program { get; set; } = 1;
        public int stdt_age { get; set; } = 1;
        public string stdt_normalized_i_email { get; set; } = "Sindatos";
        public string stdt_nomalized_p_email { get; set; } = "Sindatos";
        public string stdt_control_number { get; set; } = "Sindatos";
    }
}
