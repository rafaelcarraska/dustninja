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
    public class BackupDao
    {
        private List<Empresa> ListAllEmpresa(ConexaoMongoDB De)
        {
            return De.Empresa.Find(new BsonDocument())
                .ToList();
        }
        internal void Empresa(ConexaoMongoDB De, ConexaoMongoDB Para)
        {
            Para.Empresa.InsertMany(ListAllEmpresa(De));
        }

        private List<Perfil> ListAllPerfil(ConexaoMongoDB De)
        {
            return De.Perfil.Find(new BsonDocument())
                .ToList();
        }
        internal void Perfil(ConexaoMongoDB De, ConexaoMongoDB Para)
        {
            Para.Perfil.InsertMany(ListAllPerfil(De));
        }

        private List<Usuario> ListAllUsusrio(ConexaoMongoDB De)
        {
            return De.Usuario.Find(new BsonDocument())
                .ToList();
        }
        internal void Usuario(ConexaoMongoDB De, ConexaoMongoDB Para)
        {
            Para.Usuario.InsertMany(ListAllUsusrio(De));
        }

        private List<Facility> ListAllFacility(ConexaoMongoDB De)
        {
           return De.Facility.Find(new BsonDocument())
                .ToList();
        }
        internal void Facility(ConexaoMongoDB De, ConexaoMongoDB Para)
        {
            Para.Facility.InsertMany(ListAllFacility(De));
        }


    }
}
