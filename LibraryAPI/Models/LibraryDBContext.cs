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
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                  .HasIndex(u => u.Email)
                  .IsUnique();


            modelBuilder.Entity<Role>()
                .Property(u => u.Name)
                .IsRequired();
           

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }


    }
}
