using Bl;
using Dal;
using Web.Errors;

namespace Web
{
    public class Program
    {
        private const string FrontendCorsPolicy = "Frontend";
        private const string AllowedOriginsSection = "Cors:AllowedOrigins";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Composition root: the only place in Web that knows about Dal.
            builder.Services.AddDataAccess(builder.Configuration);
            builder.Services.AddBusinessLogic();

            builder.Services.AddControllers();
            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            AddFrontendCors(builder);

            var app = builder.Build();
            app.UseExceptionHandler();
            app.UseCors(FrontendCorsPolicy);
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }

        private static void AddFrontendCors(WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                // Any localhost port: ng serve falls back to another port when 4200 is taken.
                builder.Services.AddCors(options => options.AddPolicy(FrontendCorsPolicy, policy =>
                    policy.SetIsOriginAllowed(IsLocalhostOrigin)
                          .AllowAnyMethod()
                          .AllowAnyHeader()));
                return;
            }

            string[] allowedOrigins = builder.Configuration.GetSection(AllowedOriginsSection).Get<string[]>() ?? [];
            if (allowedOrigins.Length == 0)
            {
                throw new InvalidOperationException(
                    $"No CORS origins configured. Set '{AllowedOriginsSection}' for the '{builder.Environment.EnvironmentName}' environment.");
            }

            builder.Services.AddCors(options => options.AddPolicy(FrontendCorsPolicy, policy =>
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyMethod()
                      .AllowAnyHeader()));
        }

        private static bool IsLocalhostOrigin(string origin)
        {
            return Uri.TryCreate(origin, UriKind.Absolute, out Uri? uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
                && uri.Host == "localhost";
        }
    }
}
