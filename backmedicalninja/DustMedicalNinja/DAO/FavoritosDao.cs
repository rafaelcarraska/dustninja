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
    public class FavoritosDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal async Task<string> Insert(Favoritos favoritos)
        {
            await _ConexaoMongoDB.Favoritos.InsertOneAsync(favoritos);
            return favoritos.Id;
        }

        internal async void Delete(string usuarioId, string filedcmId)
        {
            await _ConexaoMongoDB.Favoritos.DeleteOneAsync(x => x.usuarioId == usuarioId && x.filedcmId == filedcmId);
        }

        internal async Task<Favoritos> ListFileCDMId(string usuarioId, string filedcmId)
        {           
            var favoritos = await _ConexaoMongoDB.Favoritos.Find(x => x.filedcmId == filedcmId && x. usuarioId == usuarioId).FirstOrDefaultAsync();

            return favoritos;
        }

        internal IEnumerable<Favoritos> List(string usuarioId)
        {
            var condicao = Builders<Favoritos>.Filter.Eq(x => x.usuarioId, usuarioId);
            var favoritos = _ConexaoMongoDB.Favoritos.Find(condicao).ToListAsync();

            return favoritos.Result;
        }
    }
}
