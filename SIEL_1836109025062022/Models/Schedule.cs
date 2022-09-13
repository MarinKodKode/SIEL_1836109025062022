namespace SIEL_1836109025062022.Models
{
    public class Schedule
    {
        public int id_schedule { get; set; } 
        public string schedule_name { get; set; }
        public string schedule_description { get; set; }
        public int schedule_level { get; set; }
        public int schedule_modality { get; set; }

        //Program data
        public int id_program { get; set; }
        public string program_description { get; set; }
        public string program_name { get; set; }

        //Level data
        public int id_level { get; set; }
        public string level_name { get; set; }
        public int level_id_program { get; set; }
        public string level_description { get; set; }
        public string level_picture { get; set; }

        //Modality data

        public int id_modality { get; set; }
        public string modality_name { get; set; }
        public string modality_description { get; set; }
        public int modality_weeks_duration { get; set; }
        public int modality_order { get; set; }
        public int modality_level_id { get; set; }
    }
}
