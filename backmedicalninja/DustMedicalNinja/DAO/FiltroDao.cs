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
    public class FiltroDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal async Task<string> Insert(Filtro filtro)
        {
            await _ConexaoMongoDB.Filtro.InsertOneAsync(filtro);
            return filtro.Id;
        }

        internal async void InsertList(List<Filtro> listFiltro)
        {
            await _ConexaoMongoDB.Filtro.InsertManyAsync(listFiltro);
        }

        internal async Task<List<Filtro>> ListCombo(string empresaId, string usuarioId, string perfilId)
        {
            List<Filtro> listFiltro = await _ConexaoMongoDB.Filtro.Find(x =>
                x.status == true && x.empresaId == empresaId
                && (x.usuarioId == usuarioId || !x.particular)
            ).ToListAsync();

            return listFiltro;
        }

        internal async Task<List<Filtro>> ListaComboByFacility(List<string> listaFacilityId)
        {
            List<Filtro> listFiltro = await _ConexaoMongoDB.Filtro.Find(x => listaFacilityId.Contains(x.Id)
                                                    && x.status == true).ToListAsync();

            return listFiltro;
        }

        internal async void Update(Filtro filtro)
        {
            var condicao = Builders<Filtro>.Filter.Eq(x => x.Id, filtro.Id);
            await _ConexaoMongoDB.Filtro.ReplaceOneAsync(condicao, filtro);
        }

        internal async void Delete(string Id)
        {
            await _ConexaoMongoDB.Filtro.DeleteOneAsync(x => x.Id == Id);
        }

        internal async Task<List<Filtro>> ListAll(string empresaId, string usuarioId)
        {
            List<Filtro> listFiltro = await _ConexaoMongoDB.Filtro.Find(x =>
               x.status == true && x.empresaId == empresaId 
               && (x.usuarioId == usuarioId || !x.particular)
           ).ToListAsync();

            return listFiltro;
        }

        internal async Task<List<Filtro>> List(string filtroId)
        {
            var condicao = Builders<Filtro>.Filter.Eq(x => x.Id, filtroId);
            var lista = await _ConexaoMongoDB.Filtro.Find(condicao).ToListAsync();

            return lista;
        }
        internal async Task<long> ExisteDescricao(Filtro filtro)
        {
            try
            {
                long qtd;
                if (!string.IsNullOrEmpty(filtro.Id))
                {
                    qtd = await _ConexaoMongoDB.Filtro.Find(x => 
                    x.Id != filtro.Id &&
                    x.empresaId == filtro.empresaId &&
                    x.descricao == filtro.descricao).CountDocumentsAsync();
                }
                else
                {
                    qtd = await _ConexaoMongoDB.Filtro.Find(x =>
                    x.empresaId == filtro.empresaId &&
                    x.descricao == filtro.descricao).CountDocumentsAsync();
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
