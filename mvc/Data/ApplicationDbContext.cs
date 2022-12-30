using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using mvc.Models;

namespace mvc.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<mvc.Models.Product> Product { get; set; }
        public DbSet<mvc.Models.Bill> Bill { get; set; }
        public DbSet<mvc.Models.BillDetail> BillDetail { get; set; }
    }
}
