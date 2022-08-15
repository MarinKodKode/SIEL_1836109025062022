namespace SIEL_1836109025062022.Models.ViewModel
{
    public class LevelElectionViewModel : Level
    {
        public int id_program { get; set; }
        public string program_description { get; set; }
        public string program_iname { get; set; }
        public IEnumerable<Level> Levels { get; set; }
    }
}
