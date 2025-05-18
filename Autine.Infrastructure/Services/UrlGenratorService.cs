using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Autine.Infrastructure.Services;

public class UrlGenratorService(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator) : IUrlGenratorService
{
    public string? GetImageUrl(string fileName, bool isBot)
    {
        if (string.IsNullOrEmpty(fileName))
            return null!;

        var httpContext = httpContextAccessor.HttpContext!;

        return linkGenerator.GetUriByAction(
            httpContext,
            action: "GetImage",
            controller: "Files",
            values: new { imageName = fileName, isBot },
            scheme: httpContext.Request.Scheme,
            host: httpContext.Request.Host
            );
    }
} 