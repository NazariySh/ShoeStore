using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using ShoeStore.Application.Interfaces;
using ShoeStore.Domain.Exceptions;

namespace ShoeStore.Infrastructure.Services;

public class ImageService : IImageService
{
    private const string FolderName = "images";

    private readonly Cloudinary _cloudinary;

    public ImageService(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary ?? throw new ArgumentNullException(nameof(cloudinary));
    }

    public async Task<string> UploadAsync(string publicId, IFormFile file, CancellationToken cancellationToken = default)
    {
        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
            PublicId = publicId,
            Folder = FolderName
        };

        var result = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

        if (result.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new InvalidOperationException($"Error uploading image: {result.Error.Message}");
        }

        return result.SecureUrl.ToString();
    }

    public async Task DeleteAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);

        var result = await _cloudinary.DestroyAsync(deleteParams);

        if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new NotFoundException($"Image with id {publicId} not found");
        }

        if (result.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new InvalidOperationException($"Error deleting image with id {publicId}: {result.Result}");
        }
    }
}