using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name ="Title")]
        public string Title { get; set; }
        [Display(Name = "Publication Date")]
        public int? PublicationDate { get; set; }
        [StringLength(30)]
        [Display(Name = "Genre")]
        public string? Genre { get; set; }
        [Display(Name = "Pages")]
        public int? Pages { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        public decimal? Price { get; set; }
        [StringLength(int.MaxValue)]
        [Display(Name = "Book Description")]
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public int? AuthorId { get; set; }
        [Display(Name = "Author")]

        public Author? Author { get; set; }
        public ICollection<Order>? Users { get; set; }

    }
}
