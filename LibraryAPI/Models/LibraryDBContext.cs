using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Models
{
    public class LibraryDBContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserBookReservation> UserBookReservations { get; set; }
        public LibraryDBContext(DbContextOptions<LibraryDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                  .HasIndex(u => u.Email)
                  .IsUnique();

            modelBuilder.Entity<Role>()
                .Property(u => u.Name)
                .IsRequired();

            modelBuilder.Entity<UserBookReservation>()
               .HasKey(x => new { x.UserId, x.BookId });
        }
    }
}
