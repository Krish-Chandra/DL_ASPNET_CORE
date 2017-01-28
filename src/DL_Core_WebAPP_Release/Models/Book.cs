using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Models
{

    public class Book
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Please enter the title of the book")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter the ISBN of the book")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Please enter the total number of copies of the book")]
        [Range(1, 100, ErrorMessage = "Total copies of the book must be between 1 and 100")]
        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }

        [Display(Name ="Author")]
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [Display(Name = "Publisher")]
        public int PublisherId { get; set; }
        public virtual Publisher Publisher { get; set; }
    }

}
