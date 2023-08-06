using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using API.Models.Domain.DTO;
using API.Models.Domain;
using API.Data;
using API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;

namespace API.Controlled {

  // https://localhost:xxxx/api/categories
  [Route("api/[controller]")]
  [ApiController]
  public class CategoriesController : ControllerBase {
    
    private readonly ICategoryRepository categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository){
      this.categoryRepository = categoryRepository;
    }

    //
    [HttpPost]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request) {
      // Map DTO to Domain Model
      var category = new Category {
        Name = request.Name,
        UrlHandle = request.UrlHandle
      };

      await categoryRepository.CreateAsync(category);

      // Domain Model to DTO
      var response = new CategoryDto {
        Id = category.Id,
        Name = category.Name,
        UrlHandle = category.UrlHandle
      };

      return Ok(response);
    }

    // GET: /api/categories
    [HttpGet]
    public async Task<IActionResult> GetAllCategories() {
      var categories = await categoryRepository.GetAllAsync();

      // map domain model to DTO
      var response = new List<CategoryDto>();
      foreach (var category in categories) {
        response.Add(new CategoryDto {
          Id= category.Id,
          Name= category.Name,
          UrlHandle= category.UrlHandle,
        });
      }

      return Ok(response);
    }

    // GET: /api/categories/:id
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid id) {
      var existingCategory = await categoryRepository.GetById(id);

      if (existingCategory == null) {
        return NotFound();
      }

      var response = new CategoryDto {
        Id = existingCategory.Id,
        Name = existingCategory.Name,
        UrlHandle = existingCategory.UrlHandle,
      };

      return Ok(response);
    }
  
    // PUT: /api/categories/:id
    [HttpPut]
    [Route("{id:Guid}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, UpdateCategoryRequestDto request) {
      // Convert DTO to Domain model
      var category = new Category {
        Id = id,
        Name = request.Name,
        UrlHandle = request.UrlHandle,
      };

      category = await categoryRepository.UpdateAsync(category);

      if (category == null) {
        return NotFound();
      }

      var response = new CategoryDto {
        Id = category.Id,
        Name = category.Name,
        UrlHandle = category.UrlHandle,
      };

      return Ok(response);
    }
  
    [HttpDelete]
    [Route("{id:Guid}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id) {
      var category = await categoryRepository.DeleteAsync(id);

      if(category == null) {
        return NotFound();
      }

      var response = new CategoryDto {
        Id = category.Id,
        Name = category.Name,
        UrlHandle = category.UrlHandle
      };

      return Ok(response);
    }
  }
}