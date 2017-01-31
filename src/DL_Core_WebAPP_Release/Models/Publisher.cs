using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DL_Core_WebAPP_Release.Models
{
    public class Publisher
    {
        public Publisher()
        {
            Books = new HashSet<Book>();
        }
        public int PublisherID { get; set; }

        [Required(ErrorMessage = "Please enter the name of the publisher")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PinCode { get; set; }
        [Required(ErrorMessage = "Please enter the email ID of the publisher")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter the phone of the publisher")]
        public string Phone { get; set; }


        public virtual ICollection<Book> Books { get; set; }

    }
}
