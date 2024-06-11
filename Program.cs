using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebAPI_Apollo.Domain.Model.Interfaces;
using WebAPI_Apollo.Infraestrutura.Repository.RAM;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// | Amizade Repository |
builder.Services.AddTransient<IAmizadeRepository, AmizadeRepositoryRAM>();

// | Curtida Repository |
builder.Services.AddTransient<ICurtidaRepository, CurtidaRepositoryRAM>();

// | Estatistica Repository |
builder.Services.AddTransient<IEstatisticasRepository, EstatisticasRepositoryRAM>();

// | InformHome Repository |
builder.Services.AddTransient<IInformHomeRepository, InformHomeRepositoryRAM>();

// | Mensagem Repository |
builder.Services.AddTransient<IMensagemRepository, MensagemRepositoryRAM>();

// | Notificacao Repository |
builder.Services.AddTransient<INotificacaoRepository, NotificacaoRepositoryRAM>();

// | Post Repository |
builder.Services.AddTransient<IPostRepository, PostRepositoryRAM>();

// | Usuario Repository |
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepositoryRAM>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Liberado",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header,

        },
        new List<string>()
        }
    });


});

var key = Encoding.ASCII.GetBytes(WebAPI_Apollo.Key.Secret);

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
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Liberado");

app.UseAuthorization();

app.MapControllers();

app.Run();