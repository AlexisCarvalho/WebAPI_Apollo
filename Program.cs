using WebAPI_Apollo.Infraestrutura.Services.Repository.DB;
using WebAPI_Apollo.Model.ViewModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*
 * Passado: UsuarioRAMController (Mudança da lógica "InformHome")
 *        Atualizar a Home nas rotas que envolvem ela                        (FAZER REVISÃO)
 * Passado: Impedir solicitação de amizade cruzada entre usuarios, se um já  (FAZER REVISÃO)
 *        tiver enviado o outro não pode.
 *        Garantir que o número de amigos esta aumentando onde devia
 *        Fazer o desfazer amizade
 *        
 * Atual: Finalizar os detalhes da lista de Amigos

  _____________________________________________
 |******************* Facil *******************| //
 | TODO: Lista de amigos                       |  (EM PROGRESSO) 
 | TODO: Desfazer Amizade                      |  (CONCLUIDO)
 | TODO: Mandar mensagem apenas para amigos    |  (CONCLUIDO)
  _____________________________________________
 |***************** Possivel ******************| 
 | TODO: Solicitações de amizade               |  (CONCLUIDO) 
 | TODO: Aceitar Solicitações de amizade       |  (CONCLUIDO)
 | TODO: Modo de diferenciar lidas e não lidas |  (CONCLUIDO)
 | TODO: Pesquisar por posts (pelos títulos)   |  (CONCLUIDO)
 | TODO: Delete deve apagar todas as referen-  |  (PRECISA DE REVISÃO)
 |       cias do usuario incluindo amizades,   |
 |       posts, enfim... Tudo que seu id esta  |
 |       relacionado                           |
 |                                             |
  _____________________________________________
 |*********** Possivel mas difícil ************| 
 | TODO: Notificações                          |  (PRECISA DE REVISÃO) 
 | TODO: Feed gerado com pessoas ou perfis     |  (CONCLUIDO) 
 |       que o usuario segue (Amigos)          |  
 | TODO: Posts estão sendo retornados e orden  |  (CONCLUIDO)
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
 |       pra evitar conversão na rota          |
 | TODO: Catalogar as respostas das rotas,     |
 |       possíveis retornos e código           |

*/

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IAmizadeRepository, AmizadeRepository>();
builder.Services.AddTransient<ICurtidaRepository, CurtidaRepository>();
builder.Services.AddTransient<IEstatisticasRepository, EstatisticasRepository>();
builder.Services.AddTransient<IInformHomeRepository, InformHomeRepository>();
builder.Services.AddTransient<IMensagemRepository, MensagemRepository>();
builder.Services.AddTransient<INotificacaoRepository, NotificacaoRepository>();
builder.Services.AddTransient<IPostRepository, PostRepository>();
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();

// Esse primeiro aqui limita por porta "endereço",
// se descobrir como setar uma porta especifica no Electron
// muda pra essa aqui, o que esta usando é a debaixo
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