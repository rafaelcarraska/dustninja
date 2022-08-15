namespace DustMedicalNinja.Models.ViewModel
{
    public class TotpSetupViewModel
    {
        /// <summary>
        /// If the QR code can not be used, this code is needed to setup Google Authenticator. 
        /// </summary>
        public string ManualSetupKey { get; set; }

        /// <summary>
        /// Image string ready to be used in image tag without modification. 
        /// </summary>
        public string QrCodeImage { get; set; }
    }
}