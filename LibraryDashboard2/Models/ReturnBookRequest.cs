using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace LibraryDashboard2.Models
{
    public class ReturnBookRequest
    {

        public int Id { get; set; }
        public int BookId { get; set; }
        public string Username { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
