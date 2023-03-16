namespace SIEL_1836109025062022.Models
{
    public class StudentDataViewModel : User
    {
        public int id_student { get; set; }
        public int stdt_id_class { get; set; } = 1;
        public int stdt_id_program { get; set; }
        public string stdt_control_number { get; set; }
        public int stdt_is_joined_to_class { get; set; }
        public int id_level { get; set; }
        public string level_name { get; set; } = "Aun no tienes un nivel";
        public int id_program { get; set; }
        public string program_name { get; set; } = "Aun no tienes un programa";
        public int id_class { get; set; }
        public string class_name { get; set; } = "Aun no tienes una clase";
        public string inst_name { get; set; }
    }
}