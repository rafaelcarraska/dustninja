using DustMedicalNinja.Models;
using DustMedicalNinja.Models.Heranca;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;


namespace DustMedicalNinja.Models.ViewModel
{
    [DataContract]
    public class StatusExameViewModel : GrupoEmpresa
    {
        [DataMember]
        public bool usuarioLogado { get; set; }

        public DateTime date_envio { get; set; }

        [DataMember]
        public string data_envio_formatada
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy HH:mm:ss}", date_envio);
            }
        }

        [DataMember]
        public StatusExames statusExames { get; set; }

        [DataMember]
        public SubStatusExames subStatusExames { get; set; }

        [DataMember]
        public string statusExamesDescricao
        {
            get
            {
                switch (statusExames)
                {
                    case StatusExames.transmissao:
                        return "Em transmissão";
                    case StatusExames.confirmar:
                        return "A confirmar";
                    case StatusExames.laudar:
                        if (SubStatusExames.laudar_comPreLaudo == subStatusExames)
                        {
                            return "A laudar com pré-laudo";
                        }
                        else if (SubStatusExames.laudar_segundaLeitura == subStatusExames)
                        {
                            return "A laudar aguardando 2ª leitura";
                        }
                        else if (SubStatusExames.laudar_reiterpretacao == subStatusExames)
                        {
                            return "A laudar solicitada reinterpretação";
                        }
                        else if (SubStatusExames.laudar_pendencia == subStatusExames)
                        {
                            return "A laudar com pendência";
                        }
                        return "A laudar";
                    case StatusExames.laudando:
                        return "Laudando";
                    case StatusExames.laudado:
                        return "Laudado";
                    case StatusExames.desconsiderado:
                        return "Desconsiderado";
                    case StatusExames.comparacao:
                        return "Para comparação ";
                    default:
                        return "Status do exame indefinido";
                }
            }
        }

        [DataMember]
        public string statusExamesFormatado
        {
            get
            {
                return statusExames.ToString("g");
            }
        }

        [DataMember]
        public string subStatusExamesFormatado
        {
            get
            {
                return subStatusExames.ToString("g");
            }
        }

        [DataMember]
        public string prioridade { get; set; }

        [DataMember]
        public string prioridadeFormatada
        {
            get
            {
                if (TipoPrioridade.normal.ToString("g") == prioridade)
                {
                    return "Normal";
                }
                if (TipoPrioridade.urgente.ToString("g") == prioridade)
                {
                    return "Urgência";
                }
                if (TipoPrioridade.critico.ToString("g") == prioridade)
                {
                    return "Crítico";
                }
                return "Normal";
            }
        }
    }
}
