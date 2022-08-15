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
    public class UsuarioCountNotaDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal async Task<string> Insert(UsuarioCountNota usuarioCountNota)
        {
            await _ConexaoMongoDB.UsuarioCountNota.InsertOneAsync(usuarioCountNota);
            return usuarioCountNota.Id;
        }

        internal async void Update(UsuarioCountNota usuarioCountNota)
        {
            var condicao = Builders<UsuarioCountNota>.Filter.Eq(x => x.Id, usuarioCountNota.Id);
            await _ConexaoMongoDB.UsuarioCountNota.ReplaceOneAsync(condicao, usuarioCountNota);
        }

        internal async Task<UsuarioCountNota> List(string usuarioId, string fileDCMId)
        {
            var nota = await _ConexaoMongoDB.UsuarioCountNota.Find(x => x.fileDCMId == fileDCMId 
            && x.usuarioId == usuarioId).FirstOrDefaultAsync();

            return nota;
        }

        internal async Task<List<UsuarioCountNota>> ListUsuario(string usuarioId)
        {
            var condicao = Builders<UsuarioCountNota>.Filter.Eq(x => x.usuarioId, usuarioId);
            var listaUsuarioCountNota = await _ConexaoMongoDB.UsuarioCountNota.Find(condicao).ToListAsync();

            return listaUsuarioCountNota;
        }
    }
}
