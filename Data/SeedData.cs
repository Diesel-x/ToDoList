using Microsoft.EntityFrameworkCore;

namespace ToDoList.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationContext>>()))
            {
                // Look for any movies.
                if (context.Posts.Any())
                {
                    return;   // DB has been seeded
                }
                /*context.Posts.AddRange(
                    new Post { 
                        FilePath = "aboba.jpg"
                        },
                    new Post { 
                        FilePath = "aboba.jpg"
                        },
                    new Post { 
                        FilePath = "aboba.jpg"
                        },
                    new Post { 
                        FilePath = "aboba.jpg"
                        },
                    new Post { 
                        FilePath = "aboba.jpg"
                        }
                    );
                context.SaveChanges();*/
            }
        }
    }
}
