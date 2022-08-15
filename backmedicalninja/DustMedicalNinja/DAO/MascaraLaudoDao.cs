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
    public class MascaraLaudoDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal async Task<string> Insert(MascaraLaudo mascaraLaudo)
        {
            await _ConexaoMongoDB.MascaraLaudo.InsertOneAsync(mascaraLaudo);
            return mascaraLaudo.Id;
        }

        internal async void InsertList(List<MascaraLaudo> listMascaraLaudo)
        {
            await _ConexaoMongoDB.MascaraLaudo.InsertManyAsync(listMascaraLaudo);
        }

        internal async void Update(MascaraLaudo mascaraLaudo)
        {
            var condicao = Builders<MascaraLaudo>.Filter.Eq(x => x.Id, mascaraLaudo.Id);
            await _ConexaoMongoDB.MascaraLaudo.ReplaceOneAsync(condicao, mascaraLaudo);
        }

        internal async void Delete(string Id)
        {
            await _ConexaoMongoDB.MascaraLaudo.DeleteOneAsync(x => x.Id == Id);
        }

        internal async Task<List<MascaraLaudo>> ListAll(string empresaId)
        {
            var byEmpresa = Builders<MascaraLaudo>.Filter.Eq(x => x.empresaId, empresaId);
            List<MascaraLaudo> listMascaraLaudo = await _ConexaoMongoDB.MascaraLaudo.Find(byEmpresa).ToListAsync();

            return listMascaraLaudo;
        }

        internal async Task<List<MascaraLaudo>> ListAllAtivo(string empresaId)
        {
            var byEmpresa = Builders<MascaraLaudo>.Filter.Eq(x => x.empresaId, empresaId);
            List<MascaraLaudo> listMascaraLaudo = await _ConexaoMongoDB.MascaraLaudo.Find(x => x.empresaId == empresaId && x.status == true).ToListAsync();

            return listMascaraLaudo;
        }

        internal async Task<List<MascaraLaudo>> ListCombo(string empresaId)
        {
            List<MascaraLaudo> list_mascara = await _ConexaoMongoDB.MascaraLaudo.Find(x =>
                x.status == true && x.empresaId == empresaId
            ).ToListAsync();

            return list_mascara;
        }

        internal async Task<List<MascaraLaudo>> List(string mascaraLaudoId)
        {
            var condicao = Builders<MascaraLaudo>.Filter.Eq(x => x.Id, mascaraLaudoId);
            var lista = await _ConexaoMongoDB.MascaraLaudo.Find(condicao).ToListAsync();

            return lista;
        }
        internal async Task<long> ExisteDescricao(MascaraLaudo mascaraLaudo)
        {
            try
            {
                long qtd;
                if (!string.IsNullOrEmpty(mascaraLaudo.Id))
                {
                    qtd = await _ConexaoMongoDB.MascaraLaudo.Find(x => 
                    x.Id != mascaraLaudo.Id &&
                    x.empresaId == mascaraLaudo.empresaId &&
                    x.descricao == mascaraLaudo.descricao).CountDocumentsAsync();
                }
                else
                {
                    qtd = await _ConexaoMongoDB.MascaraLaudo.Find(x =>
                    x.empresaId == mascaraLaudo.empresaId &&
                    x.descricao == mascaraLaudo.descricao).CountDocumentsAsync();
                }                                

                return qtd;
            }
            catch (Exception ex)
            {
                string er = ex.Message;
                throw;
            }

        }
    }
}
