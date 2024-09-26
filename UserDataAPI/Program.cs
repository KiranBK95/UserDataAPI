using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UserDataAPI.Model;

var builder = WebApplication.CreateBuilder(args);
//here i'm getting connection string for database which has been saved in the appsetting
var connString = builder.Configuration.GetConnectionString("Default");

//this block is to apply CORS, since the api and front end are running in different ports
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000") // default port that the react app is running in localhost now, please change it if you are running it on different port
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Setting up Database Connection
builder.Services.AddDbContext<AppDbContext>(options =>options.UseSqlServer(connString));

// Here, I'm adding JWT Authentication, no background has been provided in the task if the user is signed in or not
//so just assuming it's just a plain form, but just to make it bit secure i wanted to try and use JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "http://localhost",  // since i'm on localhost
        ValidAudience = "http://localhost", // since i'm on localhost
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecretKey1234567890123456")) // this is same as the key set in auth controller, better if i've saved in appsetting
    };
});

// Add services to the container.
builder.Services.AddControllers();

// Swagger configuration - for testing the API, with JWT authentication, had to do bit of extra work.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserDataAPI", Version = "v1" });

    // Add JWT Authentication scheme to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");

app.UseAuthentication();  
app.UseAuthorization();

app.MapControllers();


app.Run();
