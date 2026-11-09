using Bl.Dtos.Common;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Web.Swagger
{
    // Direction is a string so that an unknown value gets the same 400 ProblemDetails as an unknown sort.
    // This filter still documents it as an enum, so the generated frontend type is 'asc' | 'desc'.
    public sealed class SortDirectionParameterFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            if (context.PropertyInfo?.DeclaringType != typeof(PagingQueryDto)
                || context.PropertyInfo.Name != nameof(PagingQueryDto.Direction))
                return;

            parameter.Schema.Enum = PagingQueryDto.DirectionValues
                .Select(value => (IOpenApiAny)new OpenApiString(value))
                .ToList();
        }
    }
}
