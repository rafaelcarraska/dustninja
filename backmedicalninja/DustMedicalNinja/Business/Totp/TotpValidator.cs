using System;
using System.Linq;

namespace DustMedicalNinja.Business.Totp
{
    public class TotpValidator
    {
        public TotpValidator()
        {
            
        }

        /// <summary>
        /// Validates a given TOTP. 
        /// </summary>
        /// <param name="accountSecretKey">User's secret key. Same as used to create the setup.</param>
        /// <param name="clientTotp">Number provided by the user which has to be validated.</param>
        /// <param name="timeToleranceInSeconds">Time tolerance in seconds. Default is 60 to acceppt 60 seconds before and after now.</param>
        /// <returns>True or False if the validation was successful.</returns>
        public bool Validate(string accountSecretKey, int clientTotp, int timeToleranceInSeconds = 60)
        {
            var codes = new TotpGenerator().GetValidTotps(accountSecretKey, TimeSpan.FromSeconds(timeToleranceInSeconds));
            return codes.Any(c => c == clientTotp);
        }
    }
}
