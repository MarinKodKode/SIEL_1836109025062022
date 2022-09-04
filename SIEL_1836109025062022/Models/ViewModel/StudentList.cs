namespace SIEL_1836109025062022.Models.ViewModel
{
    public class StudentList : User
    {
        //Student props
        public int id_student { get; set; }
        public int stdt_id_class { get; set; }
        public int stdt_id_program { get; set; }
        public string stdt_control_number { get; set; }
        public int stdt_is_joined_to_class { get; set; }
        public int id_level { get; set; }
        public string level_name { get; set; }
        public int id_program { get; set; }
        public string program_name { get; set; }
        public int id_class { get; set; }
        public string class_name { get; set; }
        public string inst_name { get; set; }
        //Inscription props
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