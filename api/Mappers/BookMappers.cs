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

    }
}