using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SIEL_1836109025062022.Models
{
    public class ReportProgram
    {
        public int id_program { get; set; }

        [Required(ErrorMessage = "La descripción del programa es requerido")]
        [StringLength(
            maximumLength: 500, MinimumLength = 5,
            ErrorMessage = "El nombre del programa debe contener al menos de 20 caracteres")]
        public string program_description { get; set; }
        [Required(ErrorMessage = "El nombre del programa es requerido")]
        [StringLength(
            maximumLength: 50, MinimumLength = 5,
            ErrorMessage = "El nombre del programa debe contener menos de 50 caracteres y más de 5")]
        //[Remote(action: "VerifyExistsCourseProgram", controller: "CourseProgram")]
        public string program_name { get; set; }
    }
}
