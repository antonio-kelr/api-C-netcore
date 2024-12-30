using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using database.Data;
using Agenda.intefaces;
using Coberturas.intefaces;
using Agenda.Repositories;
using Coberturas.Repositories;
using Noticias.intefaces;
using Noticias.Repositories;
using Agenda.Services;
using Noticias.Services;
using CoberturasImagens.Repositories;
using CoberturasImagens.Interfaces;
using CoberturaImagens.Services;
using Cadastro.intefaces;
using Cadastro.Repositories;
using Cadastro.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração da conexão com o banco de dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(connectionString));

// Registrar os repositórios e serviços
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAgenda, AgendaRepositories>();
builder.Services.AddScoped<IACoberturas, CoberturaRepositories>();
builder.Services.AddScoped<IANoticias, NoticiasRepositories>();
builder.Services.AddScoped<ICoberturaImagens, CoberturaImagensRepository>();
builder.Services.AddScoped<IACadastro, CadastroRepositories>();

builder.Services.AddScoped<FirebaseImageService>();
builder.Services.AddScoped<CoberturaImagensServices>();
builder.Services.AddScoped<FirebaseImageServiceNoticias>();

// Configuração de autenticação JWT
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],  // Defina o Issuer na configuração
            ValidAudience = builder.Configuration["Jwt:Audience"], // Defina o Audience na configuração
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])) // Chave secreta
        };
    });

// Configuração dos controladores e ciclos de referência
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true; // Opcional: para JSON formatado
    });

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuração de ambiente
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware padrão
app.UseHttpsRedirection();

// Middleware de autenticação
app.UseAuthentication();
app.UseAuthorization();

// Mapeamento de controladores
app.MapControllers();

// Execução da aplicação
app.Run();
