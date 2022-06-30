using Bookstore.Models;
using System.ComponentModel.DataAnnotations;

namespace Bookstore.ViewModels
{
    public class BookEditPicture
    {
        public Book? book { get; set; }

        [Display(Name = "Upload picture")]
        public IFormFile? pictureFile { get; set; }

        [Display(Name = "Picture")]
        public string? pictureName { get; set; }
    }
}
