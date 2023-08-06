using API.Models.Domain;
using API.Models.Domain.DTO;
using API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controlled {

  [Route("api/[controller]")]
  [ApiController]
  public class BlogPostsController : ControllerBase {
    
    private readonly IBlogPostRepository blogPostRepository;
    private readonly ICategoryRepository categoryRepository;
    public BlogPostsController(IBlogPostRepository blogPostRepository,
    ICategoryRepository categoryRepository) {
      this.blogPostRepository = blogPostRepository;
      this.categoryRepository = categoryRepository;
    }

    [HttpPost]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request) {
      // convert from DTO to Domain Model
      var blogPost = new BlogPost {
        Author = request.Author,
        Title = request.Title,
        Content = request.Content,
        FeatureImageUrl = request.FeatureImageUrl,
        IsVisible = request.IsVisible,
        PublishedData = request.PublishedData,
        ShortDescription = request.ShortDescription,
        UrlHandle = request.UrlHandle,
        Categories = new List<Category>()
      };

      foreach (var categoryGuid in request.Categories){
        var existingCategory = await categoryRepository.GetById(categoryGuid);
        if (existingCategory != null) {
          blogPost.Categories.Add(existingCategory);
        }
      }

      blogPost = await blogPostRepository.CreateAsync(blogPost);

      // convert Domain Model to DTO
      var response = new BlogPostDto {
        Id = blogPost.Id,
        Author = blogPost.Author,
        Content = blogPost.Content,
        FeatureImageUrl = blogPost.FeatureImageUrl,
        IsVisible = blogPost.IsVisible,
        PublishedData = blogPost.PublishedData,
        ShortDescription = blogPost.ShortDescription,
        Title = blogPost.Title,
        UrlHandle = blogPost.UrlHandle,
        Categories = blogPost.Categories.Select(x => new CategoryDto {
          Id = x.Id,
          Name = x.Name,
          UrlHandle = x.UrlHandle,
        }).ToList(),
      };

      return Ok(response);
    }
  
    [HttpGet]
    public async Task<IActionResult> GetAllBlogPosts() {
      var blogPosts = await blogPostRepository.GetAllAsync();

      // conver domain model to DTO
      var response = new List<BlogPostDto>();
      foreach (var blogPost in blogPosts) {
        response.Add(new BlogPostDto {
          Id = blogPost.Id,
          Author = blogPost.Author,
          Content = blogPost.Content,
          FeatureImageUrl = blogPost.FeatureImageUrl,
          IsVisible = blogPost.IsVisible,
          PublishedData = blogPost.PublishedData,
          ShortDescription = blogPost.ShortDescription,
          Title = blogPost.Title,
          UrlHandle = blogPost.UrlHandle,
          Categories = blogPost.Categories.Select(x => new CategoryDto {
            Id = x.Id,
            Name = x.Name,
            UrlHandle = x.UrlHandle,
          }).ToList()
        });
      }

      return Ok(response);
    }
  
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetBlogPost([FromRoute] Guid id) {
      var blogPost = await blogPostRepository.getByIdAsync(id);

      if(blogPost == null) {
        return NotFound();
      }

      // convert domain model to DTO
      var response = new BlogPostDto {
        Id = blogPost.Id,
        Author = blogPost.Author,
        Content = blogPost.Content,
        FeatureImageUrl = blogPost.FeatureImageUrl,
        IsVisible = blogPost.IsVisible,
        PublishedData = blogPost.PublishedData,
        ShortDescription = blogPost.ShortDescription,
        Title = blogPost.Title,
        UrlHandle = blogPost.UrlHandle,
        Categories = blogPost.Categories.Select(x => new CategoryDto {
          Id = x.Id,
          Name = x.Name,
          UrlHandle = x.UrlHandle,
        }).ToList()
      };

      return Ok(response);
    }

    [HttpGet]
    [Route("{urlHandle}")]
    public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle) {
      // get blogpost details from repository
      var blogPost = await blogPostRepository.getByUrlHandleAsync(urlHandle);

      if(blogPost == null) {
        NotFound();
      }

      var response = new BlogPostDto {
        Id = blogPost.Id,
        Author = blogPost.Author,
        Content = blogPost.Content,
        FeatureImageUrl = blogPost.FeatureImageUrl,
        IsVisible = blogPost.IsVisible,
        PublishedData = blogPost.PublishedData,
        ShortDescription = blogPost.ShortDescription,
        Title = blogPost.Title,
        UrlHandle = blogPost.UrlHandle,
        Categories = blogPost.Categories.Select(x => new CategoryDto {
          Id = x.Id,
          Name = x.Name,
          UrlHandle = x.UrlHandle,
        }).ToList()
      };

      return Ok(response);

    }

    [HttpPut]
    [Route("{id:Guid}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> UpdateBlogPost([FromRoute] Guid id, UpdateBlogPostRequestDto request) {
      // convert from DTO to Domain model
      var blogPost = new BlogPost {
        Id = id,
        Author = request.Author,
        Title = request.Title,
        Content = request.Content,
        FeatureImageUrl = request.FeatureImageUrl,
        IsVisible = request.IsVisible,
        PublishedData = request.PublishedData,
        ShortDescription = request.ShortDescription,
        UrlHandle = request.UrlHandle,
        Categories = new List<Category>()
      };

      foreach(var categoryGuid in request.Categories) {
        var existingCategory = await categoryRepository.GetById(categoryGuid);
        if(existingCategory != null) {
          blogPost.Categories.Add(existingCategory);
        }
      }
      // call the repository
      var updatedBlogPost = await blogPostRepository.UpdateAsync(blogPost);

      if(updatedBlogPost == null) {
        return NotFound();
      }

      var response = new BlogPostDto {
        Id = blogPost.Id,
        Author = blogPost.Author,
        Content = blogPost.Content,
        FeatureImageUrl = blogPost.FeatureImageUrl,
        IsVisible = blogPost.IsVisible,
        PublishedData = blogPost.PublishedData,
        ShortDescription = blogPost.ShortDescription,
        Title = blogPost.Title,
        UrlHandle = blogPost.UrlHandle,
        Categories = blogPost.Categories.Select(x => new CategoryDto {
          Id = x.Id,
          Name = x.Name,
          UrlHandle = x.UrlHandle,
        }).ToList(),
      };

      return Ok(response);

    }
  
    [HttpDelete]
    [Route("{id:Guid}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id) {
      var deletedBlogPost = await blogPostRepository.DeleteAsync(id);

      if(deletedBlogPost == null) {
        return NotFound();
      }

      // convert Domain model to DTO
      var response = new BlogPostDto {
        Id = deletedBlogPost.Id,
        Author = deletedBlogPost.Author,
        Content = deletedBlogPost.Content,
        FeatureImageUrl = deletedBlogPost.FeatureImageUrl,
        IsVisible = deletedBlogPost.IsVisible,
        PublishedData = deletedBlogPost.PublishedData,
        ShortDescription = deletedBlogPost.ShortDescription,
        Title = deletedBlogPost.Title,
        UrlHandle = deletedBlogPost.UrlHandle,
      };

      return Ok(response);
    }
  }
}