using DAl.IRepository;
using DAl.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ThirdParty.Middlewares
{
    public class IPBlockMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public IPBlockMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork<BannedIP>>();

                string ipAddress = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                    ?? httpContext.Connection.RemoteIpAddress?.ToString();

                var checkingip = await unitOfWork.BannedIP.GetItemWithFunc(e => e.IPAddress == ipAddress);

                if (!string.IsNullOrEmpty(ipAddress) && checkingip != null)
                {
                    if (checkingip.FailureCount == 2)
                    {
                        httpContext.Response.StatusCode = 403;
                        await httpContext.Response.WriteAsync("Access Denied from IP");
                        return;
                    }
                    else
                    {
                        checkingip.FailureCount++;
                        checkingip.LastFailed = DateTime.Now;
                        unitOfWork.BannedIP.UpdateItem(checkingip);
                        await unitOfWork.SaveChangesAsync();
                    }
                }
            }

            await _next(httpContext);
        }
    }
}
