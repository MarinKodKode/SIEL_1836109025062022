namespace SIEL_1836109025062022.Models
{
    public class Inscription
    {
        public int id_inscription { get; set; }
        public int insc_id_student { get; set; }
        public int insc_id_schedule { get; set; }
        public int insc_id_modality { get; set; }
        public int insc_id_level { get; set; }
        public int insc_id_course_program { get; set; }
        public string insc_file_one { get; set; }
        public string insc_file_two { get; set; }
        public IFormFile file_one { get; set; }
        public IFormFile file_two { get; set; }
        public DateTime insc_date_time { get; set; }
        public int insc_institution { get; set; } = 1;
        public int insc_status { get; set; }
    }
}