using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc.Data;
using mvc.Models;

namespace mvc.Controllers
{
    public class BillDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BillDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BillDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BillDetail.Include(b => b.Bill).Include(b => b.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BillDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billDetail = await _context.BillDetail
                .Include(b => b.Bill)
                .Include(b => b.Product)
                .FirstOrDefaultAsync(m => m.BillDetailId == id);
            if (billDetail == null)
            {
                return NotFound();
            }

            return View(billDetail);
        }

        // GET: BillDetails/Create
        public IActionResult Create()
        {
            ViewData["BillId"] = new SelectList(_context.Bill, "BillId", "CustomerAddress");
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name");
            return View();
        }

        // POST: BillDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BillDetailId,BillId,ProductId,Price,Quantity,Amount")] BillDetail billDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(billDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BillId"] = new SelectList(_context.Bill, "BillId", "CustomerAddress", billDetail.BillId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", billDetail.ProductId);
            return View(billDetail);
        }

        // GET: BillDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billDetail = await _context.BillDetail.FindAsync(id);
            if (billDetail == null)
            {
                return NotFound();
            }
            ViewData["BillId"] = new SelectList(_context.Bill, "BillId", "CustomerAddress", billDetail.BillId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", billDetail.ProductId);
            return View(billDetail);
        }

        // POST: BillDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BillDetailId,BillId,ProductId,Price,Quantity,Amount")] BillDetail billDetail)
        {
            if (id != billDetail.BillDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillDetailExists(billDetail.BillDetailId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BillId"] = new SelectList(_context.Bill, "BillId", "CustomerAddress", billDetail.BillId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", billDetail.ProductId);
            return View(billDetail);
        }

        // GET: BillDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billDetail = await _context.BillDetail
                .Include(b => b.Bill)
                .Include(b => b.Product)
                .FirstOrDefaultAsync(m => m.BillDetailId == id);
            if (billDetail == null)
            {
                return NotFound();
            }

            return View(billDetail);
        }

        // POST: BillDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var billDetail = await _context.BillDetail.FindAsync(id);
            _context.BillDetail.Remove(billDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillDetailExists(int id)
        {
            return _context.BillDetail.Any(e => e.BillDetailId == id);
        }
    }
}
