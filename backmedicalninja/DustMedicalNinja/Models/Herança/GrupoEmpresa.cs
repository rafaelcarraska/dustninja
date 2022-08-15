using System.Runtime.Serialization;

namespace DustMedicalNinja.Models.Heranca
{
    [DataContract]
    public class GrupoEmpresa : BaseModal 
    {
        public string empresaId { get; set; }
    }
}
