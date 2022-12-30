using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.Models
{
    public class BillDetail
    {
		public int BillDetailId { get; set; }

		public int BillId { get; set; }

		public int ProductId { get; set; }

		public int Price { get; set; }

		public int Quantity { get; set; }

		public int Amount { get; set; }

		public Bill Bill { get; set; }

		public Product Product { get; set; }
	}
}
