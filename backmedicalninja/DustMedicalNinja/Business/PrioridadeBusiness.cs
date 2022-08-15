using DustMedicalNinja.DAO;
using DustMedicalNinja.Models;
using DustMedicalNinja.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DustMedicalNinja.Extensions;

namespace DustMedicalNinja.Business
{
    internal class PrioridadeBusiness : Uteis
    {
        Msg msg = new Msg();

        internal Prioridade Input()
        {
            try
            {
                return new Prioridade()
                {
                    rotina = new Random().Next(1, 120),                    
                    urgencia = new Random().Next(1, 24),
                    critico = new Random().Next(1, 5),
                };
            }
            catch (Exception)
            {
                return new Prioridade();
            }
        }
    }
}
