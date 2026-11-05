using System.Reflection;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Web.Swagger
{
    // Marks every non-nullable property of a schema as required.
    // Swashbuckle 6.6.2 has no NonNullableReferenceTypesAsRequired(), and the frontend generates its TypeScript
    // types from docs/openapi.json, so without this every property would be generated as optional.
    // Applies to body and response schemas only; query parameters are not schemas and stay optional.
    public sealed class NonNullableAsRequiredSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties.Count == 0)
                return;

            // Not thread-safe, so one instance per call.
            NullabilityInfoContext nullabilityContext = new NullabilityInfoContext();

            foreach (PropertyInfo property in context.Type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // Same naming policy as the JSON serializer (camelCase), so the key matches the schema property.
                string schemaKey = JsonNamingPolicy.CamelCase.ConvertName(property.Name);
                if (!schema.Properties.ContainsKey(schemaKey))
                    continue;

                // NotNull covers both non-nullable value types and reference types not annotated with '?'.
                if (nullabilityContext.Create(property).ReadState == NullabilityState.NotNull)
                    schema.Required.Add(schemaKey);
            }
        }
    }
}
