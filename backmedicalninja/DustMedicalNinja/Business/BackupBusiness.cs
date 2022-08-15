using DustMedicalNinja.DAO;
using DustMedicalNinja.Models;
using DustMedicalNinja.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DustMedicalNinja.Extensions;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace DustMedicalNinja.Business
{
    internal class BackupBusiness : Uteis
    {
        BackupDao _BackupDao = new BackupDao();
        

        //internal void Restore()
        //{
        //    ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();
        //    ConexaoMongoDB _ConexaoMongoDBDust = new ConexaoMongoDB();

        //    _BackupDao.Empresa(_ConexaoMongoDB, _ConexaoMongoDBDust);
        //    _BackupDao.Perfil(_ConexaoMongoDB, _ConexaoMongoDBDust);
        //    _BackupDao.Usuario(_ConexaoMongoDB, _ConexaoMongoDBDust);
        //    _BackupDao.Facility(_ConexaoMongoDB, _ConexaoMongoDBDust);
        //}

        //internal void Backup()
        //{
        //    ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();
        //    ConexaoMongoDB _ConexaoMongoDBDust = new ConexaoMongoDB();

        //    _BackupDao.Empresa(_ConexaoMongoDBDust, _ConexaoMongoDB);
        //    _BackupDao.Perfil(_ConexaoMongoDBDust, _ConexaoMongoDB);
        //    _BackupDao.Usuario(_ConexaoMongoDBDust, _ConexaoMongoDB);
        //    _BackupDao.Facility(_ConexaoMongoDBDust, _ConexaoMongoDB);
        //}
    }
}
