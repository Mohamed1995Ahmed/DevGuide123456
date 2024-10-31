using DevGuide.Models;
using DevGuide.Models.Models;
using Managers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models.Models;
using Services;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<IISServerOptions>(options => {
    options.MaxRequestBodySize = int.MaxValue; // Set to a large number or use int.MaxValue for no limit
});

builder.Services.Configure<KestrelServerOptions>(options => {
    options.Limits.MaxRequestBodySize = int.MaxValue; // Set the max request body size for Kestrel
});

// Register HttpClient


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Keep property names as they are
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; // Ignore null values
    });

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    //options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Always;
});

builder.Services.Configure<PayPalSettings>(builder.Configuration.GetSection("PayPalSettings"));

// Add services to the container.


//each one has its two lines for not conflict

//each one use its connectionstring in
//appsettings.json and make migration and
//comment the other lines of connectionstring in program.cs




//SQL Server version:	MS SQL 2022 Express
//SQL Server address:	Account.mssql.somee.com
//Login name: Adhamdy_SQLLogin_1
//Password: 7xmxtmkhsg



//Default Connection String 
//builder.Services.AddDbContext<ProjectContext>(options =>
//    options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



















//Adham
builder.Services.AddDbContext<ProjectContext>(options =>
    options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));





////Dina
//builder.Services.AddDbContext<ProjectContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("Dina")));

//Mirna
//builder.Services.AddDbContext<ProjectContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("Mirna")));

//Mohamed
//builder.Services.AddDbContext<ProjectContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("Mohamed")));

//Hassan
//builder.Services.AddDbContext<ProjectContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("Hassan")));

//Ahmed
//builder.Services.AddDbContext<ProjectContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("Ahmed")));

builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ProjectContext>();
builder.Services.AddScoped<AccountManager>();
builder.Services.AddScoped<MyRoleManager>();
builder.Services.AddScoped<SkillManager>();
builder.Services.AddScoped<EducationManager>();
builder.Services.AddScoped<ReviewManager>();
builder.Services.AddScoped<QueryManager>();

builder.Services.AddScoped<InstructorManager>();
builder.Services.AddScoped<QuerAnswerManager>();

builder.Services.AddScoped<QuizeManager>();

builder.Services.AddScoped<User_QuizeManager>();
builder.Services.AddScoped<ExperienceManager>();

builder.Services.AddHttpClient<ChatGptQuizService>();

builder.Services.AddScoped<ScheduleManager>();
builder.Services.AddScoped<SessionManager>();
builder.Services.AddScoped<SupportManager>();
builder.Services.AddScoped<PaymentManager>();






builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = false,
        ValidateActor = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:Key"]))
    };

});


builder.Services.AddCors(i => i.AddDefaultPolicy(
    i => i.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));







builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}
app.UseStaticFiles();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();