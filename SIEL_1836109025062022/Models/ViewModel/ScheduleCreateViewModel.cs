using Microsoft.AspNetCore.Mvc.Rendering;

namespace SIEL_1836109025062022.Models.ViewModel
{
    public class ScheduleCreateViewModel : Schedule
    {
        public IEnumerable<Level> Levels { get; set; }
        public IEnumerable<Modality> Modalities { get; set; }
    }
}
