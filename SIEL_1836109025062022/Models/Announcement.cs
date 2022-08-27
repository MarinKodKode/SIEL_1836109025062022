namespace SIEL_1836109025062022.Models
{
    public class Announcement
    {
        public int id_announcement { get; set; }
        public string announcement_name { get; set; }
        public string announcement_description { get; set; }
        public string announcement_picture { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string notes { get; set; }
    }
}