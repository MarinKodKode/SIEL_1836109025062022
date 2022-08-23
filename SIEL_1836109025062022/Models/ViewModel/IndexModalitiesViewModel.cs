namespace SIEL_1836109025062022.Models.ViewModel
{
    public class IndexModalitiesViewModel
    {
        public string program { get; set; }
        public string level { get; set; }
        public IEnumerable<Modality> modalities { get; set; }
        public IEnumerable<Level> levels { get; set; }
        public int summary => modalities.Count();
        public int summaryLevel => levels.Count();

    }
}
