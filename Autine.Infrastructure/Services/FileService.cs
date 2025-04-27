using Autine.Application.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Autine.Infrastructure.Services;
public class FileService(
    IWebHostEnvironment _webHostEnvironment) : IFileService
{
    private const string _profileImagePath = ImageSettings.ImagePath;
    private readonly string _path = Path.Combine(_webHostEnvironment.WebRootPath, _profileImagePath);
    public async Task<Result<string>> UploadImageAsync(IFormFile image, CancellationToken token = default)
    {
        if (!Directory.Exists(_path))
        {
            Directory.CreateDirectory(_path);
        }
        
        var uniqueFileName = $"{Guid.CreateVersion7()}{Path.GetExtension(image.FileName)}";
        var filePath = Path.Combine(_path, uniqueFileName);

        using var fileStream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(fileStream, token);

        return Result.Success(uniqueFileName);
    }
    public async Task<(FileStream? stream, string? contentType, string? fileName)> StreamAsync(string image, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(image))
        {
            return (null, null, null);
        }
        
        var fileName = Path.GetFileName(image);
        var imagePath = Path.Combine(_path, fileName);
        
        if (!File.Exists(imagePath))
        {
            return (null, null, null);
        }
        var extension = Path.GetExtension(imagePath).ToLowerInvariant();
        var contentType = extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };
        var stream = new FileStream(
             imagePath,
             FileMode.Open,
             FileAccess.Read,
             FileShare.Read,
             4096,
             FileOptions.Asynchronous
        );


        return (stream, contentType, fileName); 
    }

    public Task<Result> DeleteImageAsync(string image, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(image))
            return Task.FromResult(Result.Failure(Error.BadRequest("", "")));

        
        image = Path.GetFileName(image);
        var imagePath = Path.Combine(_path, image);

        if (!File.Exists(imagePath))
            return Task.FromResult(Result.Failure(Error.BadRequest("", "")));

        try
        {
            File.Delete(imagePath);
            return Task.FromResult(Result.Success());
        }
        catch
        {
            return Task.FromResult(Result.Failure(Error.BadRequest("", "")));
        }
    }
}
