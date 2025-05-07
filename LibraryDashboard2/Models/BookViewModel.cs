namespace LibraryDashboard2.Models
{
    public class BookViewModel
    {
        public int BookId { get; set; }  // Unique BookId for this book (will be same for all copies)
        public string Title { get; set; }
        public string Author { get; set; }
        public int Quantity { get; set; }  // Number of copies you want to add

        public string Genre { get; set; }
    }

}
