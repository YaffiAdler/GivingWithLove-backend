using Bl.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Web.Errors
{
    // Turns unhandled exceptions into ProblemDetails responses:
    // NotFoundException -> 404, InvalidQueryException -> 400, anything else -> 500.
    // Logging is not done here: ExceptionHandlerMiddleware already logs every exception it receives.
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IProblemDetailsService problemDetailsService;
        private readonly IHostEnvironment environment;

        public GlobalExceptionHandler(IProblemDetailsService problemDetailsService, IHostEnvironment environment)
        {
            this.problemDetailsService = problemDetailsService;
            this.environment = environment;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ProblemDetails problem = exception switch
            {
                NotFoundException notFound => CreateNotFoundProblem(notFound),
                InvalidQueryException invalidQuery => CreateInvalidQueryProblem(invalidQuery),
                _ => CreateServerErrorProblem(exception)
            };

            httpContext.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;

            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problem,
                Exception = exception
            });
        }

        private static ProblemDetails CreateNotFoundProblem(NotFoundException exception)
        {
            return new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found.",
                Detail = exception.Message
            };
        }

        private static ProblemDetails CreateInvalidQueryProblem(InvalidQueryException exception)
        {
            return new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid query.",
                Detail = exception.Message
            };
        }

        private ProblemDetails CreateServerErrorProblem(Exception exception)
        {
            ProblemDetails problem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred."
            };
            // Internal details help local debugging but must not leak to clients outside Development.
            if (environment.IsDevelopment())
            {
                problem.Detail = exception.Message;
                problem.Extensions["exception"] = exception.ToString();
            }
            return problem;
        }
    }
}
