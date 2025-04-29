using Autine.Application.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Autine.Infrastructure.Services;
public class FileService(
    IWebHostEnvironment _webHostEnvironment) : IFileService
{
    private const string _profileImagePath = ImageSettings.ImagePath;
    private const string _botImagePath = ImageSettings.BotImagePath;
    private readonly string _profilePath = Path.Combine(_webHostEnvironment.WebRootPath, _profileImagePath);
    private readonly string _botBath = Path.Combine(_webHostEnvironment.WebRootPath, _botImagePath);
    public async Task<Result<string>> UploadImageAsync(IFormFile image, bool isBot = false, CancellationToken token = default)
    {
        var path = isBot ? _botBath : _profilePath;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        
        var uniqueFileName = $"{Guid.CreateVersion7()}{Path.GetExtension(image.FileName)}";
        var filePath = Path.Combine(path, uniqueFileName);

        using var fileStream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(fileStream, token);

        return Result.Success(uniqueFileName);
    }
    public async Task<Result<string>> UpdateImageAsync(string image, IFormFile newImage, bool isBot = false, CancellationToken ct = default)
    {
        if (!string.IsNullOrEmpty(image))
        {
            var deleteResult = await DeleteImageAsync(image, isBot);
            if (deleteResult.IsFailure)
                return deleteResult.Error;
        }

        var addResult = await UploadImageAsync(newImage, isBot, ct);
        
        if (addResult.IsFailure)
            return addResult.Error;

        return addResult.Value;
    }
    public async Task<(FileStream? stream, string? contentType, string? fileName)> StreamAsync(string image, bool isBot = false, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(image))
        {
            return (null, null, null);
        }
        
        var path = isBot ? _botBath : _profilePath;
        var fileName = Path.GetFileName(image);
        var imagePath = Path.Combine(path, fileName);
        
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

    public Task<Result> DeleteImageAsync(string image, bool isBot = false)
    {
        if (string.IsNullOrEmpty(image))
            return Task.FromResult(Result.Failure(Error.BadRequest("EmptyFileName", "File name cannot be empty")));

        var path = isBot ? _botBath : _profilePath;
        
        try
        {
            var fileName = Path.GetFileName(image);
            var imagePath = Path.Combine(path, fileName);

            if (!File.Exists(imagePath))
                return Task.FromResult(Result.Failure(Error.BadRequest("FileNotFound", "Image file does not exist")));


            File.Delete(imagePath);
            return Task.FromResult(Result.Success());
        }
        catch (Exception ex)
        {

            return Task.FromResult(Result.Failure(Error.BadRequest("DeleteFailed", $"Failed to delete image: {ex.Message}")));
        }
    }
}
