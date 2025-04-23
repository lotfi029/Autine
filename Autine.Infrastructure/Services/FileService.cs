using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Autine.Infrastructure.Services;
public class FileService(
    IWebHostEnvironment _webHostEnvironment) : IFileService
{
    private readonly string _path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "ProfilePictures");
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

        var response = $"/Uploads/ProfilePictures/{uniqueFileName}";

        return Result.Success(response);
    }
    public Task<(FileStream? stream, string? contentType, string? fileName)> StreamAsync(string image, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteImageAsync(string image, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
