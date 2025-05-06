namespace LibraryDashboard2.Areas.Student.Models
{
    public class LateFeeView
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string StudentName { get; set; }
        public DateTime IssueDate { get; set; }
        public int DaysOverdue { get; set; }
        public decimal Fee { get; set; }

    }
}
