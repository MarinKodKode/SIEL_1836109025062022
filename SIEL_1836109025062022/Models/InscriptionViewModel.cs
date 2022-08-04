namespace SIEL_1836109025062022.Models
{
    public class InscriptionViewModel : Inscription
    {
        public IEnumerable<Schedule>  Schedules { get; set; }
        public IEnumerable<Modality> Modalities { get; set; }

    }
}
