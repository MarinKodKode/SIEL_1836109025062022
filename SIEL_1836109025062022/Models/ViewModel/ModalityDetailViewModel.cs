namespace SIEL_1836109025062022.Models.ViewModel
{
    public class ModalityDetailViewModel : Level
    {
        public int id_modality { get; set; }
        public string modality_name { get; set; }
        public string modality_description { get; set; }
        public int modality_weeks_duration { get; set; }
        public int modality_order { get; set; }

        public int modality_level_id { get; set; }
    }
}