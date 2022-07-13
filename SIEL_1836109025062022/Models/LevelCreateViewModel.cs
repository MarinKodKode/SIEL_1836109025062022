using Microsoft.AspNetCore.Mvc.Rendering;

namespace SIEL_1836109025062022.Models
{
    public class LevelCreateViewModel : Level
    {
        public IEnumerable<SelectListItem> Programs { get; set; }
    }
}
