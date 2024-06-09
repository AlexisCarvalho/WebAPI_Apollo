using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Repository.RAM
{
    public class AmizadeRepositoryRAM : IAmizadeRepository
    {
        private readonly InformHomeRepositoryRAM _infHomeRepository = new();

        public async Task Add(Amizade amizade)
        {
            await Task.Run(() =>
            {
                // Código pra substituir o autoIncrement do Banco
                var ultimaAmizade = GetLast().Result;

                if (ultimaAmizade != null)
                {
                    amizade.Id = ultimaAmizade.Id + 1;
                }
                else
                {
                    amizade.Id = 1;
                }
                // Visa manter o uso de id int ao invés de trocar pra Guid

                VolatileContext.Amizades.Add(amizade);
            });

            return;
        }

        public Task<Amizade?> VerificarAmizade(Amizade amizade)
        {
            var resultado = VolatileContext.Amizades
                                .FirstOrDefault(amizadesNoBanco =>
                                amizadesNoBanco.Remetente == amizade.Remetente
                                && amizadesNoBanco.Destinatario == amizade.Destinatario
                                || amizadesNoBanco.Destinatario == amizade.Remetente
                                && amizadesNoBanco.Remetente == amizade.Destinatario);

            return Task.FromResult(resultado);
        }

        public Task<Amizade?> Get(int id)
        {
            var resultado = VolatileContext.Amizades
                .FirstOrDefault(e => e.Id == id);

            return Task.FromResult(resultado);
        }

        public Task<List<Amizade>> GetAllUsr(Guid idUsuario)
        {
            var resultado = 
                VolatileContext.Amizades
                        .OrderByDescending(amz => amz.Id)
                        .Select(amz => new Amizade
                        (
                            amz.Remetente,
                            amz.Destinatario
                        ))
                        .Where(amz => amz.Destinatario == idUsuario
                                   || amz.Remetente == idUsuario)
                        .ToList();

            return Task.FromResult(resultado);
        }

        public Task<Amizade?> GetLast()
        {
            var resultado = VolatileContext.Amizades
                .OrderByDescending(e => e.Id)
                .FirstOrDefault();

            return Task.FromResult(resultado);
        }

        public async Task Update(Amizade amizade)
        {
            await Task.Run(() =>
            {
                var index = VolatileContext.Amizades
                    .FindIndex(e => e.Id == amizade.Id);
                if (index != -1)
                {
                    VolatileContext.Amizades[index] = amizade;
                }
            });
        }

        public async Task Delete(Amizade amizade)
        {
            await Task.Run(() =>
            {
                VolatileContext.Amizades.Remove(amizade);
            });
        }

        public async Task DeletarReferencias(Guid idUsuario)
        {
            await Task.Run(() => 
            { 
                var amizadesDoUsr = VolatileContext.Amizades
                    .Select(amz => new Amizade
                    (
                        amz.Id,
                        amz.Remetente,
                        amz.Destinatario
                     ))
                    .Where(amz => amz.Destinatario == idUsuario
                                  || amz.Remetente == idUsuario)
                    .ToList();

                foreach (var amizade in amizadesDoUsr)
                {
                    if (amizade.Remetente == idUsuario)
                    {
                        var homeAmigo = _infHomeRepository.GetViaUsr(amizade.Destinatario).Result;

                        if (homeAmigo != null)
                        {
                            homeAmigo.NumAmigos--;
                            _infHomeRepository.Update(homeAmigo);
                        }
                    }
                    else
                    {
                        var homeAmigo = _infHomeRepository.GetViaUsr(amizade.Remetente).Result;

                        if (homeAmigo != null)
                        {
                            homeAmigo.NumAmigos--;
                            _infHomeRepository.Update(homeAmigo);
                        }
                    }
                    VolatileContext.Amizades.Remove(amizade);
                }
            });
        }
    }
}
