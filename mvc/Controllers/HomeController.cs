using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc.Data;
using mvc.Models;
using Newtonsoft.Json;

namespace mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Home
        public async Task<IActionResult> Index(string search)
        {
            Random rand = new Random();
            ViewData["rand"] = rand.Next().ToString();

            //tim kiem

            var sanpham = from m in _context.Product select m;
            if (!String.IsNullOrEmpty(search))
            {
                sanpham = sanpham.Where(s => s.Name.Contains(search));
            }
            return View(await sanpham.ToListAsync());
        }
        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
 
        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        // Các phương thức liên quan đến GIỎ HÀNG

        // Đọc danh sách CartItem từ session
        List<CartItem> GetCartItems()
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString("shopcart");
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            return new List<CartItem>();
        }

        // Lưu danh sách CartItem trong giỏ hàng vào session
        void SaveCartSession(List<CartItem> list)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(list);
            session.SetString("shopcart", jsoncart);
        }

        // Xóa session giỏ hàng
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove("shopcart");
        }

        // Cho hàng vào giỏ
        public async Task<IActionResult> AddToCart(int id)
        {
            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại");
            }
            var cart = GetCartItems();
            var item = cart.Find(p => p.Product.Id == id);
            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                cart.Add(new CartItem() { Product = product, Quantity = 1 });
            }
            SaveCartSession(cart);
            return RedirectToAction(nameof(ViewCart));
        }

        // Chuyển đến view xem giỏ hàng
        public IActionResult ViewCart()
        {
            return View(GetCartItems());
        }

        public IActionResult RemoveItem(int id)
        {
            var cart = GetCartItems();
            var item = cart.Find(p => p.Product.Id == id);
            if (item != null)
            {
                cart.Remove(item);
            }
           
            SaveCartSession(cart);
            return RedirectToAction(nameof(ViewCart));
        }

        public IActionResult UpdateItem(int id, int quantity)
        {
            var cart = GetCartItems();
            var item = cart.Find(p => p.Product.Id == id);
            if (item != null)
            {
                item.Quantity = quantity;
            }

            SaveCartSession(cart);
            return RedirectToAction(nameof(ViewCart));
        }

        public IActionResult DeleteAll()
        {
            ClearCart();
            return RedirectToAction(nameof(ViewCart));
        }

        public IActionResult CheckOut()
        {
            return View(GetCartItems());
        }

        // Lập hóa đơn: lưu hóa đơn, lưu chi tiết hóa đơn
        [HttpPost, ActionName("CreateBill")]
        public async Task<IActionResult> CreateBill(string cusName, string cusPhone, string cusAddress, int billTotal)
        {
            var bill = new Bill();
            bill.Date = DateTime.Now;
            bill.CustomerName = cusName;
            bill.CustomerPhone = cusPhone;
            bill.CustomerAddress = cusAddress;
            // cập nhật tổng tiền hóa đơn ?
            bill.BillTotal = billTotal;
            _context.Add(bill);
            await _context.SaveChangesAsync();

            // thêm chi tiết hóa đơn
            var cart = GetCartItems();

            int amount = 0;
            int total = 0;
            foreach (var i in cart)
            {
                var b = new BillDetail();
                b.BillId = bill.BillId;
                b.ProductId = i.Product.Id;
                amount = i.Product.Price * i.Quantity;
                total += amount;
                b.Price = i.Product.Price;
                b.Quantity = i.Quantity;
                b.Amount = amount;
                _context.Add(b);

            }

            await _context.SaveChangesAsync();
            
            // xóa giỏ
            ClearCart();
            return RedirectToAction(nameof(Thank));
        }
        public IActionResult Thank()
        {
            return View();
        }
    }
}
