using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Mock;
using Repository.Interfaces;
using Service.Interfaces;
using Service;
using Common.Dto;
using System.Text;
using Repository.Entities;
using Repository.Repositories;
using Service.Services;
using Microsoft.Extensions.FileProviders;

//var v = new Database();
//GreedyAlg f = new GreedyAlg(v);
//f.GetDietType();


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

// ✅ הרשמה לשירותים שלך
builder.Services.AddScoped<IService<CustomerDto>, CustomerService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
builder.Services.AddAutoMapper(typeof(MyMapper).Assembly); // ✅ שינוי: MyMapper הוא כנראה פרופיל בתוך Assembly, עדיף .Assembly
// אם MyMapper הוא לא קובץ, אלא רק "שם הפרופיל" אז תצטרכי לשים את ה-Assembly בו הוא נמצא
// לדוגמה: builder.Services.AddAutoMapper(typeof(Program).Assembly); או typeof(Startup).Assembly

builder.Services.AddScoped<IService<DietDto>, DietTypeService>();
builder.Services.AddScoped<IRepository<DietType>, DietTypeRepository>();
builder.Services.AddScoped<IService<WeeklyTrackingDto>, WeeklyTrackingService>();
builder.Services.AddScoped<IRepository<WeeklyTracking>, WeeklyTrackingRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFoodPreferenceRepository, FoodPreferenceRepository>();

// ✅ שינוי קריטי: הסר את השורה הזו והחלף אותה בשורה שמתחתיה
// builder.Services.AddScoped<IService<ProductDto>, ProductService>(); // ❌ הסר שורה זו!

// ✅ הוספה: רישום IProductService עבור ProductService
builder.Services.AddScoped<IProductService, ProductService>(); // ✅ זהו הרישום הנכון עבור הקונטרולר

builder.Services.AddScoped<IRepository<Product>, ProductRepository>();

// ודא ש-IContext ו-Database מגיעים מ-namespace נכון
// אם הם ב-namespace בשם 'DataRepository', ודא ש-using DataRepository; קיים למעלה
builder.Services.AddScoped<IContext, Database>();
builder.Services.AddDbContext<Database>(); // ✅ אם Database הוא DbContext, רשום אותו גם כ-DbContext
//builder.Services.AddScoped<IProductApiService, ProductApiService>();
builder.Services.AddHttpClient<IProductApiService, ProductApiService>();
builder.Services.AddScoped<GreedyAlg>();

// ✅ Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// ✅ CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/images"
});

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();