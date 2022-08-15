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
    public class WebConfigDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal async Task<string> Insert(WebConfig webConfig)
        {
            await _ConexaoMongoDB.WebConfig.InsertOneAsync(webConfig);
            return webConfig.Id;
        }

        internal async void Update(WebConfig webConfig)
        {
            var condicao = Builders<WebConfig>.Filter.Eq(x => x.Id, webConfig.Id);
            await _ConexaoMongoDB.WebConfig.ReplaceOneAsync(condicao, webConfig);
        }

        internal async void Delete(string Id)
        {
            await _ConexaoMongoDB.WebConfig.DeleteOneAsync(x => x.Id == Id);
        }

        internal async Task<WebConfig> List()
        {
            return await _ConexaoMongoDB.WebConfig.Find(new BsonDocument()).FirstOrDefaultAsync();
        }
    }
}
