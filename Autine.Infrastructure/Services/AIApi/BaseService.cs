using Autine.Application.ExternalContracts;
using Autine.Application.Interfaces.AIApi;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Autine.Infrastructure.Services.AIApi;
public class BaseService(IHttpClientFactory _httpClientFactory) : IBaseService
{
    public async Task<Result<T>> SendAsync<T>(Request request, CancellationToken ct = default)
    {

        HttpClient client = _httpClientFactory.CreateClient();
        using var message = BuildHttpRequestMessage(request);

        try
        {
            using var response = await client.SendAsync(message, ct);
            var responseContent = await response.Content.ReadAsStringAsync(ct);


            var returnedResult = response.StatusCode switch
            {
                HttpStatusCode.OK or HttpStatusCode.Created or HttpStatusCode.NoContent =>
                    Result.Success(JsonConvert.DeserializeObject<T>(responseContent)!),
                _ => Result.Failure<T>(ParseError(responseContent))
            };

            return returnedResult;
        }
        catch (Exception ex)
        {
            return Result.Failure<T>(Error.BadRequest("ex", ex.ToString()));
        }
    }
    public async Task<Result> SendAsync(Request request, CancellationToken ct = default)
    {
        HttpClient client = _httpClientFactory.CreateClient();
        using var message = BuildHttpRequestMessage(request);

        try
        {
            using var response = await client.SendAsync(message, ct);
            var responseContent = await response.Content.ReadAsStringAsync(ct);

            return response.StatusCode switch
            {
                HttpStatusCode.OK or HttpStatusCode.Created or HttpStatusCode.NoContent =>
                    Result.Success(),
                _ => Result.Failure(ParseError(responseContent))
            };
        }
        catch
        {
            // TODO: log error
            return Error.BadRequest("AI.Error", "An error occure while caling ai service.");
        }
    }

    private HttpRequestMessage BuildHttpRequestMessage(Request request)
    {
        var message = new HttpRequestMessage
        {
            RequestUri = new Uri(request.Url),
            Method = (sbyte)request.ApiMethod switch
            {
                1 => HttpMethod.Post,
                2 => HttpMethod.Put,
                3 => HttpMethod.Delete,
                _ => HttpMethod.Get
            }
        };

        message.Headers.Add("accept", "application/json");

        if (request.Data is not null)
        {
            message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
        }

        return message;
    }

    private static Error ParseError(string content)
    {
        try
        {

            var problemDetails = JsonConvert.DeserializeObject<ServiceResponse>(content);

            return Error.BadRequest("Ai.Error", problemDetails!.msg ?? "an error accure");

        }
        catch
        {
            return Error.BadRequest("Ai.Error", "an error eccure");
        }
    }

}
public record ServiceResponse(string msg);