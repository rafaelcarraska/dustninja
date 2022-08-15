using DustMedicalNinja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustMedicalNinja.Extensions
{
    public static class FileDCMExtensions
    {
        public static bool UsuarioLaudando(this FileDCM fileDCM, string usuarioId)
        {
            if (fileDCM.historicoExame != null && fileDCM.historicoExame.Count > 0)
            {
                var maxData = fileDCM.historicoExame.DataUltimoHistorico(fileDCM.log.updateData);
                var ultimoHistorico = fileDCM.historicoExame.FirstOrDefault(x => x.log.updateData == maxData);

                if (ultimoHistorico.statusExames == StatusExames.laudando.ToString("g") && ultimoHistorico.log.updateUsuarioId != usuarioId)
                {
                    return false;
                }
            }

            return true;
        }

        public static string TemplateLaudando(this FileDCM fileDCM)
        {
            if (fileDCM.historicoExame != null && fileDCM.historicoExame.Count > 0)
            {
                var ultimoHistorico = fileDCM.historicoExame.Where(x => x.statusExames == StatusExames.laudar.ToString("g") && x.templateImpressaoid != null).ToList();


                if (ultimoHistorico != null && ultimoHistorico.Count > 0)
                {
                    var maxData = ultimoHistorico.DataUltimoHistorico(fileDCM.log.updateData);
                    return fileDCM.historicoExame.FirstOrDefault(x => x.log.updateData == maxData).templateImpressaoid;
                }
            }

            return string.Empty;
        }

        public static DateTime DataUltimoHistorico(this List<Confirmacao> historicoExame, DateTime updateData)
        {
            if (historicoExame != null && historicoExame.Count > 0)
            {
                return historicoExame.Max(x => x.log.updateData);
            }

            return updateData;
        }

        public static Confirmacao ListaStatus(this List<Confirmacao> historicoExame, StatusExames statusExame)
        {
            if (historicoExame != null) {
                //TODO Remover o "or" do linq abaixo, é apenas correção paleativa, pois as vezes esta salvando o int e nao a string do enun
                var listaStatus = historicoExame.Where(x => x.statusExames == statusExame.ToString("g") || x.statusExames == statusExame.ToString()).ToList();
                if (listaStatus != null && listaStatus.Count > 0)
                {
                    return listaStatus.FirstOrDefault(y => y.log.updateData == listaStatus.Max(x => x.log.updateData));

                }
            }
            return null;
        }

        public static string SexoToString(this string sexo)
        {
            switch (sexo.ToUpper().Trim())
            {
                case "F":
                    return "Feminino";
                case "M":
                    return "Masculino";
                default:
                    return "sexo";
            }
        }
    }
}
