using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExcaliburCodingAssignment.Models
{
    public class OrderDate
    {
        [Key]
        public int OrderID { get; set; }

        public DateTime OrderedDate { get; set; }

    }
}
