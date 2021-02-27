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
            "Server=(localdb)\\mssqllocaldb;Database=LibraryDB;Trusted_Connection=True;";
        public DbSet<Books> Books { get; set; }
        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Books>()
                .Property(b => b.Id)
                .IsRequired();

            modelBuilder.Entity<Books>()
               .Property(b => b.ISBN)
               .IsRequired();

            modelBuilder.Entity<Books>()
               .Property(b => b.book_name)
               .IsRequired();

            modelBuilder.Entity<Books>()
              .Property(b => b.author_name)
              .IsRequired();

            modelBuilder.Entity<Books>()
              .Property(b => b.publisher_name)
              .IsRequired();

            modelBuilder.Entity<Books>()
              .Property(b => b.publish_date)
              .IsRequired();

            modelBuilder.Entity<Books>()
             .Property(b => b.actual_stock)
             .IsRequired();

            modelBuilder.Entity<Books>()
            .Property(b => b.current_stock)
            .IsRequired();
            modelBuilder.Entity<Users>()
           .Property(u => u.Id)
           .IsRequired();

            modelBuilder.Entity<Users>()
          .Property(u => u.name)
          .IsRequired();

            modelBuilder.Entity<Users>()
          .Property(u => u.surname)
          .IsRequired();

            modelBuilder.Entity<Users>()
          .Property(u => u.email)
          .IsRequired();

            modelBuilder.Entity<Users>()
         .Property(u => u.password)
         .IsRequired();

         modelBuilder.Entity<Users>()
         .Property(u => u.authorization)
         .IsRequired();


            modelBuilder.Entity<BooksUsers>().HasKey(x => new { x.BooksId, x.UsersId });


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }


    }
}
