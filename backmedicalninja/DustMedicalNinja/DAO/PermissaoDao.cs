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
    public class PermissaoDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();       

        internal void Insert_List(List<Permissao> listaPermissao)
        {
            _ConexaoMongoDB.Permissao.InsertManyAsync(listaPermissao);
        }

        internal void Delete_FacilityUsuario(string facilityId, string usuarioId)
        {
            _ConexaoMongoDB.Permissao.DeleteMany(x => x.facilityId == facilityId && x.usuarioId == usuarioId);
        }

        internal void Delete_FacilityId(string facilityId)
        {
            _ConexaoMongoDB.Permissao.DeleteMany(x => x.facilityId == facilityId);
        }

        internal void Delete_UsuarioId(string usuarioId)
        {
            _ConexaoMongoDB.Permissao.DeleteMany(x => x.usuarioId == usuarioId);
        }

        internal async Task<List<Permissao>> List_FacilityId(string facilityId)
        {
            var condicao = Builders<Permissao>.Filter.Eq(x => x.facilityId, facilityId);
            var lista = await _ConexaoMongoDB.Permissao.Find(condicao).ToListAsync();

            return lista;
        }

        internal async Task<List<Permissao>> List_UsuarioId(string usuarioId)
        {
            var condicao = Builders<Permissao>.Filter.Eq(x => x.usuarioId, usuarioId);
            var lista = await _ConexaoMongoDB.Permissao.Find(condicao).ToListAsync();

            return lista;
        }
    }
}
