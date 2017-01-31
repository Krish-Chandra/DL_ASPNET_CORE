using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL_Core_WebAPP_Release.Models
{
    public class Role
    {
        public string Id { get; set; }
        [Required(ErrorMessage="Please enter the name of the role")]
        public string Name { get; set; }
        public string[] Claims { get; set; }
    }
}
