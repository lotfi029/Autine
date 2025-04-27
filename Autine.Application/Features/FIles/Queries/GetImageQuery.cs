namespace Autine.Application.Features.FIles.Queries;
public record GetImageQuery(string ImageUrl) : IQuery<(FileStream? stream, string? contentType, string? fileName)>;


public class GetImageQueryHandler(IFileService fileService) : IQueryHandler<GetImageQuery, (FileStream? stream, string? contentType, string? fileName)>
{
    public async Task<Result<(FileStream? stream, string? contentType, string? fileName)>> Handle(GetImageQuery request, CancellationToken cancellationToken)
        => await fileService.StreamAsync(request.ImageUrl, cancellationToken);
    
}
