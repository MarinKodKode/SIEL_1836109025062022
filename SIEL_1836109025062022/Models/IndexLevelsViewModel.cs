namespace SIEL_1836109025062022.Models
{
    public class IndexLevelsViewModel
    {
        public string program { get; set; }
        public IEnumerable<Level> levels { get; set; }
        public int summary => levels.Count();

        //public int summary => levels.Sum(x => x.summary);
    }
}