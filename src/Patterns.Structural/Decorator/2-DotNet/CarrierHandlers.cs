namespace Patterns.Structural.Decorator.DotNet;

// Role: ConcreteDecorator — stamps every outgoing request with a correlation id.
// Guide: §5.4
public sealed class CorrelationIdHandler(Func<string> newId) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        request.Headers.Add("X-Correlation-Id", newId());
        return base.SendAsync(request, cancellationToken); // on to the inner handler
    }
}

// Role: ConcreteDecorator — writes "<method> <url>" for every request that passes through.
public sealed class RequestLogHandler(ICollection<string> log) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        log.Add($"{request.Method} {request.RequestUri}");
        return base.SendAsync(request, cancellationToken);
    }
}

// Role: ConcreteComponent — the "network" at the centre of the chain: answers 200 "ok" without a real call,
// and writes down the correlation header it received, to show what the decorators did on the way in.
public sealed class StubCarrierHandler(ICollection<string> log) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var correlationId = request.Headers.TryGetValues("X-Correlation-Id", out var values)
            ? string.Join(",", values)
            : "(none)";
        log.Add($"carrier saw X-Correlation-Id: {correlationId}");
        return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent("ok") });
    }
}

/// <summary>Builds the chain by hand. In an application, IHttpClientFactory builds it from registrations.</summary>
public static class CarrierHttpClient
{
    // The outermost handler runs first on the way in: correlation id → request log → carrier.
    public static HttpClient Create(Func<string> newId, ICollection<string> log) =>
        new(new CorrelationIdHandler(newId)
        {
            InnerHandler = new RequestLogHandler(log) { InnerHandler = new StubCarrierHandler(log) },
        });
}
