using WebAPI_Apollo.Infraestrutura.Services.Repository.DB;
using WebAPI_Apollo.Model.ViewModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*
 * Passado: UsuarioRAMController (Mudan�a da l�gica "InformHome")
 *        Atualizar a Home nas rotas que envolvem ela                        (FAZER REVIS�O)
 * Passado: Impedir solicita��o de amizade cruzada entre usuarios, se um j�  (FAZER REVIS�O)
 *        tiver enviado o outro n�o pode.
 *        Garantir que o n�mero de amigos esta aumentando onde devia
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
 | TODO: Solicita��es de amizade               |  (CONCLUIDO) 
 | TODO: Aceitar Solicita��es de amizade       |  (CONCLUIDO)
 | TODO: Modo de diferenciar lidas e n�o lidas |  (CONCLUIDO)
 | TODO: Pesquisar por posts (pelos t�tulos)   |  (CONCLUIDO)
 | TODO: Delete deve apagar todas as referen-  |  (PRECISA DE REVIS�O)
 |       cias do usuario incluindo amizades,   |
 |       posts, enfim... Tudo que seu id esta  |
 |       relacionado                           |
 |                                             |
  _____________________________________________
 |*********** Possivel mas dif�cil ************| 
 | TODO: Notifica��es                          |  (PRECISA DE REVIS�O) 
 | TODO: Feed gerado com pessoas ou perfis     |  (CONCLUIDO) 
 |       que o usuario segue (Amigos)          |  
 | TODO: Posts est�o sendo retornados e orden  |  (CONCLUIDO)
 |       ados pelo id, por ser Guid a ordem    |
 |       fica bagun�ada, como posts n�o tem    |
 |       timeStamp seria sabio adicionar e     |
 |       ordenar por ela                       |

  _____________________________________________
 |************* Nearly Impossible *************| 
 | TODO: Compartilhar Post com Amigo           |
  _____________________________________________
 |******* Mexer Quando Tiver Pouco Tempo ******| 
 | TODO: Mudar todos os retornos Get para Dto, |
 |       pra evitar convers�o na rota          |
 | TODO: Catalogar as respostas das rotas,     |
 |       poss�veis retornos e c�digo           |

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