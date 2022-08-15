using DustMedicalNinja.DAO;
using DustMedicalNinja.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustMedicalNinja.DAO
{
    public class EventoDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal async void Insert(Evento evento)
        {
            try
            {
                await _ConexaoMongoDB.Evento.InsertOneAsync(evento);
            }
            catch (Exception)
            {
                //TODO: add validacao e verificar o bug
            }

        }

        internal async Task<List<Evento>> ListEventos_FileDCM_LogUsuario(string fileDCMId, string tela)
        {
            var listaEventos = await _ConexaoMongoDB.Evento.Find(x =>
            x.itemId == fileDCMId &&
            x.tela == tela &&
            x.erro == false &&
            x.logUsuario == true).ToListAsync();

            return listaEventos;
        }

        internal async Task<List<Evento>> ListEventos_FileDCM(string fileDCMId, string tela)
        {
            var listaEventos = await _ConexaoMongoDB.Evento.Find(x =>
                x.itemId == fileDCMId &&
                x.tela == tela &&
                x.erro == false).ToListAsync();

            return listaEventos;
        }
    }
}
