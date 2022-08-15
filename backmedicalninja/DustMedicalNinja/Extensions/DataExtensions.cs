using DustMedicalNinja.Business;
using DustMedicalNinja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustMedicalNinja.Extensions
{
    public static class DataExtensions
    {
        public static DateTime StringToDate(this string data, string horas = null)
        {
            try
            {
                var ano = Convert.ToInt16(data.Substring(0, 4));
                var mes = Convert.ToInt16(data.Substring(4, 2));
                var dia = Convert.ToInt16(data.Substring(6, 2));

                if (horas != null)
                {
                    var hora = Convert.ToInt16(horas.Substring(0, 2));
                    var minutos = Convert.ToInt16(horas.Substring(2, 2));
                    var segundos = Convert.ToInt16(horas.Substring(4, 2));

                    return new DateTime(ano, mes, dia, hora, minutos, segundos);
                }

                return new DateTime(ano, mes, dia);
            }
            catch (Exception)
            {
                return new DateTime();
            }
        }

        public static int Idade(this DateTime dataNasc)
        {
            try
            {
                var idade = DateTime.Now.Year - dataNasc.Year;
                if (DateTime.Now.Month < dataNasc.Month || (DateTime.Now.Month == dataNasc.Month && DateTime.Now.Day < dataNasc.Day))
                {
                    idade--;
                }

                return idade <= 0 ? 0 : idade;
            }
            catch (Exception)
            {
                return 0;
            }
        }


    }
}
