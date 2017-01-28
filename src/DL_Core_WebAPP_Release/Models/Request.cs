using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Models
{
    public class Request
    {
        public int RequestID { get; set; }
        public string RequesedOn { get; set; }

        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        [MaxLength(450)]
        public string UserId { get; set; }

    }
}
