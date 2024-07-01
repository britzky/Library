using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Book
    {
        public int Id { get; set; }
        public long ISBN { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CoverImage { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public string Category { get; set; } = string.Empty;
        public int PageCount { get; set; }
        public bool Availability { get; set; } = true;
    }
}