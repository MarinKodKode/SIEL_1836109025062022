using System.ComponentModel.DataAnnotations;

namespace SIEL_1836109025062022.Models.Classes
{
    public class Class
    {
        [Key]
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