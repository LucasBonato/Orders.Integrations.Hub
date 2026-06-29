using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using NSubstitute;

using Orders.Integrations.Hub.Core.Infrastructure.Exceptions;

using HubExceptionHandlerMiddleware = Orders.Integrations.Hub.Core.Infrastructure.Middlewares.ExceptionHandlerMiddleware;

namespace Orders.Integrations.Hub.UnitTests.Middleware;

public class ExceptionHandlerMiddlewareTests
{
    private static HubExceptionHandlerMiddleware CreateSut(out IProblemDetailsService problemDetailsService)
    {
        problemDetailsService = Substitute.For<IProblemDetailsService>();
        problemDetailsService.TryWriteAsync(Arg.Any<ProblemDetailsContext>()).Returns(true);
        return new HubExceptionHandlerMiddleware(problemDetailsService);
    }

    [Fact]
    public async Task TryHandleAsync_ShouldUseProblemDetailsService()
    {
        HubExceptionHandlerMiddleware sut = CreateSut(out IProblemDetailsService problemDetailsService);
        DefaultHttpContext httpContext = new();

        bool handled = await sut.TryHandleAsync(httpContext, new Exception("test"), TestContext.Current.CancellationToken);

        Assert.True(handled);
        await problemDetailsService.Received(1).TryWriteAsync(Arg.Any<ProblemDetailsContext>());
    }

    [Fact]
    public async Task TryHandleAsync_ShouldCreateProblemDetails_ForGenericException()
    {
        HubExceptionHandlerMiddleware sut = CreateSut(out IProblemDetailsService problemDetailsService);
        DefaultHttpContext httpContext = new();

        await sut.TryHandleAsync(httpContext, new Exception("generic error"), TestContext.Current.CancellationToken);

        await problemDetailsService.Received(1).TryWriteAsync(Arg.Is<ProblemDetailsContext>(predicate: ctx =>
            ctx.ProblemDetails.Status == 500
        ));
    }

    [Fact]
    public async Task TryHandleAsync_ShouldCreateProblemDetails_ForInvalidOperationException()
    {
        HubExceptionHandlerMiddleware sut = CreateSut(out IProblemDetailsService problemDetailsService);
        DefaultHttpContext httpContext = new();

        await sut.TryHandleAsync(httpContext, new InvalidOperationException("invalid op"), TestContext.Current.CancellationToken);

        await problemDetailsService.Received(1).TryWriteAsync(Arg.Is<ProblemDetailsContext>(ctx =>
            ctx.ProblemDetails.Status == 501
        ));
    }

    [Fact]
    public async Task TryHandleAsync_ShouldUseCustomProblemDetails_WhenIProblemDetailsException()
    {
        HubExceptionHandlerMiddleware sut = CreateSut(out IProblemDetailsService problemDetailsService);
        DefaultHttpContext httpContext = new();
        TestProblemDetailsException exception = new("custom error", 422);

        await sut.TryHandleAsync(httpContext, exception, TestContext.Current.CancellationToken);

        await problemDetailsService.Received(1).TryWriteAsync(Arg.Is<ProblemDetailsContext>(ctx =>
            ctx.ProblemDetails.Status == 422 &&
            ctx.ProblemDetails.Detail == "custom error"
        ));
    }

    private sealed class TestProblemDetailsException(
        string message, 
        int statusCode
    ) : Exception(message), IProblemDetailsException {
        private int StatusCode { get; } = statusCode;

        public ProblemDetails ToProblemDetails() => new() {
            Status = StatusCode,
            Detail = Message,
            Title = "Custom Error"
        };
    }
}