using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApiGetaway.Models;

namespace ApiGetaway.Data
{
    public class ApiKeyDbContext : DbContext
    {
        public DbSet<Apikey> ApiKeys { get; set; }

        public ApiKeyDbContext(DbContextOptions<ApiKeyDbContext> options) : base(options)
        {
           
        }
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Apikey>().ToTable("TB_APIAUTO");
        }
    }
}