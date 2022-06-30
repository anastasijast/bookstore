using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Bookstore.Areas.Identity.Data;
using Bookstore.Models;

namespace Bookstore.Data
{
    public class BookstoreContext : IdentityDbContext<BookstoreUser>

    {
        public BookstoreContext (DbContextOptions<BookstoreContext> options)
            : base(options)
        {
        }

        public DbSet<Bookstore.Models.Book>? Book { get; set; }

        public DbSet<Bookstore.Models.Author>? Author { get; set; }

        public DbSet<Bookstore.Models.User>? User { get; set; }

        public DbSet<Bookstore.Models.Order>? Order { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Order>()
                .HasOne<User>(d => d.User)
                .WithMany(x => x.Books)
                .HasForeignKey(d => d.UserId);
            builder.Entity<Order>()
                .HasOne<Book>(m => m.Book)
                .WithMany(x => x.Users)
                .HasForeignKey(m => m.BookId);
            builder.Entity<Book>()
                .HasOne<Author>(d => d.Author)
                .WithMany(x => x.Books)
                .HasForeignKey(m => m.AuthorId);
        }

    }
}
