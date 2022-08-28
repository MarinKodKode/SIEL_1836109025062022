namespace SIEL_1836109025062022.Models.ViewModel
{
    public class ListsScheduleViewModel
    {
        public string modality { get; set; }
        public IEnumerable<Schedule> schedules { get; set; }
        public int summary => schedules.Count();

        //public int summary => levels.Sum(x => x.summary);
    }
}
