using Microsoft.AspNetCore.Mvc.Rendering;

namespace SIEL_1836109025062022.Models
{
    public class StudenIndexViewModel : Level
    {
        public IEnumerable<SelectListItem> Programs { get; set; }

        public string program { get; set; }
        public IEnumerable<Level> levels { get; set; }

    }
}
