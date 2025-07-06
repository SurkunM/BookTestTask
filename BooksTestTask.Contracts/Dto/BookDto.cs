using System.ComponentModel.DataAnnotations;

namespace BooksTestTask.Contracts.Dto;

public class BookDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is null")]
    public required string Title { get; set; }


    [Required(ErrorMessage = "Author is null")]
    public required string Author { get; set; }


    [Required(ErrorMessage = "Year is null")]
    public required string Year { get; set; }
}
