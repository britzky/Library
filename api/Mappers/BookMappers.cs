using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Dtos.Transaction;
using api.Models;

namespace api.Mappers
{
    public static class BookMappers
    {
        public static BookDto ToBookDto(this Book bookModel)
        {
            return new BookDto
            {
                Id = bookModel.Id,
                ISBN = bookModel.ISBN,
                Title = bookModel.Title,
                Author = bookModel.Author,
                Description = bookModel.Description,
                CoverImage = bookModel.CoverImage,
                Publisher = bookModel.Publisher,
                PublicationDate = bookModel.PublicationDate,
                Category = bookModel.Category,
                PageCount = bookModel.PageCount,
                Availability = bookModel.Availability
            };
        }
        public static FeaturedBookDto ToFeaturedBookDto(this Book bookModel)
        {
            return new FeaturedBookDto
            {
                Id = bookModel.Id,
                Title = bookModel.Title,
                Author = bookModel.Author,
                Description = bookModel.Description,
                CoverImage = bookModel.CoverImage,
                Availability = bookModel.Availability
            };
        }

        public static BookDetailDto ToBookDetailDto(this Book bookModel)
        {
            return new BookDetailDto
            {
                ISBN = bookModel.ISBN,
                Title = bookModel.Title,
                Author = bookModel.Author,
                Description = bookModel.Description,
                CoverImage = bookModel.CoverImage,
                Publisher = bookModel.Publisher,
                PublicationDate = bookModel.PublicationDate,
                Category = bookModel.Category,
                PageCount = bookModel.PageCount,
                Availability = bookModel.Availability
            };
        }

        public static Book ToCreateBookDto(this CreateBookDto bookModel)
        {
            return new Book
            {
                ISBN = bookModel.ISBN,
                Title = bookModel.Title,
                Author = bookModel.Author,
                Description = bookModel.Description,
                CoverImage = bookModel.CoverImage,
                Publisher = bookModel.Publisher,
                PublicationDate = bookModel.PublicationDate,
                Category = bookModel.Category,
                PageCount = bookModel.PageCount,
                Availability = bookModel.Availability
            };
        }

        public static BookTransactionDto ToBookTransactionDto(this BookTransaction transaction)
        {
            return new BookTransactionDto
            {
                Id = transaction.Id,
                BookId = transaction.BookId,
                BookTitle = transaction.Book.Title,
                UserId = transaction.UserId,
                UserName = transaction.User.UserName,
                CheckoutDate = transaction.CheckoutDate,
                DueDate = transaction.DueDate,
                IsReturned = transaction.IsReturned
            };
        }

        public static CheckedOutBookDto ToCheckedOutBookDto(this BookTransaction transaction)
        {
            return new CheckedOutBookDto
            {
                TransactionId = transaction.Id,
                Title = transaction.Book.Title,
                Author = transaction.Book.Author,
                Description = transaction.Book.Description,
                CoverImage = transaction.Book.CoverImage,
                DueDate = transaction.DueDate
            };
        }

    }
}