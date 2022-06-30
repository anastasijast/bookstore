using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [StringLength(25)]
        [Display(Name = "Nationality")]
        public string? Nationality { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }
        [StringLength(int.MaxValue)]
        [Display(Name = "Biography")]
        public string? Biography { get; set; }
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public ICollection<Book>? Books { get; set; }
    }
}
