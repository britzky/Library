using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
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
                Title = bookModel.Title,
                Author = bookModel.Author,
                Description = bookModel.Description,
                CoverImage = bookModel.CoverImage
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

    }
}