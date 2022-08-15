using DustMedicalNinja.Models;
using DustMedicalNinja.Models.AiModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustMedicalNinja.DAO
{
    public class ConexaoMongoDB
    {
        private readonly string stringConexao = Startup.stringConexaoMongo;
        private readonly string Base = "DustMedicalNinja";

        private readonly IMongoClient _Cliente;
        private readonly IMongoDatabase _BasedeDados;

        internal ConexaoMongoDB()
        {
            _Cliente = new MongoClient(stringConexao);
            _BasedeDados = _Cliente.GetDatabase(Base);
        }

        public IMongoClient Cliente
        {
            get { return _Cliente; }
        }

        public IMongoCollection<FileDCM> FileDCM
        {
            get { return _BasedeDados.GetCollection<FileDCM>("FileDCM"); }
        }

        public IMongoCollection<Facility> Facility
        {
            get { return _BasedeDados.GetCollection<Facility>("Facility");
            }
        }

        public IMongoCollection<Empresa> Empresa
        {
            get
            {
                return _BasedeDados.GetCollection<Empresa>("Empresa");
            }
        }
        public IMongoCollection<Usuario> Usuario
        {
            get
            {
                return _BasedeDados.GetCollection<Usuario>("Usuario");
            }
        }
        public IMongoCollection<Perfil> Perfil
        {
            get
            {
                return _BasedeDados.GetCollection<Perfil>("Perfil");
            }
        }

        public IMongoCollection<MascaraLaudo> MascaraLaudo
        {
            get
            {
                return _BasedeDados.GetCollection<MascaraLaudo>("MascaraLaudo");
            }
        }

        public IMongoCollection<Log> Log
        {
            get
            {
                return _BasedeDados.GetCollection<Log>("Log");
            }
        }

        public IMongoCollection<Evento> Evento
        {
            get
            {
                return _BasedeDados.GetCollection<Evento>("Evento");
            }
        }

        public IMongoCollection<Permissao> Permissao
        {
            get
            {
                return _BasedeDados.GetCollection<Permissao>("Permissao");
            }
        }

        public IMongoCollection<TemplateImpressao> TemplateImpressao
        {
            get
            {
                return _BasedeDados.GetCollection<TemplateImpressao>("TemplateImpressao");
            }
        }

        public IMongoCollection<TipoExame> TipoExame
        {
            get
            {
                return _BasedeDados.GetCollection<TipoExame>("TipoExame");
            }
        }

        public IMongoCollection<Nota> Nota
        {
            get
            {
                return _BasedeDados.GetCollection<Nota>("Nota");
            }
        }

        public IMongoCollection<UsuarioCountNota> UsuarioCountNota
        {
            get
            {
                return _BasedeDados.GetCollection<UsuarioCountNota>("UsuarioCountNota");
            }
        }

        public IMongoCollection<Anexo> Anexo
        {
            get
            {
                return _BasedeDados.GetCollection<Anexo>("Anexo");
            }
        }

        public IMongoCollection<AnexoTemplate> AnexoTemplate
        {
            get
            {
                return _BasedeDados.GetCollection<AnexoTemplate>("AnexoTemplate");
            }
        }

        public IMongoCollection<SyncNinja> SyncNinja
        {
            get
            {
                return _BasedeDados.GetCollection<SyncNinja>("SyncNinja");
            }
        }

        public IMongoCollection<Paciente> Paciente
        {
            get
            {
                return _BasedeDados.GetCollection<Paciente>("Paciente");
            }
        }

        public IMongoCollection<Favoritos> Favoritos
        {
            get
            {
                return _BasedeDados.GetCollection<Favoritos>("Favoritos");
            }
        }

        public IMongoCollection<Configuracao> Configuracao
        {
            get
            {
                return _BasedeDados.GetCollection<Configuracao>("Configuracao");
            }
        }

        public IMongoCollection<WebConfig> WebConfig
        {
            get
            {
                return _BasedeDados.GetCollection<WebConfig>("WebConfig");
            }
        }

        public IMongoCollection<Filtro> Filtro
        {
            get
            {
                return _BasedeDados.GetCollection<Filtro>("Filtro");
            }
        }

        public IMongoCollection<DesmembrarAi> DesmembrarAi
        {
            get
            {
                return _BasedeDados.GetCollection<DesmembrarAi>("DesmembrarAi");
            }
        }
    }
}
