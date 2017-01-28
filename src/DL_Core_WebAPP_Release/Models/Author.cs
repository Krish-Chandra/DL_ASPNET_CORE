using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Models
{
    public class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }
        public int AuthorID { get; set; }
        [Required(ErrorMessage="Please enter the name of the author")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PinCode { get; set; }
        [Required(ErrorMessage = "Please enter the email ID of the author")]
        [EmailAddress(ErrorMessage = "Please enter the email ID in correct format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter the phone number of the author")]
        public string Phone { get; set; }


        public virtual ICollection<Book> Books { get; set; }

    }
}
