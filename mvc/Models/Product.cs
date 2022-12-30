using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm.")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Display(Name = "Hãng SX")]
        public string Manufacturer { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0} đ")]
        [Display(Name = "Giá")]
        public int Price { get; set; }

        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Display(Name = "Mô tả")]
        public string Desciption { get; set; }

        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }
    }
}
