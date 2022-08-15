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
    public class NotaDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal async Task<string> Insert(Nota nota)
        {
            await _ConexaoMongoDB.Nota.InsertOneAsync(nota);
            return nota.Id;
        }

        internal async void Update(Nota nota)
        {
            var condicao = Builders<Nota>.Filter.Eq(x => x.Id, nota.Id);
            await _ConexaoMongoDB.Nota.ReplaceOneAsync(condicao, nota);
        }

        internal async void Delete(string Id)
        {
            await _ConexaoMongoDB.Nota.DeleteOneAsync(x => x.Id == Id);
        }

        internal async Task<Nota> List(string fileDCMId)
        {
            var condicao = Builders<Nota>.Filter.Eq(x => x.fileDCMId, fileDCMId);
            var nota = await _ConexaoMongoDB.Nota.Find(condicao).FirstOrDefaultAsync();

            return nota;
        }

        internal async Task<bool> Existe(string fileDCMId)
        {
            var condicao = Builders<Nota>.Filter.Eq(x => x.fileDCMId, fileDCMId);
            var nota = await _ConexaoMongoDB.Nota.Find(condicao).AnyAsync();

            return nota;
        }

        internal async Task<Nota> ListNota(string notaID)
        {
            var condicao = Builders<Nota>.Filter.Eq(x => x.Id, notaID);
            var nota = await _ConexaoMongoDB.Nota.Find(condicao).FirstOrDefaultAsync();

            return nota;
        }

        internal int CountNota(string fileDCMId, string usuarioId)
        {
            var countAnexo = _ConexaoMongoDB.Nota.Find(x => x.fileDCMId == fileDCMId && x.listaNota.Count() > 0)
                .FirstOrDefault()?.listaNota.Count(y => y.usuarioId != usuarioId);

            return countAnexo??0;
        }

        internal int CountNotaData(string fileDCMId, DateTime data, string usuarioId)
        {
            var countAnexo = _ConexaoMongoDB.Nota.Find(x => x.fileDCMId == fileDCMId && x.listaNota
                .Count() > 0)
                .FirstOrDefault()?.listaNota.Count(y => y.data >= data && y.usuarioId != usuarioId);

            return countAnexo??0;
        }
    }
}
