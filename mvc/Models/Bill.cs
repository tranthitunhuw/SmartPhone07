using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.Models
{
    public class Bill
    {
		public int BillId { get; set; }
		[DataType(DataType.Date)]
		[Display(Name = "Ngày hóa đơn")]
		public DateTime Date { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập họ tên.")]
		[Display(Name = "Tên khách hàng")]
		public string CustomerName { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
		[Display(Name = "Điện thoại khách hàng")]
		public string CustomerPhone { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng.")]
		[Display(Name = "Địa chỉ khách hàng")]
		public string CustomerAddress { get; set; }

		[DisplayFormat(DataFormatString = "{0:#,##0} đ")]
		[Display(Name = "Tổng trị giá")]
		public int BillTotal { get; set; }

		public ICollection<BillDetail> BillDetails { get; set; }
	}
}
