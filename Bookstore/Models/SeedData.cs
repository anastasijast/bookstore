using Bookstore.Areas.Identity.Data;
using Bookstore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<BookstoreUser>>();
            IdentityResult roleResult;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            BookstoreUser user = await UserManager.FindByEmailAsync("admin@bookstore.com");
            if (user == null)
            {
                var User = new BookstoreUser();
                User.Email = "admin@bookstore.com";
                User.UserName = "admin@bookstore.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }
            var x = await RoleManager.RoleExistsAsync("User");
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = "User";
                await RoleManager.CreateAsync(role);
            }
        }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BookstoreContext(
               serviceProvider.GetRequiredService<
                   DbContextOptions<BookstoreContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();

                if (context.Author.Any() || context.Book.Any() || context.User.Any() || context.Order.Any())
                {
                    return;
                }
                context.Author.AddRange(
                    new Author { FirstName = "Fyodor", LastName = "Dostoevsky", BirthDate = DateTime.Parse("1821-11-11") },
                    new Author { FirstName = "Leo", LastName = "Tolstoy", BirthDate = DateTime.Parse("1828-9-9") },
                    new Author { FirstName = "Charles", LastName = "Bukowski", BirthDate = DateTime.Parse("1920-8-16") },
                    new Author { FirstName = "William", LastName = "Shakespeare", BirthDate = DateTime.Parse("1616-4-23") },
                    new Author { FirstName = "Franz", LastName = "Kafka", BirthDate = DateTime.Parse("1883-7-3") }
                    );
                context.SaveChanges();
                context.Book.AddRange(
                    new Book {
                        Title = "Crime and Punishment",
                        PublicationDate = 1866,
                        Genre = "Psychological Fiction",
                        Pages = 720,
                        Price = 15.99M,
                        Description = "Crime and Punishment follows the mental anguish and moral dilemmas of Rodion Raskolnikov, an impoverished ex-student in Saint Petersburg who plans to kill an unscrupulous pawnbroker, an old woman who stores money and valuable objects in her flat. He theorises that with the money he could liberate himself from poverty and go on to perform great deeds, and seeks to convince himself that certain crimes are justifiable if they are committed in order to remove obstacles to the higher goals of 'extraordinary' men. Once the deed is done, however, he finds himself racked with confusion, paranoia, and disgust. His theoretical justifications lose all their power as he struggles with guilt and horror and confronts both the internal and external consequences of his deed.",
                        Picture = null,
                        AuthorId = 1 },
                    new Book
                    {
                        Title = "War and Peace",
                        PublicationDate = 1869,
                        Genre = "Historical novel",
                        Pages = 1225,
                        Price = 13.22M,
                        Description = "War and Peace broadly focuses on Napoleon’s invasion of Russia in 1812 and follows three of the most well-known characters in literature: Pierre Bezukhov, the illegitimate son of a count who is fighting for his inheritance and yearning for spiritual fulfillment; Prince Andrei Bolkonsky, who leaves his family behind to fight in the war against Napoleon; and Natasha Rostov, the beautiful young daughter of a nobleman who intrigues both men.",
                        Picture = null,
                        AuthorId = 2 },
                    new Book
                    {
                        Title = "Anna Karenina",
                        PublicationDate = 1878,
                        Genre = "Novel",
                        Pages = 864,
                        Price = 15.69M,
                        Description = "A complex novel in eight parts, with more than a dozen major characters, Anna Karenina is spread over more than 800 pages, typically contained in two volumes. It deals with themes of betrayal, faith, family, marriage, Imperial Russian society, desire, and rural vs. city life. The story centers on an extramarital affair between Anna and dashing cavalry officer Count Alexei Kirillovich Vronsky that scandalizes the social circles of Saint Petersburg and forces the young lovers to flee to Italy in a search for happiness, but after they return to Russia, their lives further unravel.",
                        Picture = null,
                        AuthorId = 2
                    },
                    new Book
                    {
                        Title = "Romeo and Juliet",
                        PublicationDate = 1597,
                        Genre = "Tragedy",
                        Pages = 480,
                        Price = 12.89M,
                        Description = "An age-old vendetta between two powerful families erupts into bloodshed. A group of masked Montagues risk further conflict by gatecrashing a Capulet party. A young lovesick Romeo Montague falls instantly in love with Juliet Capulet, who is due to marry her father's choice, the County Paris.",
                        Picture = null,
                        AuthorId = 4
                    }
                    );
                context.SaveChanges();
                context.User.AddRange(
                    new User
                    {
                        FirstName = "Anastasija",
                        LastName = "Stefanovska",
                        ProfilePicture = null
                    },
                    new User
                    {
                        FirstName = "Ana",
                        LastName = "Aleksova",
                        ProfilePicture = null
                    },
                    new User
                    {
                        FirstName = "Elena",
                        LastName = "Jovanova",
                        ProfilePicture = null
                    });
                context.SaveChanges();
                context.Order.AddRange(
                    new Order {  Status="Pending Approval", UserId = 1, BookId = 2 },
                    new Order { Status = "Approved", UserId = 1, BookId = 1 },
                    new Order { Status = "Approved", UserId = 3, BookId = 4 },
                    new Order { Status = "Approved", UserId = 2, BookId = 3 });
                context.SaveChanges();
            }
        }
    }
}