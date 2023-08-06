using API.Models.Domain;

namespace API.Repositories.Interface {
  public interface IImageRepository {
    Task<BlobImage> Upload(IFormFile file, BlobImage blogImage);

    Task<IEnumerable<BlobImage>> GetAll();
  }
}