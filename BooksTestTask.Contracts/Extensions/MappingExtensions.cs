using BooksTestTask.Contracts.Dto;
using BooksTestTask.Model;

namespace BooksTestTask.Contracts.Extensions;

public static class MappingExtensions
{
    public static Book ToModel(this BookDto bookDto)
    {
        return new Book
        {
            Id = bookDto.Id,
            Author = "123",
            Title = bookDto.Title,
            Year = bookDto.Year
        };
    }

    public static BookDto ToDto(this Book book)
    {
        return new BookDto
        {
            Id = book.Id,
            Author = book.Author,
            Title = book.Title,
            Year = book.Year
        };
    }
}
