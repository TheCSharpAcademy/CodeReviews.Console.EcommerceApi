using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Auth;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services.Handlers;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly IAuthTokenHolder _tokenHolder;

    public AuthHeaderHandler(IAuthTokenHolder tokenHolder)
    {
        _tokenHolder = tokenHolder;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_tokenHolder.Token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenHolder.Token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
