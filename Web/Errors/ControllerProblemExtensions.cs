using Microsoft.AspNetCore.Mvc;

namespace Web.Errors
{
    public static class ControllerProblemExtensions
    {
        // The route id is the source of truth for PUT; a different id in the body is a client error, not a silent overwrite.
        public static ObjectResult IdMismatchProblem(this ControllerBase controller, int routeId, int bodyId)
        {
            return controller.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Route id and body id differ.",
                detail: $"The route id {routeId} does not match the id {bodyId} in the request body.");
        }
    }
}
