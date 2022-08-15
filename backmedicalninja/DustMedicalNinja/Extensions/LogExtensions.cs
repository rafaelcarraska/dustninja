using DustMedicalNinja.Business;
using DustMedicalNinja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustMedicalNinja.Extensions
{
    public static class LogExtensions
    {
        public static Log InsertLog(this Log log, string usuarioId)
        {
            if (log == null)
            {
                log = new Log();
            }

            log.insertUsuarioId = usuarioId;
            log.insertData = DateTime.Now;
            log.updateUsuarioId = usuarioId;
            log.updateData = DateTime.Now;

            return log;
        }

        public static Log UpdateLog(this Log log, string usuarioId)
        {
            if (log == null)
            {
                return log.InsertLog(usuarioId);
            }

            log.updateUsuarioId = usuarioId;
            log.updateData = DateTime.Now;

            return log;
        }
    }
}
