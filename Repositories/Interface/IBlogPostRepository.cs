using API.Models.Domain;

namespace API.Repositories.Interface {
  public interface IBlogPostRepository {
    
    Task<BlogPost> CreateAsync(BlogPost blogPost);

    Task<IEnumerable<BlogPost>> GetAllAsync();

    Task<BlogPost?> getByIdAsync(Guid id);

    Task<BlogPost?> getByUrlHandleAsync(string urlHandle);

    Task<BlogPost?> UpdateAsync(BlogPost blogPost);

    Task<BlogPost?> DeleteAsync(Guid id);
  }
}