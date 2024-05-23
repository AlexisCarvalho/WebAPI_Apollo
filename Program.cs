using WebAPI_Apollo.Infraestrutura.Services.Repository.DB;
using WebAPI_Apollo.Model.ViewModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*
 * Passado: UsuarioRAMController (Mudança da lógica "InformHome")
 *        Atualizar a Home nas rotas que envolvem ela           (FAZER REVISÃO)
 * Atual: Impedir solicitação de amizade cruzada entre usuarios, se um já
 *        tiver enviado o outro não pode.
 *        Garantir que o número de amigos esta aumentando onde devia
 *        Fazer o desfazer amizade
  _____________________________________________
 |******************* Facil *******************| //
 | TODO: Lista de amigos                       |  (EM PROGRESSO) 
 | TODO: Desfazer Amizade                      |  (EM PROGRESSO)

  _____________________________________________
 |***************** Possivel ******************| 
 | TODO: Solicitações de amizade               |  (EM PROGRESSO) 
 | TODO: Aceitar Solicitações de amizade       |  (EM PROGRESSO)
 | TODO: Modo de diferenciar lidas e não lidas |
 | TODO: Pesquisar por posts (pelos títulos)   |
 | TODO: Delete deve apagar todas as referen-  |
 |       cias do usuario incluindo amizades,   |
 |       posts, enfim... Tudo que seu id esta  |
 |       relacionado                           |
 |                                             |
  _____________________________________________
 |*********** Possivel mas difícil ************| 
 | TODO: Notificações                          |  (EM PROGRESSO) 
 | TODO: Feed gerado com pessoas ou perfis     |  (EM PROGRESSO) 
 |       que o usuario segue (Amigos)          |  
 | TODO: Posts estão sendo retornados e orden  |  (COMPLETO)
 |       ados pelo id, por ser Guid a ordem    |
 |       fica bagunçada, como posts não tem    |
 |       timeStamp seria sabio adicionar e     |
 |       ordenar por ela                       |

  _____________________________________________
 |************* Nearly Impossible *************| 
 | TODO: Compartilhar Post com Amigo           |
  _____________________________________________
 |******* Mexer Quando Tiver Pouco Tempo ******| 
 | TODO: Mudar todos os retornos Get para Dto, |
 |      pra evitar conversão na rota           |
 | TODO: Catalogar as respostas das rotas,     |
 |      possíveis retornos e código            |

*/

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<IEstatisticasRepository, EstatisticasRepository>();
builder.Services.AddTransient<IMensagemRepository, MensagemRepository>();
builder.Services.AddTransient<IPostsRepository, PostRepository>();
builder.Services.AddTransient<ICurtidaRepository, CurtidaRepository>();

// Esse primeiro aqui limita por porta "endereço",
// se descobrir como setar uma porta especifica no Electron
// muda pra essa aqui, o que esta usando é a debaixo

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "RestringirAcesso",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Muda isso aqui
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

// Essa aqui libera pra qualquer endereço ou porta,
// independente de quem tentar,
// se estiver rodando mesma maquina vai executar
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