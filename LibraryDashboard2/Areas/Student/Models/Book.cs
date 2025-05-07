namespace LibraryDashboard2.Areas.Student.Models
{
    public class Book
    {
        public int Id { get; set; }  // Unique ID for each copy
        public string Title { get; set; }
        public string Author { get; set; }
        public int BookId { get; set; }  // Shared BookId across all copies of the same book

        public string Genre { get; set; }

        public bool IsIssued { get; set; } = false;  // Track if this specific copy is issued or not
        public string? IssuedTo { get; set; }  // Who the book is issued to (if applicable)
        public DateTime? IssueDate { get; set; }  // When the book was issued (if applicable)

    }
}
