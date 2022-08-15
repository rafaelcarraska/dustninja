using DustMedicalNinja.Business;
using DustMedicalNinja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustMedicalNinja.Extensions
{
    public static class NumberExtensions
    {
        public static string longTobytes(this long data)
        {
            try
            {
                switch (data/1024)
                {
                    case var n when (n < 1):
                        return string.Format("{0} b", data);
                    case var n when (n >= 1 && n < 1024 ):
                        return string.Format("{0} kb", n);
                    case var n when (n >= 1024 ):
                        return string.Format("{0} mb", n/1024);
                    default:
                        break;
                }

                return data.ToString(); 
            }
            catch (Exception)
            {
                return data.ToString()?? "0";
            }
        }

        public static string doubleToTime(this Double data)
        {
            try
            {
                return (data < 0 ? "-":"") + TimeSpan.FromHours(data).ToString("h\\:mm");
            }
            catch (Exception)
            {
                return data.ToString() ?? "0";
            }
        }
    }
}
