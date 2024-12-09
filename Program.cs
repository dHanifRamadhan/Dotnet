using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Project01.Database;
using Project01.Helpers;
using Project01.Services;
using Project01.Validations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

/*===========[Config Serilog]======================*/
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level:u3}] : [{Message:lj}] [{SourceContext}]{NewLine}")
    .WriteTo.File("Logs/AllDaily.log", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Debug()
    .CreateBootstrapLogger();
builder.Host.UseSerilog();
/*===========[Config Serilog]======================*/

/*===========[Config Database]======================*/
builder.Services.AddDbContext<MysqlContext>(options => {
    options.UseMySql(
        builder.Configuration.GetConnectionString("MysqlConnnection"), 
        new MySqlServerVersion(new Version(8, 0, 27)));
});
/*===========[Config Database]======================*/

/*===========[Config Swagger]======================*/
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config => {
    config.SwaggerDoc("1.0", new OpenApiInfo {
        Version = "1.0",
        Title = "Project01",
        Description = "Digunakan untuk membuat game rmt",
        Contact = new OpenApiContact {
            Name = "Contact",
            Url = new Uri("https://wa.me/0"),
        },
    });

    config.AddSecurityDefinition("bearer", new OpenApiSecurityScheme {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Scheme = "bearer"
    });

    config.OperationFilter<HeaderSecurity>();
});
/*===========[Config Swagger]======================*/

builder.Services.Configure<ConfigApp>(builder.Configuration.GetSection("ConfigApp"));
builder.Services.AddHttpContextAccessor();

ServiceRegister.Regis(builder.Services);
ValidationRegister.Regis(builder.Services);

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(config => {
        config.SwaggerEndpoint("/swagger/1.0/swagger.json", "1.0");
    });
}

app.UseHttpsRedirection();
app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseMiddleware<JWTMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
