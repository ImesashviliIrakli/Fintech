using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OrderService.Models;

public class CustomHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters =
        [
            new OpenApiParameter
            {
                Name = "ApiKey",
                In = ParameterLocation.Header,
                Description = "ApiKey for authentication",
                Required = true
            },
            new OpenApiParameter
            {
                Name = "ApiSecret",
                In = ParameterLocation.Header,
                Description = "ApiSecret for authentication",
                Required = true
            },
        ];
    }
}