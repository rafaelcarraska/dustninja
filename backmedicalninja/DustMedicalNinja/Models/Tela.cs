using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;


namespace DustMedicalNinja.Models
{
    [DataContract]
    public class Tela
    {
        [DataMember]
        public string descricao { get; set; }
        [DataMember]
        public bool master { get; set; }
        [DataMember]
        public List<string> permissao { get; set; }
    }

    public enum Permissoes
    {
        Visualizar,
        Adicionar,
        Editar,
        Deletar
    }

    public enum Telas
    {
        Worklist,
        Empresa,
        Facility,
        Usuario,
        Perfil,
        MascaraLaudo,
        TemplateImpressao,
        TipoExame,
        Filtros,
        Relatorio,
        Paciente
    }
}