using DustMedicalNinja.DAO;
using DustMedicalNinja.Models;
using DustMedicalNinja.Models.AiModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustMedicalNinja.DAO
{
    public class DesmembrarAiDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal async Task<string> Insert(DesmembrarAi desmembrarAi)
        {
            await _ConexaoMongoDB.DesmembrarAi.InsertOneAsync(desmembrarAi);
            return desmembrarAi.Id;
        }
    }
}
