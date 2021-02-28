using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Models
{
    public class LibraryDBContext: DbContext
    {
        private string _connectionString =
            "Server=tcp:libraryapi1.database.windows.net,1433;Initial Catalog=LibraryApi;Persist Security Info=False;User ID=adminlibrary;Password=library123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
        public DbSet<Books> Books { get; set; }
        public DbSet<Users> Users { get; set; }
        

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
          
           

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }


    }
}
