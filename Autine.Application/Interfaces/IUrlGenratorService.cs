namespace Autine.Application.Interfaces;
public interface IUrlGenratorService
{
    string? GetImageUrl(string fileName,bool isBot);
}