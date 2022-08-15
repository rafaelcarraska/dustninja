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
    public class AnexoDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal async Task<string> Insert(Anexo anexo)
        {
            await _ConexaoMongoDB.Anexo.InsertOneAsync(anexo);
            return anexo.Id;
        }

        internal async void Update(Anexo anexo
            )
        {
            var condicao = Builders<Anexo>.Filter.Eq(x => x.Id, anexo.Id);
            await _ConexaoMongoDB.Anexo.ReplaceOneAsync(condicao, anexo);
        }

        internal async void Delete(string Id)
        {
            await _ConexaoMongoDB.Anexo.DeleteOneAsync(x => x.Id == Id);
        }

        internal async Task<List<Anexo>> List(string fileDCMId)
        {
            var condicao = Builders<Anexo>.Filter.Eq(x => x.fileDCMId, fileDCMId);
            var listaAnexo = await _ConexaoMongoDB.Anexo.Find(condicao).ToListAsync();

            return listaAnexo;
        }

        internal async Task<Anexo> ListAnexo(string anexoId)
        {
            var condicao = Builders<Anexo>.Filter.Eq(x => x.Id, anexoId);
            var anexo = await _ConexaoMongoDB.Anexo.Find(condicao).FirstOrDefaultAsync();

            return anexo;
        }

        internal async Task<long> CountAnexo(string fileDCMId)
        {
            var condicao = Builders<Anexo>.Filter.Eq(x => x.fileDCMId, fileDCMId);
            var countAnexo = await _ConexaoMongoDB.Anexo.Find(condicao).CountDocumentsAsync();

            return countAnexo;
        }

        internal async Task<List<Anexo>> ListaAnexoByFileDCMId(List<string> listaFileDCMId)
        {
            List<Anexo> listAnexo = await _ConexaoMongoDB.Anexo.Find(x => listaFileDCMId.Contains(x.fileDCMId)
                                                    && x.status == true).ToListAsync();

            return listAnexo;
        }
    }
}
