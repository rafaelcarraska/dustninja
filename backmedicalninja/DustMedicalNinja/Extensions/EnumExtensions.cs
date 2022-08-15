using DustMedicalNinja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustMedicalNinja.Extensions
{
    public static class EnumExtensions
    {
        public static List<string> randomArray(this List<string> lista)
        {
            return lista.OrderBy(x => new Guid())
                .Take(new Random().Next(lista.Count())).ToList();
        }

        public static string randomArrayOne(this List<string> lista)
        {
            return lista[new Random().Next(lista.Count())];
        }

        public static TipoPrioridade TipoPrioridadeConvert(this string tipoPrioridade)
        {
            if (string.IsNullOrEmpty(tipoPrioridade))
            {
                return TipoPrioridade.normal;
            }
            return (TipoPrioridade)Enum.Parse(typeof(TipoPrioridade), tipoPrioridade);
        }

        public static StatusExames StatusExamesConvert(this string statusExames)
        {
            if (string.IsNullOrEmpty(statusExames))
            {
                return StatusExames.transmissao;
            }
            return (StatusExames)Enum.Parse(typeof(StatusExames), statusExames);
        }

        public static SubStatusExames SubStatusExamesConvert(this string subStatusExames)
        {
            if(string.IsNullOrEmpty(subStatusExames))
            {
                return SubStatusExames.novo;
            }
            return (SubStatusExames)Enum.Parse(typeof(SubStatusExames), subStatusExames);
        }
    }
}
