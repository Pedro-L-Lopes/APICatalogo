using ApiCatalogo.Repository;
using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions
        .ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "APICatalogo", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Header de autoriza��o JWT usando o esquema Bearer.\r\n\r\nInforme 'Bearer'[espa�o]..."

    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
             new string[]{}
        }

    });
});

builder.Services.AddTransient<IMeuServico, MeuServico>();

var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(mySqlConnection,
                    ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

// JWT
builder.Services.AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme).
        AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidAudience = builder.Configuration["TokenConfiguration:Audience"],
            ValidIssuer = builder.Configuration["TokenConfiguration:Issuer"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
        });

builder.Services.AddScoped<IUnityOfWork, UnityOfWork>();

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirApiRequest",
        builder =>
        builder.WithOrigins("https://wwwapirequest.io/")
        .WithMethods("GET"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Politica cors restritiva
//app.UseCors(opt  => opt
//    .WithOrigins("https://wwwapirequest.io/")
//        .WithMethods("GET"));

app.MapControllers();
app.Run();
