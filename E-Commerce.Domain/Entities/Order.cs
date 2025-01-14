using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal TotalAmount { get; set; }

        [ForeignKey("CustomerId")]
        public string CustomerId { get; set; }
        public User Customer { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
