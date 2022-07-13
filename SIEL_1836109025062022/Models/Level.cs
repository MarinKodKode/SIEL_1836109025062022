using System.ComponentModel.DataAnnotations;

namespace SIEL_1836109025062022.Models
{
    public class Level
    {
        public int id_level { get; set; }
        [Required(ErrorMessage = "El nombre del nivel es requerido")]
        [StringLength(maximumLength:20, MinimumLength =5, ErrorMessage ="El campo debe tener una longitud aceptable")]
        public string level_name { get; set; }
        [Required(ErrorMessage = "Debe de asignar el nivel a un programa válido")]
        public int level_id_program { get; set; }

        [Required(ErrorMessage = "La descripción del nivel es requerido")]
        [StringLength(maximumLength: 900, MinimumLength = 5, ErrorMessage = "El campo debe tener una longitud aceptable")]
        public string level_description { get; set; }
        public string level_picture { get; set; } = "No picture";
        public int level_order { get; set; } = 1;
    }
}
