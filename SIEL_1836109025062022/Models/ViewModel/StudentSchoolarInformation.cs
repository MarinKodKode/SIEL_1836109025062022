namespace SIEL_1836109025062022.Models.ViewModel
{
    public class StudentSchoolarInformation : User
    {
        //Student List
        public int id_student { get; set; }
        public int stdt_id_class { get; set; } = 1;
        public int stdt_id_program { get; set; }
        public string stdt_control_number { get; set; } = "Sindatos";
        public int stdt_is_joined_to_class { get; set; }

        // Program
        public int id_program { get; set; }
        public string program_description { get; set; }
        public string program_name { get; set; }

        //Level
        public int id_level { get; set; }
        public string level_name { get; set; }
        public int level_id_program { get; set; }
        public string level_description { get; set; }
        public string level_picture { get; set; }
        public IFormFile level_file_picture { get; set; }
        public int level_order { get; set; }

        //Modality
        public int id_modality { get; set; }
        public string modality_name { get; set; }
        public string modality_description { get; set; }
        public int modality_weeks_duration { get; set; }
        public int modality_order { get; set; }
        public int modality_level_id { get; set; }

        //Schedule
        public int id_schedule { get; set; }
        public string schedule_name { get; set; }
        public string schedule_description { get; set; }
        public int schedule_level { get; set; }
        public int schedule_modality { get; set; }

        //Class
        public int id_class { get; set; }
        public string group_name { get; set; }
        public int g_minimun_student { get; set; }
        public int g_maximum_student { get; set; }
        public DateTime gruop_start_date { get; set; }
        public DateTime gruop_end_date { get; set; }
        public int gruop_id_mode { get; set; }
        public bool group_isClosed { get; set; }
        public int group_id_responsable { get; set; }
        public int group_id_teacher { get; set; }
        public int group_id_schedule { get; set; }

    }
}