namespace SIEL_1836109025062022.Models
{
    public class User
    {
        public int id_user { get; set; }
        public string user_name { get; set; }
        public string user_surname { get; set; }
        public string user_personal_email { get; set; }
        public string user_institution_email { get; set; }
        public int user_id_institution { get; set; }
        public int user_id_role { get; set; }
        public IFormFile file_user_profile_picture { set; get; }
        public string user_profile_picture { get; set; }
        public string user_hash_password { get; set; }
        public string user_normalized_email_p { get; set; }
        public string user_normalized_email_i { get; set; }
        public string user_phone_1 { get; set; }
        public string user_phone_2 { get; set; }
    }
}