namespace LibraryDashboard2.Areas.Student.Models
{
    public class IssuedBook
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public DateTime IssueDate { get; set; }
    }
}
