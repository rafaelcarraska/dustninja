using DustMedicalNinja.DAO;
using DustMedicalNinja.Models;
using DustMedicalNinja.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustMedicalNinja.Business
{
    internal class ContatoBusiness : Uteis
    {

        internal List<Contato> Input(int quantidadeMax)
        {
            try
            {
                var listaContato = new List<Contato>();
                for (int i = 0; i < new Random().Next(1, quantidadeMax); i++)
                {
                    listaContato.Add(new Contato
                    {
                        telefone = "11-97635-6278",
                        contato = "contato " + Aleatorio(8, 2)
                    });
                }

                return listaContato;
            }
            catch (Exception)
            {
                return new List<Contato>();
            }

        }
    }
}
