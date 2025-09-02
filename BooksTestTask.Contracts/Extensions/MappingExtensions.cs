using BooksTestTask.Contracts.Dto;
using BooksTestTask.Model;
using BooksTestTask.Model.Identity;

namespace BooksTestTask.Contracts.Extensions;

public static class MappingExtensions
{
    public static Book ToBookModel(this BookDto bookDto)
    {
        return new Book
        {
            Id = bookDto.Id,
            Author = "123",
            Title = bookDto.Title,
            Year = bookDto.Year
        };
    }

    public static BookDto ToBookDto(this Book book)
    {
        return new BookDto
        {
            Id = book.Id,
            Author = book.Author,
            Title = book.Title,
            Year = book.Year
        };
    }

    public static User ToUserModel(this RegisterUserRequest request)
    {
        return new User
        {
            UserName = request.UserName,
            Email = request.Email,
            PasswordHash = request.Password
        };
    }
}
