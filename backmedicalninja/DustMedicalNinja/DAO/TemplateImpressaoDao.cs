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
    public class TemplateImpressaoDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal async Task<string> Insert(TemplateImpressao templateImpressao)
        {
            await _ConexaoMongoDB.TemplateImpressao.InsertOneAsync(templateImpressao);
            return templateImpressao.Id;
        }

        internal async void InsertList(List<TemplateImpressao> listTemplateImpressao)
        {
            await _ConexaoMongoDB.TemplateImpressao.InsertManyAsync(listTemplateImpressao);
        }

        internal async Task<List<TemplateImpressao>> ListCombo(string empresaId)
        {
            List<TemplateImpressao> list_templateImpressao = await _ConexaoMongoDB.TemplateImpressao.Find(x =>
                x.status == true && x.empresaId == empresaId
            ).ToListAsync();

            return list_templateImpressao;
        }

        internal async Task<List<TemplateImpressao>> ListaComboByFacility(List<string> listaFacilityId)
        {
            List<TemplateImpressao> list_templateImpressao = await _ConexaoMongoDB.TemplateImpressao.Find(x => listaFacilityId.Contains(x.Id)
                                                    && x.status == true).ToListAsync();

            return list_templateImpressao;
        }

        internal async void Update(TemplateImpressao templateImpressao)
        {
            var condicao = Builders<TemplateImpressao>.Filter.Eq(x => x.Id, templateImpressao.Id);
            await _ConexaoMongoDB.TemplateImpressao.ReplaceOneAsync(condicao, templateImpressao);
        }

        internal async void Delete(string Id)
        {
            await _ConexaoMongoDB.TemplateImpressao.DeleteOneAsync(x => x.Id == Id);
        }

        internal async Task<List<TemplateImpressao>> ListAll(string empresaId)
        {
            var byEmpresa = Builders<TemplateImpressao>.Filter.Eq(x => x.empresaId, empresaId);
            List<TemplateImpressao> listTemplateImpressao = await _ConexaoMongoDB.TemplateImpressao.Find(byEmpresa).ToListAsync();

            return listTemplateImpressao;
        }

        internal async Task<List<TemplateImpressao>> List(string templateImpressaoId)
        {
            var condicao = Builders<TemplateImpressao>.Filter.Eq(x => x.Id, templateImpressaoId);
            var lista = await _ConexaoMongoDB.TemplateImpressao.Find(condicao).ToListAsync();

            return lista;
        }
        internal async Task<long> ExisteDescricao(TemplateImpressao templateImpressao)
        {
            try
            {
                long qtd;
                if (!string.IsNullOrEmpty(templateImpressao.Id))
                {
                    qtd = await _ConexaoMongoDB.TemplateImpressao.Find(x => 
                    x.Id != templateImpressao.Id &&
                    x.empresaId == templateImpressao.empresaId &&
                    x.descricao == templateImpressao.descricao).CountDocumentsAsync();
                }
                else
                {
                    qtd = await _ConexaoMongoDB.TemplateImpressao.Find(x =>
                    x.empresaId == templateImpressao.empresaId &&
                    x.descricao == templateImpressao.descricao).CountDocumentsAsync();
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
