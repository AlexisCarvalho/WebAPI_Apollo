using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebAPI_Apollo.Application.Services;
using WebAPI_Apollo.Domain.Model.Interfaces;
using WebAPI_Apollo.Infraestrutura;
using WebAPI_Apollo.Infraestrutura.Proxy;
using WebAPI_Apollo.Infraestrutura.Repository.DB;
using WebAPI_Apollo.Infraestrutura.Repository.RAM;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ConfigService>();

// | Amizade Repository |
builder.Services.AddTransient<AmizadeRepository>();
builder.Services.AddTransient<AmizadeRepositoryRAM>();
builder.Services.AddTransient<IAmizadeRepository>(provider =>
{
    var configService = provider.GetRequiredService<ConfigService>();
    var dbRepository = provider.GetRequiredService<AmizadeRepository>();
    var ramRepository = provider.GetRequiredService<AmizadeRepositoryRAM>();
    return new AmizadeRepositoryProxy(configService, dbRepository, ramRepository);
});

// | Curtida Repository |
builder.Services.AddTransient<CurtidaRepository>();
builder.Services.AddTransient<CurtidaRepositoryRAM>();
builder.Services.AddTransient<ICurtidaRepository>(provider =>
{
    var configService = provider.GetRequiredService<ConfigService>();
    var dbRepository = provider.GetRequiredService<CurtidaRepository>();
    var ramRepository = provider.GetRequiredService<CurtidaRepositoryRAM>();
    return new CurtidaRepositoryProxy(configService, dbRepository, ramRepository);
});

// | Estatistica Repository |
builder.Services.AddTransient<EstatisticasRepository>();
builder.Services.AddTransient<EstatisticasRepositoryRAM>();
builder.Services.AddTransient<IEstatisticasRepository>(provider =>
{
    var configService = provider.GetRequiredService<ConfigService>();
    var dbRepository = provider.GetRequiredService<EstatisticasRepository>();
    var ramRepository = provider.GetRequiredService<EstatisticasRepositoryRAM>();
    return new EstatisticasRepositoryProxy(configService, dbRepository, ramRepository);
});

// | InformHome Repository |
builder.Services.AddTransient<InformHomeRepository>();
builder.Services.AddTransient<InformHomeRepositoryRAM>();
builder.Services.AddTransient<IInformHomeRepository>(provider =>
{
    var configService = provider.GetRequiredService<ConfigService>();
    var dbRepository = provider.GetRequiredService<InformHomeRepository>();
    var ramRepository = provider.GetRequiredService<InformHomeRepositoryRAM>();
    return new InformHomeRepositoryProxy(configService, dbRepository, ramRepository);
});

// | Mensagem Repository |
builder.Services.AddTransient<MensagemRepository>();
builder.Services.AddTransient<MensagemRepositoryRAM>();
builder.Services.AddTransient<IMensagemRepository>(provider =>
{
    var configService = provider.GetRequiredService<ConfigService>();
    var dbRepository = provider.GetRequiredService<MensagemRepository>();
    var ramRepository = provider.GetRequiredService<MensagemRepositoryRAM>();
    return new MensagemRepositoryProxy(configService, dbRepository, ramRepository);
});

// | Notificacao Repository |
builder.Services.AddTransient<NotificacaoRepository>();
builder.Services.AddTransient<NotificacaoRepositoryRAM>();
builder.Services.AddTransient<INotificacaoRepository>(provider =>
{
    var configService = provider.GetRequiredService<ConfigService>();
    var dbRepository = provider.GetRequiredService<NotificacaoRepository>();
    var ramRepository = provider.GetRequiredService<NotificacaoRepositoryRAM>();
    return new NotificacaoRepositoryProxy(configService, dbRepository, ramRepository);
});

// | Post Repository |
builder.Services.AddTransient<PostRepository>();
builder.Services.AddTransient<PostRepositoryRAM>();
builder.Services.AddTransient<IPostRepository>(provider =>
{
    var configService = provider.GetRequiredService<ConfigService>();
    var dbRepository = provider.GetRequiredService<PostRepository>();
    var ramRepository = provider.GetRequiredService<PostRepositoryRAM>();
    return new PostRepositoryProxy(configService, dbRepository, ramRepository);
});

// | Usuario Repository |
builder.Services.AddTransient<UsuarioRepository>();
builder.Services.AddTransient<UsuarioRepositoryRAM>();
builder.Services.AddTransient<IUsuarioRepository>(provider =>
{
    var configService = provider.GetRequiredService<ConfigService>();
    var dbRepository = provider.GetRequiredService<UsuarioRepository>();
    var ramRepository = provider.GetRequiredService<UsuarioRepositoryRAM>();
    return new UsuarioRepositoryProxy(configService, dbRepository, ramRepository);
});

// Adicionar testes automaticos do Banco
builder.Services.AddHostedService<TestarBDService>();

// Esse primeiro aqui limita por porta "endereço",
// se descobrir como setar uma porta especifica no Electron
// muda pra essa aqui, o que esta sendo usado é a de baixo
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "RestringirAcesso",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Muda isso aqui pro do Electron João
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

// Essa aqui libera pra qualquer endereço ou porta,
// independente de quem tentar,
// se estiver rodando na mesma maquina vai executar
builder.Services.AddCors(options =>
{
    options.AddPolicy("Liberado",
        builder =>
        {
            builder.AllowAnyOrigin() // Permite acesso de qualquer origem
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