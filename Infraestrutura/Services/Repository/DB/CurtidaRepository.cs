﻿using WebAPI_Apollo.Model;
using WebAPI_Apollo.Model.Interacoes;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.DB
{
    public class CurtidaRepository : ICurtidaRepository
    {
        private readonly AppDbContext _context = new AppDbContext();

        public void Add(Curtida curtida, ref Post postCurtido)
        {
            postCurtido.NumCurtidas++;
            _context.Curtidas.Add(curtida);
            _context.SaveChanges();
        }

        public Curtida? JaCurtiu(Curtida curtida)
        {
            return _context.Curtidas
                .FirstOrDefault(jaCurtiuAntes =>
                jaCurtiuAntes.Remetente == curtida.Remetente &&
                jaCurtiuAntes.Destinatario == curtida.Destinatario);
        }

        public Curtida? Get(int id)
        {
            return _context.Curtidas.FirstOrDefault(e => e.Id == id);
        }

        public Curtida? GetLast()
        {
            return _context.Curtidas.OrderByDescending(e => e.Id).FirstOrDefault();
        }

        public void Update(Curtida curtida)
        {
            _context.Curtidas.Update(curtida);
            _context.SaveChanges();
        }

        public void Delete(Curtida curtida, ref Post postDescurtido)
        {
            postDescurtido.NumCurtidas--;
            _context.Curtidas.Remove(curtida);
            _context.SaveChanges();
        }
    }
}