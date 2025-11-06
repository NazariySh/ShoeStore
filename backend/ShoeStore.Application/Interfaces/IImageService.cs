using Microsoft.AspNetCore.Http;

namespace ShoeStore.Application.Interfaces;

public interface IImageService
{
    Task<string> UploadAsync(string publicId, IFormFile file, CancellationToken cancellationToken = default);
    Task DeleteAsync(string publicId);
}