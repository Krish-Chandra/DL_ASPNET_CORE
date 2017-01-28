using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Models
{
    public class Category
    {
        public Category()
        {
            Books = new HashSet<Book>();
        }
        public int CategoryID { get; set; }
        [Required(ErrorMessage="Please enter the name of the category")]
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Book> Books { get; set; }

    }
}
