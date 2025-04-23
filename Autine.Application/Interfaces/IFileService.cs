namespace Autine.Application.Interfaces;

public interface IFileService
{
    Task<Result<string>> UploadImageAsync(IFormFile image, CancellationToken token = default);
    Task<(FileStream? stream, string? contentType, string? fileName)> StreamAsync(string image, CancellationToken cancellationToken = default);
    Task<Result> DeleteImageAsync(string image, CancellationToken cancellationToken = default);
}