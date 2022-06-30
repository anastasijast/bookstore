using Bookstore.Models;
using System.ComponentModel.DataAnnotations;

namespace Bookstore.ViewModels
{
    public class BookOrder
    {
        public User? user { get; set; }
        public int? bookId { get; set; }
        [Required]
        public String location { get; set; }
    }
}
