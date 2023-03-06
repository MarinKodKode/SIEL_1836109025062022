using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SIEL_1836109025062022.Models
{
    public class Student : User
    {
        public int id_student { get; set; }
        public int stdt_id_class { get; set; } = 1;
        public int stdt_id_program { get; set; }
        public string stdt_control_number { get; set; } = "Sindatos";
        public int stdt_is_joined_to_class { get; set; }

    }
}