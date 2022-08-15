using DustMedicalNinja.DAO;
using DustMedicalNinja.Models;
using DustMedicalNinja.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustMedicalNinja.Business
{
    internal class AssinaturaBusiness : Uteis
    {
        Msg msg = new Msg();

        internal Assinatura Input()
        {
            try
            {
                var acesso = new Assinatura()
                {
                    arquivo = new Guid() + ".png",
                    linha1 = Aleatorio(10, 5),
                    linha2 = Aleatorio(10, 5),
                    linha3 = Aleatorio(10, 5)
                };

                return acesso;
            }
            catch (Exception)
            {
                return new Assinatura();
            }
        }
    }
}
