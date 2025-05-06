namespace LibraryDashboard2.Models
{
    public class LateFeeViewModel
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string StudentName { get; set; }
        public DateTime IssueDate { get; set; }
        public int DaysOverdue { get; set; }
        public decimal Fee { get; set; }

    }
}
