using DustMedicalNinja.DAO;
using DustMedicalNinja.Models;
using DustMedicalNinja.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustMedicalNinja.Business
{
    internal class EnderecoBusiness : Uteis
    {
        Msg msg = new Msg();

        internal Msg Validar(Perfil perfil)
        {
            msg = new Msg();
            List<string> erros = new List<string>();

            msg.erro = List_Erros(msg.erro);
            return msg;
        }

        internal Endereco Input()
        {
            try
            {
                return new Endereco
                {
                    endereco = "rua " + Aleatorio(10, 3),
                    numero = Aleatorio(4, 1, false, false, true, false),
                    bairro = "bairo " + Aleatorio(8, 3, true, true, true, false),
                    complemento = Aleatorio(10, 5),
                    cep = "00000-000",
                    cidade = "São Paulo",
                    uf = "SP",
                    obs = Aleatorio(12, 20, true, true, true, true)
                };
            }
            catch (Exception)
            {
                return new Endereco();
            }

        }
    }
}
