using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interface;
using WebAPI_Apollo.Infraestrutura;
using WebAPI_Apollo.Infraestrutura.Services;
using WebAPI_Apollo.Infraestrutura.Services.Repository.DB;
using WebAPI_Apollo.Infraestrutura.Services.Repository.RAM;
using WebAPI_Apollo.Infraestrutura.Services.Proxy;

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

// Esse primeiro aqui limita por porta "endere�o",
// se descobrir como setar uma porta especifica no Electron
// muda pra essa aqui, o que esta usando � a debaixo
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "RestringirAcesso",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Muda isso aqui pro do Electron Jo�o
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

// Essa aqui libera pra qualquer endere�o ou porta,
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