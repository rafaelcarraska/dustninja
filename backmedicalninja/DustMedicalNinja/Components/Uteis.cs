using DustMedicalNinja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DustMedicalNinja.Extensions;

namespace DustMedicalNinja.Components
{
    internal class Uteis
    {
        private static Random random = new Random();

        internal string Valida_Periodo(DateTime De, DateTime Ate)
        {
            string Data_Inicial = De.ToString("yyyy-MM-dd");
            string Data_Final = Ate.ToString("yyyy-MM-dd");

            if (Convert.ToUInt32(Data_Final.Substring(0, 4) + Data_Final.Substring(5, 2) + Data_Final.Substring(8)) < Convert.ToUInt32(Data_Inicial.Substring(0, 4) + Data_Inicial.Substring(5, 2) + Data_Inicial.Substring(8)))
                return "Informe uma Data Final igual ou posterior a Data Inicial.";
            else return string.Empty;
        }

        internal List<string> List_Erros(List<string> erros)
        {
            if (erros == null)
                return null;
            
            return erros.Where(e => e != string.Empty).Distinct().Any()? 
                erros.Where(e => e != string.Empty).Distinct().ToList(): null;
            
        }

        internal static string Aleatorio(int lengthMax = 10, int tamanho = 1, bool letras = true, bool maiusculas = false, bool numeros = true, bool caracteresEspeciais = false, string preTexto = "")
        {
            string chars = "abcdefghijklmnopqrstuvwxyz";

            chars = letras ? chars : string.Empty;
            chars = maiusculas? chars + chars.ToUpper(): chars;
            chars = numeros ? chars + "0123456789" : chars;
            chars = caracteresEspeciais ? chars + "!@#$%¨&*()" : chars;

            preTexto += " "+ new string(Enumerable.Repeat(chars, new Random().Next(0, lengthMax))
              .Select(s => s[random.Next(s.Length)]).ToArray());

            if (tamanho > 1)
            {
                preTexto = Aleatorio(lengthMax, tamanho - 1, letras, maiusculas, numeros, caracteresEspeciais, preTexto);
            }
            return preTexto;
        }
    }
}
