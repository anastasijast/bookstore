using Bookstore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookstore.ViewModels
{
    public class BooksFilterVM
    {
        public IList<Book> books { get; set; }
        public SelectList Genres { get; set; }
        public String BookGenre { get; set; }
        public SelectList Authors { get; set; }
        public String BookAuthor { get; set; }
        public String SearchString { get; set; }

    }
}
