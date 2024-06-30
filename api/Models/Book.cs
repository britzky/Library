using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Book
    {
        public int ISBN { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CoverImage { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string PublicationDate { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int PageCount { get; set; }
    }
}