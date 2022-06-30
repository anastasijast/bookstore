using Bookstore.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bookstore.ViewModels
{
    public class MoreBooksVM
    {
        public User? user { get; set; }
        public IEnumerable<int>? selectedBooks { get; set; }
        public IEnumerable<SelectListItem>? bookList { get; set; }
        [Required]
        public String location { get; set; }

    }
}
