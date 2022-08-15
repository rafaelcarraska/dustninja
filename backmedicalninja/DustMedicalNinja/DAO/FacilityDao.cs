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
    public class FacilityDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal async Task<string> Insert(Facility facility)
        {
            await _ConexaoMongoDB.Facility.InsertOneAsync(facility);
            return facility.Id;
        }

        internal async void InsertList(List<Facility> list_facility)
        {
            await _ConexaoMongoDB.Facility.InsertManyAsync(list_facility);
        }

        internal async void Update(Facility facility)
        {
            var condicao = Builders<Facility>.Filter.Eq(x => x.Id, facility.Id);
            await _ConexaoMongoDB.Facility.ReplaceOneAsync(condicao, facility);
        }

        internal async void Delete(string Id)
        {
            await _ConexaoMongoDB.Facility.DeleteOneAsync(x => x.Id == Id);
        }

        internal async Task<List<Facility>> ListAllAtiva(string empresaId)
        {
            List<Facility> list_facility = await _ConexaoMongoDB.Facility.Find(x => x.empresaId == empresaId
                                                     && x.status == true).ToListAsync();

            return list_facility;
        }

        internal async Task<List<Facility>> ListAll(string empresaId)
        {
            var byEmpresa = Builders<Facility>.Filter.Eq(x => x.empresaId, empresaId);
            var list_facility = await _ConexaoMongoDB.Facility.Find(byEmpresa)
                .ToListAsync();

            return list_facility;
        }

        internal async Task<List<Facility>> ListaCombo(string empresaId)
        {
            List<Facility> list_facility = await _ConexaoMongoDB.Facility.Find(x => x.empresaId == empresaId 
                                                    && x.status == true).ToListAsync();

            return list_facility;
        }

        internal async Task<List<Facility>> ListaByUsuario(string empresaId, string usuarioId)
        {
            List<Facility> list_facilityEmpresaID = _ConexaoMongoDB.Facility.Find(x => x.empresaId == empresaId && x.status == true).ToList();
            IEnumerable<string> list_permissao = _ConexaoMongoDB.Permissao.Find(x => x.usuarioId == usuarioId).ToList().Select(s => s.facilityId);


           return list_facilityEmpresaID.Where(x => list_permissao.Contains(x.Id)).ToList();
        }

        internal async Task<List<Facility>> ListaByUsuarioEmpresaId(string empresaId)
        {
            List<Facility> list_facility = await _ConexaoMongoDB.Facility.Find(x => x.empresaId == empresaId
                                                    && x.status == true).ToListAsync();

            return list_facility;
        }

        internal async Task<List<Facility>> ListaByUsuarioListaEmpresa(List<string> listaEmpresaId)
        {
            List<Facility> list_facility = await _ConexaoMongoDB.Facility.Find(x => listaEmpresaId.Contains(x.empresaId)
                                                    && x.status == true).ToListAsync();

            return list_facility;
        }

        internal async Task<long> ListaFacilityCount(string empresaId)
        {
            var byEmpresa = Builders<Facility>.Filter.Eq(x => x.empresaId, empresaId);
            var list_facility = await _ConexaoMongoDB.Facility.Find(byEmpresa)
                .CountDocumentsAsync();

            return list_facility;
        }        

        internal async Task<List<Facility>> List(string facilityId)
        {
            var condicao = Builders<Facility>.Filter.Eq(x => x.Id, facilityId);
            var lista = await _ConexaoMongoDB.Facility.Find(condicao).ToListAsync();

            return lista;
        }

        internal async Task<List<Facility>> ListaEaTitle(string aeTitle)
        {
            var condicao = Builders<Facility>.Filter.Eq(x => x.aeTitle, aeTitle);
            var lista = await _ConexaoMongoDB.Facility.Find(condicao).ToListAsync();

            return lista;
        }

        internal async Task<long> ExisteDescricao(Facility facility)
        {
            try
            {
                long qtd;
                if (!string.IsNullOrEmpty(facility.Id))
                {
                    qtd = await _ConexaoMongoDB.Facility.Find(x =>
                    x.Id != facility.Id &&
                    x.empresaId == facility.empresaId &&
                    x.descricao == facility.descricao).CountDocumentsAsync();
                }
                else
                {
                    qtd = await _ConexaoMongoDB.Facility.Find(x =>
                    x.empresaId == facility.empresaId &&
                    x.descricao == facility.descricao).CountDocumentsAsync();
                }

                return qtd;
            }
            catch (Exception ex)
            {
                string er = ex.Message;
                return 1;
            }

        }
       
    }
}
