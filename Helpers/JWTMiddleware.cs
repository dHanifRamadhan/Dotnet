using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Project01.Services;

namespace Project01.Helpers {
    public class JWTMiddleware {
        private readonly RequestDelegate next;
        private readonly ConfigApp configApp;
        private readonly ILogger<JWTMiddleware> logger;
        public JWTMiddleware(
            RequestDelegate next,
            IOptions<ConfigApp> options,
            ILogger<JWTMiddleware> logger
        ) {
            this.next = next;
            configApp = options.Value;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context, IUserService userService) {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
                await attachUserToContext(context, userService, token);

            await next(context);
        }

        private  async Task  attachUserToContext(HttpContext context, IUserService userService, string token) {
            try {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(configApp.KeyApp);

                tokenHandler.ValidateToken(token, new TokenValidationParameters {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userCode = jwtToken.Claims.First(x => x.Type == "UserCode").Value;

                // Retrieve user from DB
                var user = await userService.GetByCode(userCode);
                context.Items["User"] = user;
            } catch(Exception e) {
                logger.LogError("JWT {0}",e);
            }
        }
    }
}