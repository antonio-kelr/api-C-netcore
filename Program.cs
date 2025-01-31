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
using Classificados.Services;
using Classificados.intefaces;
using Classificados.Repositories;
using Classificado.Interfaces;
using Recado.intefaces;
using Recado.Repositories;
using Banner.intefaces;
using Banner.Repositories;
using banner.Services;



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
builder.Services.AddScoped<IRecado, RecadoRepositories>();
builder.Services.AddScoped<IBanner, BannerRepositories>();
builder.Services.AddScoped<ICoberturaImagens, CoberturaImagensRepository>();
builder.Services.AddScoped<IClassificadoImagen, ClassificaImagemdoRepositories>();
builder.Services.AddScoped<IACadastro, CadastroRepositories>();
builder.Services.AddScoped<IClassificados, ClassificadoRepositories>();

builder.Services.AddScoped<FirebaseImageBanner>();
builder.Services.AddScoped<FirebaseImageService>();
builder.Services.AddScoped<CoberturaImagensServices>();
builder.Services.AddScoped<ClassificadosServices>();
builder.Services.AddScoped<FirebaseImageServiceNoticias>();

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowLocalhost");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
