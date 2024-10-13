using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Project01.Helpers {
    public class HeaderSecurity : IOperationFilter {
        public void Apply(OpenApiOperation apiOperation, OperationFilterContext filterContext) {
            if (apiOperation == null)
                apiOperation.Security = new List<OpenApiSecurityRequirement>();

            var scheme = new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearer",
                },
            };

            apiOperation.Security.Add(new OpenApiSecurityRequirement {
                [scheme] = new List<string>(),
            });
        }
    }
}