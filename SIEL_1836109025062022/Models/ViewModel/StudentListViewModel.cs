namespace SIEL_1836109025062022.Models.ViewModel
{
    public class StudentListViewModel
    {
        public string schedule { get; set; }
        public IEnumerable<StudentList> students { get; set; }
        public int summary => students.Count();

        //public int summary => levels.Sum(x => x.summary);
    }
}
