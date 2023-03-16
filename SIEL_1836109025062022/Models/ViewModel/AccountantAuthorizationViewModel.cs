namespace SIEL_1836109025062022.Models.ViewModel
{
    public class AccountantAuthorizationViewModel : User
    {
        public int id_student { get; set; }
        public int stdt_id_class { get; set; }
        public int stdt_id_program { get; set; }
        public string stdt_control_number { get; set; }
        public int stdt_is_joined_to_class { get; set; }
        public int id_inscription { get; set; }
        public int insc_id_student { get; set; }
        public int insc_id_schedule { get; set; }
        public int insc_id_modality { get; set; }
        public int insc_id_level { get; set; }
        public int insc_id_course_program { get; set; }
        public int insc_status { get; set; }
        public string insc_file_one { get; set; }
        public string insc_file_two { get; set; }
        public IFormFile file_one { get; set; }
        public IFormFile file_two { get; set; }
        public DateTime insc_date_time { get; set; }
        public int insc_institution { get; set; } = 1;
        public IEnumerable<StatusIncription> status { get; set; }

    }
}