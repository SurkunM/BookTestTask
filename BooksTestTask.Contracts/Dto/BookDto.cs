namespace BooksTestTask.Contracts.Dto;

public class BookDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public required string Author { get; set; }

    public required string Year { get; set; }
}
