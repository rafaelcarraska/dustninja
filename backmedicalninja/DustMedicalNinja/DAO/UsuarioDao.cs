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
    public class UsuarioDao
    {
        ConexaoMongoDB _ConexaoMongoDB = new ConexaoMongoDB();

        internal async Task<string> Insert(Usuario usuario)
        {
            await _ConexaoMongoDB.Usuario.InsertOneAsync(usuario);
            return usuario.Id;
        }

        internal void InsertList(List<Usuario> list_usuario)
        {
            _ConexaoMongoDB.Usuario.InsertMany(list_usuario);
        }

        internal async void InsertListAsync(List<Usuario> list_usuario)
        {
            await _ConexaoMongoDB.Usuario.InsertManyAsync(list_usuario);
        }

        internal async void Update(Usuario usuario)
        {
            var condicao = Builders<Usuario>.Filter.Eq(x => x.Id, usuario.Id);
            await _ConexaoMongoDB.Usuario.ReplaceOneAsync(condicao, usuario);
        }

        internal async void Delete(string Id)
        {
            await _ConexaoMongoDB.Usuario.DeleteOneAsync(x => x.Id == Id);
        }

        internal async Task<List<Usuario>> ListAll()
        {
            List<Usuario> list_usuario = await _ConexaoMongoDB.Usuario.Find(new BsonDocument()).ToListAsync();

            return list_usuario;
        }

        internal async Task<List<Usuario>> ListAllMaster()
        {
            List<Usuario> list_usuario = await _ConexaoMongoDB.Usuario.Find(new BsonDocument()).ToListAsync();
            
            return list_usuario;
        }

        internal async Task<List<Usuario>> ListaCombo()
        {
            var condicao = Builders<Usuario>.Filter.Eq(x => x.status, true);
            List<Usuario> list_usuario = await _ConexaoMongoDB.Usuario.Find(condicao).ToListAsync();

            return list_usuario;
        }

        //internal List<Combobox> ListaComboUsuarioMaster()
        //{
        //    var query = from u in _ConexaoMongoDB.Usuario.AsQueryable()
        //                join p in _ConexaoMongoDB.Perfil.AsQueryable() on u.perfilId equals p.Id
        //                where u.status == true
        //                select new Combobox()
        //                {
        //                    Id = u.Id,
        //                    descricao = u.nome,
        //                    filtro1 = p.descricao,
        //                    filtroId1 = p.Id,
        //                    dataInclusao = u.log.insertData
        //                };


        //    return query.ToList();
        //}

        //internal async Task<List<UsuarioViewModel>> ListaComboUsuario()
        //{
        //    var query = from u in _ConexaoMongoDB.Usuario.AsQueryable().Where(x => x.status == true)
        //                join p in _ConexaoMongoDB.Perfil.AsQueryable() on u.perfilId equals p.Id                                            
        //                select new UsuarioViewModel()
        //                {
        //                    Id = u.Id,
        //                    nome = u.nome,
        //                    perfil = p.descricao,
        //                    perfilId = p.Id,
        //                    dataInclusao = u.log.insertData,
        //                    listaEmpresa = u.listaEmpresa
        //                };


        //    return query.ToList();
        //}

        internal async Task<Usuario> List(string usuarioId)
        {
            var condicao = Builders<Usuario>.Filter.Eq(x => x.Id, usuarioId);
            var usuario = await _ConexaoMongoDB.Usuario.Find(condicao).FirstOrDefaultAsync();

            return usuario;
        }

        internal async Task<long> CountPerfilId(string perfilId)
        {
            var CondicaoByPerfilId = Builders<Usuario>.Filter.Eq(x => x.perfilId, perfilId);
            var count = await _ConexaoMongoDB.Usuario.Find(CondicaoByPerfilId).CountDocumentsAsync();

            return count;
        }

        internal async Task<long> EmpresaEmUso(string empresaId)
        {
            long listaEmpresaCount = await _ConexaoMongoDB.Usuario.Find(x => x.listaEmpresa.Contains(empresaId)).CountDocumentsAsync();

            return listaEmpresaCount;
        }

        internal async Task<Usuario> Autenticacao(Autenticacao autenticacao)
        {
            var usuario = await _ConexaoMongoDB.Usuario
                .Find(x => x.login == autenticacao.login && x.senha == autenticacao.senha).FirstOrDefaultAsync();

            return usuario;
        }

        internal async Task<long> ExisteNome(Usuario usuario)
        {
            try
            {
                long qtd;
                if (!string.IsNullOrEmpty(usuario.Id))
                {
                    qtd = await _ConexaoMongoDB.Usuario.Find(x =>
                    x.Id != usuario.Id &&
                    x.nome == usuario.nome).CountDocumentsAsync();
                }
                else
                {
                    qtd = await _ConexaoMongoDB.Usuario.Find(x =>
                    x.nome == usuario.nome).CountDocumentsAsync();
                }

                return qtd;
            }
            catch (Exception ex)
            {
                string er = ex.Message;
                throw;
            }

        }

        internal async Task<long> ExisteLogin(Usuario usuario)
        {
            try
            {
                long qtd;
                if (!string.IsNullOrEmpty(usuario.Id))
                {
                    qtd = await _ConexaoMongoDB.Usuario.Find(x =>
                    x.Id != usuario.Id &&
                    x.login == usuario.login).CountDocumentsAsync();
                }
                else
                {
                    qtd = await _ConexaoMongoDB.Usuario.Find(x =>
                    x.login == usuario.login).CountDocumentsAsync();
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
