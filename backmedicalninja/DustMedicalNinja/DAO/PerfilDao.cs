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
    public class PerfilDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal async Task<string> Insert(Perfil perfil)
        {
            await _ConexaoMongoDB.Perfil.InsertOneAsync(perfil);
            return perfil.Id;
        }

        internal void InsertList(List<Perfil> list_perfil)
        {
            _ConexaoMongoDB.Perfil.InsertMany(list_perfil);
        }

        internal async void InsertListAsysnc(List<Perfil> list_perfil)
        {
            await _ConexaoMongoDB.Perfil.InsertManyAsync(list_perfil);
        }

        internal async void Update(Perfil perfil)
        {
            var condicao = Builders<Perfil>.Filter.Eq(x => x.Id, perfil.Id);
            await _ConexaoMongoDB.Perfil.ReplaceOneAsync(condicao, perfil);
        }

        internal async void Delete(string Id)
        {
            await _ConexaoMongoDB.Perfil.DeleteOneAsync(x => x.Id == Id);
        }

        internal async Task<List<Perfil>> ListAll()
        {
            List<Perfil> list_perfil = await _ConexaoMongoDB.Perfil.Find(new BsonDocument())
                .ToListAsync();

            return list_perfil;
        }

        internal async Task<List<Perfil>> ListCombo()
        {
            List<Perfil> list_perfil = await _ConexaoMongoDB.Perfil.Find(x => 
                x.status == true
            ).ToListAsync();

            return list_perfil;
        }

        internal async Task<List<Perfil>> List(string perfilId)
        {
            var condicao = Builders<Perfil>.Filter.Eq(x => x.Id, perfilId);
            var lista = await _ConexaoMongoDB.Perfil.Find(condicao).ToListAsync();

            return lista;
        }
        
        internal async Task<long> ExisteDescricao(Perfil perfil)
        {
            try
            {
                long qtd;
                if (!string.IsNullOrEmpty(perfil.Id))
                {
                    qtd = await _ConexaoMongoDB.Perfil.Find(x => 
                    x.Id != perfil.Id &&
                    x.descricao == perfil.descricao).CountDocumentsAsync();
                }
                else
                {
                    qtd = await _ConexaoMongoDB.Perfil.Find(x =>
                    x.descricao == perfil.descricao).CountDocumentsAsync();
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
