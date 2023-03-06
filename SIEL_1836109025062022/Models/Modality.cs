using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SIEL_1836109025062022.Models
{
    public class Modality
    {
        public int id_modality { get; set; }
        [Required(ErrorMessage = "El nombre de la modalidad es requerida")]
        [StringLength(
            maximumLength: 45, MinimumLength = 5,
            ErrorMessage = "El campo debe tener una longitud minima de 5 carácteres y máxima de 45")]
        //[Remote(action: "VerifyExistsModality", controller: "Modality")]
        public string modality_name { get; set; }
        [Required(ErrorMessage = "Una descripción breve de la modalidad es requerida")]
        [StringLength(
            maximumLength: 450, MinimumLength = 10,
            ErrorMessage = "El campo debe tener una longitud minima de 10 carácteres y máxima de 450")]
        public string modality_description { get; set; }
        [Required(ErrorMessage = "La duración de la modalidad es requerida")]
        public int modality_weeks_duration { get; set; }
        public int modality_order { get; set; } = 1;
        public int modality_level_id { get; set; }
        public string level_name { get; set; }
        public string program_name { get; set; }
    }
}
