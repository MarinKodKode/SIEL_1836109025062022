using Microsoft.AspNetCore.Mvc.Rendering;

namespace SIEL_1836109025062022.Models
{
    public class StudenIndexViewModel:CourseProgram
    {
        public IEnumerable<SelectListItem> Programs { get; set; }
    }
}
