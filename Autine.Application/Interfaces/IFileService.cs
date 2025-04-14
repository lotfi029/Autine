namespace Autine.Application.Interfaces;

public interface IFileService
{
    Task<Result<string>> UploadImageAsync(IFormFile image, CancellationToken token = default);
    Task<(FileStream? stream, string contentType, string fileName)> StreamAsync(Guid id, CancellationToken cancellationToken = default);
}