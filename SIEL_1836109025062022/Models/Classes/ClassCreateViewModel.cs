namespace SIEL_1836109025062022.Models.Classes
{
    public class ClassCreateViewModel : Class
    {
        //General data
        public int nstudents { get; set; }
        public int inscrptions_count { get; set; }
        public int noClassCount { get; set; }
        public IEnumerable<User> Teachers { get; set; }
        public IEnumerable<User> Adm_Institution { get; set; }
        public int studentsAssigned { get; set; }
        public int assignedClass { get; set; }

        //Scholar program data
        public string program_name { get; set; }

        //Levels data
        public string level_name { get; set; }

        //Modality data
        public string modality_name { get; set; }

        //Schedule data
        public int id_schedule { get; set; }
        public string schedule_name { get; set; }
        public string schedule_description { get; set; }

        //Teachers Data
        public string user_name { get; set; }
        public string user_surname { get; set; }
    }
}
