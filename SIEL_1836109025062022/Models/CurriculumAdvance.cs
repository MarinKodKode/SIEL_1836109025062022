namespace SIEL_1836109025062022.Models
{
    public class CurriculumAdvance
    {
        public int id_register_curriculum_advace { get; set; }
        public int crlm_id_student { get; set; }
        public int crlm_id_level { get; set; }
        public int id_status_level { get; set; }
        public string crlm_notes { get; set; }
        public string crlm_certified_path { get; set; }
        public double crlm_final_mark { get; set; }
        public DateTime crlm_start_date { get; set; }
        public DateTime crlm_end_date { get; set; }
    }
}