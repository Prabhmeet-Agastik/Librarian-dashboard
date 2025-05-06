namespace LibraryDashboard2.Areas.Student.Models
{
    public class ReturnBookRequest
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string Username { get; set; } 
        public DateTime RequestDate { get; set; }
        
    }

}
