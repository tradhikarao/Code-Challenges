using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExcaliburCodingAssignment.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public OrderDate OrderDate { get; set; }

        [ForeignKey("OrderDate")]
        public int OrderId { get; set; }

        public double OrderAmount { get; set; }

        public string OrderDesc { get; set; }
    }
}
