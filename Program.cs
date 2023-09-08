using Api.MyFlix;
using Api.MyFlix.Data;
using Api.MyFlix.Services;
using Api.MyFlix.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ISeriesService, SeriesService>();
builder.Services.AddTransient<IEpisodesService, EpisodesService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Api.MyFlix",
        Version = "v1",
        Description = "API para um app de streaming",
        Contact = new OpenApiContact()
        {
            Name = "Emerson",
            Email = "emersondejesussantos@hotmail.com",
            Url = new Uri("https://www.linkedin.com/in/emerson-de-jesus-santos-303640195/")
        },
    });
    // using System.Reflection;
    // var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
});

// Banco de dados
string db = builder.Configuration.GetConnectionString("DB");
if (db == "mssql")
{
    string msSqlConnection = builder.Configuration.GetConnectionString("MsSqlConnection");
    builder.Services.AddDbContextPool<AppDbContext>(options =>
    {
        options.UseSqlServer(msSqlConnection);
        //options.UseLazyLoadingProxies();
    });
}
else if (db == "mysql")
{
    string mySqlConnection = builder.Configuration.GetConnectionString("MySqlConnection");
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection));

        //options.UseLazyLoadingProxies();
    });
}
else if(db == "postgresql")
{
    string postgreSqlConnection = builder.Configuration.GetConnectionString("PostgreSqlConnection");
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseNpgsql(postgreSqlConnection);
        //options.UseLazyLoadingProxies();
    });
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
}

// AUTHENTICATION
var key = Encoding.ASCII.GetBytes(Settings.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Settings.Secret)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// Configure the HTTP request pipeline.
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

// Aplicar migração ao rodar o projeto
/*
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dataContext.Database.Migrate();
}
*/
app.Run();
