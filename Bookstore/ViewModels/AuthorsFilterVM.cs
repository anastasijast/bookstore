using Bookstore.Models;

namespace Bookstore.ViewModels
{
    public class AuthorsFilterVM
    {
        public IList<Author> authors { get; set; }

        public string SearchString { get; set; }
    }
}
