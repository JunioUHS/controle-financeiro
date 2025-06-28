using ControleFinanceiro.Api.Services;
using ControleFinanceiro.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using ControleFinanceiro.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using ControleFinanceiro.Api.Middlewares;
using ControleFinanceiro.Api.Repositories.Contracts;
using ControleFinanceiro.Api.Services.Contracts;
using ControleFinanceiro.Api.Models.Context;

var builder = WebApplication.CreateBuilder(args);

var AllowSpecificOrigins = "AllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
        policy =>
        {
            if (builder.Environment.IsDevelopment())
            {
                // Ambiente de desenvolvimento
                policy.WithOrigins(
                    "http://localhost:3000",      // React/Vite dev server comum
                    "http://localhost:5173",      // Vite dev server padrão
                    "http://127.0.0.1:3000",      
                    "http://127.0.0.1:5173"
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            }
            else
            {
                // Ambiente Production (Docker)
                policy.WithOrigins(
                    "http://localhost"        // Frontend no Docker (porta 80)
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            }
        });
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5097);
});

// Configuração do banco de dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mySqlOptions =>
    {
        mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    })
 );

// Configuração do Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<IdentityErrorDescriberPtBr>();

// Configuração de opções do Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Password.RequireUppercase = false;
});

// Configuração do JWT
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("A chave JWT n�o foi configurada. Verifique as configura��es em 'appsettings.json'.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
    options.DefaultScheme = "Bearer";
})
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero // Reduz o tempo de tolerância para 0
        };
    });

// Unit of Work
builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

// Repositories
builder.Services.AddScoped<IUserRefreshTokenRepository, UserRefreshTokenRepository>();
builder.Services.AddScoped<IAccountPayableRepository, AccountPayableRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAccountReceivableRepository, AccountReceivableRepository>();
builder.Services.AddScoped<ICreditCardRepository, CreditCardRepository>();
builder.Services.AddScoped<ICreditCardPurchaseRepository, CreditCardPurchaseRepository>();
builder.Services.AddScoped<IRecurringAccountReceivableRepository, RecurringAccountReceivableRepository>();
builder.Services.AddScoped<IPurchaseInstallmentRepository, PurchaseInstallmentRepository>();

// Services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAccountPayableService, AccountPayableService>();
builder.Services.AddScoped<IAccountReceivableService, AccountReceivableService>();
builder.Services.AddScoped<ICreditCardService, CreditCardService>();
builder.Services.AddScoped<ICreditCardPurchaseService, CreditCardPurchaseService>();
builder.Services.AddScoped<IRecurringAccountReceivableService, RecurringAccountReceivableService>();
builder.Services.AddScoped<IPurchaseInstallmentRepository, PurchaseInstallmentRepository>();
builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddAutoMapper(typeof(ControleFinanceiro.Api.Mapping.MappingProfile));

builder.Services.AddControllers(options =>
{
    // Adiciona autorização global para todos os controladores
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

// Desabilita o filtro de validação de modelo padrão do ASP.NET Core
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Insira o token JWT no formato 'Bearer {token}'"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { 
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Aplicar migrações automaticamente
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Só redirecionar para HTTPS se não estiver no Docker
if (!app.Environment.IsEnvironment("Docker"))
{
    app.UseHttpsRedirection();
}

app.UseCors(AllowSpecificOrigins);
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }