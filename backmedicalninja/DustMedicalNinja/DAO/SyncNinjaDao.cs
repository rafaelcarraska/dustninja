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
    public class SyncNinjaDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal void InsertList(List<SyncNinja> list_syncNinja)
        {
            _ConexaoMongoDB.SyncNinja.InsertMany(list_syncNinja);
        }

        internal async Task<List<SyncNinja>> ListAll()
        {
            List<SyncNinja> list_sync = await _ConexaoMongoDB.SyncNinja.Find(new BsonDocument())
                .ToListAsync();

            return list_sync;
        }

        internal async Task<List<SyncNinja>> ListaSyncFileDCM(List<string> listaFileDCM)
        {
            List<SyncNinja> LlstaSyncFileDCM = await _ConexaoMongoDB.SyncNinja.Find(x => listaFileDCM.Contains(x.filedcm)
                                                    && x.status == true).ToListAsync();

            return LlstaSyncFileDCM;
        }
    }
}
