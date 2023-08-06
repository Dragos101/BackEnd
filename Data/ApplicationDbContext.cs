using API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace API.Data {
  public class ApplicationDbContext: DbContext {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<BlobImage> BlobImages { get; set; }
  }
}