using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Bogus;

namespace api.Data
{
    public class DataSeeder
    {
        private readonly AppDBContext _context;
        private readonly List<string> bookGenres = new List<string>
        {
            "Fantasy", "Science Fiction", "Mystery", "Thriller",
            "Romance", "Western", "Dystopian", "Memoir", "Biography",
            "Poetry", "Young Adult", "Children"
        };

        public DataSeeder(AppDBContext context)
        {
            _context = context;
        }

        public void SeedBooks()
        {
            if(!_context.Books.Any())
            {
                var fakeBooks = new Faker<Book>()
                    .RuleFor(b => b.ISBN, f => f.Random.Long(1000000000000, 9999999999999))
                    .RuleFor(b => b.Title, f => string.Join(" ", f.Lorem.Words(f.Random.Int(1, 5))))
                    .RuleFor(b => b.Author, f => f.Name.FullName())
                    .RuleFor(b => b.Description, f => f.Lorem.Paragraph())
                    .RuleFor(b => b.CoverImage, f => f.Image.PicsumUrl())
                    .RuleFor(b => b.Publisher, f => f.Company.CompanyName())
                    .RuleFor(b => b.PublicationDate, f => f.Date.Past(10))
                    .RuleFor(b => b.Category, f => string.Join(", ", f.PickRandom(bookGenres, f.Random.Int(1, 3))))
                    .RuleFor(b => b.PageCount, f => f.Random.Int(100, 1000))
                    .Generate(100);

                _context.Books.AddRange(fakeBooks);
                _context.SaveChanges();

            }
        }
    }
}