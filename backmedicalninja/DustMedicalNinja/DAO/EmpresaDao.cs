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
    public class EmpresaDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal async Task<string> Insert(Empresa empresa)
        {
            await _ConexaoMongoDB.Empresa.InsertOneAsync(empresa);
            return empresa.Id;
        }

        internal void InsertList(List<Empresa> list_empresa)
        {
            _ConexaoMongoDB.Empresa.InsertManyAsync(list_empresa);
        }

        internal async void InsertListAsync(List<Empresa> list_empresa)
        {
            await _ConexaoMongoDB.Empresa.InsertManyAsync(list_empresa);
        }

        internal async void Update(Empresa empresa)
        {
            var condicao = Builders<Empresa>.Filter.Eq(x => x.Id, empresa.Id);
            await _ConexaoMongoDB.Empresa.ReplaceOneAsync(condicao, empresa);
        }


        internal async void Delete(string Id)
        {
            await _ConexaoMongoDB.Empresa.DeleteOneAsync(x => x.Id == Id);
        }

        internal async Task<List<Empresa>> ListAll()
        {
            List<Empresa> list_empresa = await _ConexaoMongoDB.Empresa.Find(new BsonDocument()).ToListAsync();

            return list_empresa;
        }

        internal async Task<List<Empresa>> ListaCombo()
        {
            var condicao = Builders<Empresa>.Filter.Eq(x => x.status, true);
            List<Empresa> list_empresa = await _ConexaoMongoDB.Empresa.Find(condicao).ToListAsync();

            return list_empresa;
        }

        internal async Task<List<Empresa>> listaComboLogin(List<string> listaEmpresa)
        {
            var condicao = Builders<Empresa>.Filter.In(x => x.Id, listaEmpresa);
            List<Empresa> list_empresa = await _ConexaoMongoDB.Empresa.Find(condicao).ToListAsync();

            return list_empresa;
        }

        internal async Task<List<Empresa>> Lista(string empresaId)
        {
            var condicao = Builders<Empresa>.Filter.Eq(x => x.Id, empresaId);
            var lista = await _ConexaoMongoDB.Empresa.Find(condicao).ToListAsync();

            return lista;
        }

        internal async Task<long> ExisteRazaoSocial(Empresa empresa)
        {
            try
            {
                long qtd;
                if (!string.IsNullOrEmpty(empresa.Id))
                {
                    qtd = await _ConexaoMongoDB.Empresa.Find(x =>
                    x.Id != empresa.Id &&
                    x.razaoSocial == empresa.razaoSocial).CountDocumentsAsync();
                }
                else
                {
                    qtd = await _ConexaoMongoDB.Empresa.Find(x =>
                    x.razaoSocial == empresa.razaoSocial).CountDocumentsAsync();
                }

                return qtd;
            }
            catch (Exception ex)
            {
                string er = ex.Message;
                throw;
            }

        }

        internal async Task<long> ExisteNomeFantasia(Empresa empresa)
        {
            try
            {
                long qtd;
                if (!string.IsNullOrEmpty(empresa.Id))
                {
                    qtd = await _ConexaoMongoDB.Empresa.Find(x =>
                    x.Id != empresa.Id &&
                    x.nomeFantasia == empresa.nomeFantasia).CountDocumentsAsync();
                }
                else
                {
                    qtd = await _ConexaoMongoDB.Empresa.Find(x =>
                    x.nomeFantasia == empresa.nomeFantasia).CountDocumentsAsync();
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
