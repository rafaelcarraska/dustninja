using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DustMedicalNinja.Components;
using DustMedicalNinja.DAO;
using DustMedicalNinja.Models;
using DustMedicalNinja.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using DustMedicalNinja.Business;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using DustMedicalNinja.Extensions;
//using Amazon.CognitoIdentity;
//using Amazon;
//using Amazon.CognitoIdentityProvider;
//using Amazon.Extensions.CognitoAuthentication;
//using System.Numerics;
//using Amazon.CognitoIdentityProvider.Model;
using DustMedicalNinja.Models.ViewModel;
using DustMedicalNinja.Business.Totp;

namespace DustMedicalNinja.Business
{
    internal class AutenticacaoBusiness : Uteis
    {
        UsuarioDao _UsuarioDao = new UsuarioDao();
        Msg msg = new Msg();
        SelecionaEmpresa selecionaEmpresa = new SelecionaEmpresa();

        private readonly HttpContext _HttpContext;

        internal AutenticacaoBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
        }

        internal AutenticacaoBusiness()
        {
        }

        internal SelecionaEmpresa SelecionaEmpresa(Autenticacao autenticacao)
        {
            selecionaEmpresa = new SelecionaEmpresa();
            autenticacao.senha = getMD5Hash(autenticacao.senha);

            var usuario = _UsuarioDao.Autenticacao(autenticacao).Result;
            if (usuario != null)
            {
                selecionaEmpresa.twofactor = usuario.twofactor;
                if (!usuario.status)
                {
                    selecionaEmpresa.erro = "Esse usuário está inativo";
                    return selecionaEmpresa;
                }
                if (usuario.master)
                {
                    selecionaEmpresa.comboBox = new EmpresaBusiness(_HttpContext).ListaCombo();
                    return selecionaEmpresa;
                }
                else if (usuario.listaEmpresa.Count > 0)
                {
                    selecionaEmpresa.comboBox = new EmpresaBusiness(_HttpContext).listaComboLogin(usuario.listaEmpresa);
                    return selecionaEmpresa;
                }
                else
                {
                    selecionaEmpresa.erro = "Esse usuário não possuí empresas";
                    return selecionaEmpresa;
                }
            }

            selecionaEmpresa.erro = "Usuário ou senha invalida!";
            return selecionaEmpresa;
        }

        internal Msg validaLogin(Autenticacao autenticacao)
        {
            msg = new Msg();

            autenticacao.senha = getMD5Hash(autenticacao.senha);

            var usuario = _UsuarioDao.Autenticacao(autenticacao).Result;
            if (usuario != null)
            {
                if (usuario.twofactor)
                {
                    // To validate the pin after user input (where pin is an int variable)
                    bool isCorrectPIN = new TotpValidator().Validate(usuario.key, autenticacao.pin);

                    if (!isCorrectPIN)
                    {
                        msg.erro.Add("Chave invalida!");
                        return msg;
                    }
                }

                List<string> roles = new PerfilBusiness(_HttpContext).Roles(usuario.perfilId, usuario.master);

                //GetCredsChallengesAsync();

                msg.id = CreateJwt(autenticacao, roles, usuario.Id, usuario.master);
                new EventoBusiness(_HttpContext).Login(autenticacao);
                return msg;
            }

            msg.erro.Add("Usuário ou senha invalida!");
            return msg;
        }

        //public async void GetCredsChallengesAsync()
        //{
        //    var credentials = new CognitoAWSCredentials(
        //           "us-east-2:f726695e-fc86-487b-982c-3dea2bfff0c9", // Identity pool ID
        //           RegionEndpoint.USEast2 // Region
        //       );

        //    var provider = new AmazonCognitoIdentityProviderClient(credentials, RegionEndpoint.USEast2);
        //    var userPool = new CognitoUserPool("us-east-2_gEmUbJIW9", "20auvjm3sdqoqbut13i4ficfjl", provider, "130ggiap0cei9o2pnagag2fe9luu9hgfjnbv4tfbe35ji17pg092");
        //    var user = new CognitoUser("rafaelcarraska@hotmail.com", "20auvjm3sdqoqbut13i4ficfjl", userPool, provider);
        //    var authRequest = new InitiateSrpAuthRequest()
        //    {
        //        Password = "teste"

        //    };

        //    var authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);

        //    while (authResponse.AuthenticationResult == null)
        //    {
        //        if (authResponse.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
        //        {
        //            Console.WriteLine("Enter your desired new password:");
        //            string newPassword = Console.ReadLine();

        //            authResponse = await user.RespondToNewPasswordRequiredAsync(new RespondToNewPasswordRequiredRequest()
        //            {
        //                SessionID = authResponse.SessionID,
        //                NewPassword = newPassword
        //            });
        //            accessToken = authResponse.AuthenticationResult.AccessToken;
        //        }
        //        else if (authResponse.ChallengeName == ChallengeNameType.SMS_MFA)
        //        {
        //            Console.WriteLine("Enter the MFA Code sent to your device:");
        //            string mfaCode = Console.ReadLine();

        //            var mfaResponse = await user.RespondToSmsMfaAuthAsync(new RespondToSmsMfaRequest()
        //            {
        //                SessionID = authResponse.SessionID,
        //                MfaCode = mfaCode

        //            }).ConfigureAwait(false);
        //            accessToken = authResponse.AuthenticationResult.AccessToken;
        //        }
        //        else
        //        {
        //            Console.WriteLine("Unrecognized authentication challenge.");
        //            accessToken = "";
        //            break;
        //        }
        //    }

        //    if (authResponse.AuthenticationResult != null)
        //    {
        //        Console.WriteLine("User successfully authenticated.");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Error in authentication process.");
        //    }

        //}

        internal TotpSetupViewModel ListaTwoFactor(string usuarioId)
        {
            var usuario = new UsuarioBusiness(_HttpContext).List(usuarioId);

            var totpSetupViewModel = new TotpSetupViewModel();

            if (usuario.twofactor)
            {
                // To generate the qrcode/setup key

                var totpSetupGenerator = new TotpSetupGenerator();
                var totpSetup = totpSetupGenerator.Generate(usuario.login, usuario.email, usuario.key, 300, 300);

                totpSetupViewModel.QrCodeImage = totpSetup.QrCodeImage;
                totpSetupViewModel.ManualSetupKey = totpSetup.ManualSetupKey;
            }
            //// To validate the pin after user input (where pin is an int variable)

            //var totp = this.totpGenerator.Generate("7FF3F52B-2BE1-41DF-80DE-04D32171F8A3");
            //var valid = this.totpValidator.Validate("7FF3F52B-2BE1-41DF-80DE-04D32171F8A3", totp, 60);

            return totpSetupViewModel;
        }

        internal bool ValidaTwoFactor(ValidaTwoFactorViewModel validaTwoFactorViewModel)
        {
            var usuario = new UsuarioBusiness(_HttpContext).List(validaTwoFactorViewModel.usuarioId);
            if (usuario.twofactor)
            {
                // To validate the pin after user input (where pin is an int variable)
                bool isCorrectPIN = new TotpValidator().Validate(usuario.key, validaTwoFactorViewModel.pin, 60);

                if (!isCorrectPIN)
                {
                    return true;
                }
            }
            return false;
        }

        internal DadosBasicos DadosBasicos()
        {
            DadosBasicos dadosBasicos = new DadosBasicos();

            var usuario = new UsuarioBusiness(_HttpContext).ListUsuarioLogado();
            if (usuario != null)
            {
                List<string> roles = new PerfilBusiness(_HttpContext).Roles(usuario.perfilId, usuario.master);

                dadosBasicos.usuarioNome = usuario.nome;
                dadosBasicos.usuarioId = usuario.Id;
                dadosBasicos.master = usuario.master;
                dadosBasicos.roles = roles;
                dadosBasicos.picture = "assets/images/nick.png";
                dadosBasicos.empresaLogada = new EmpresaBusiness(_HttpContext).ListaEmpresaLogada().nomeFantasia;
            }

            return dadosBasicos;
        }

        private string CreateJwt(Autenticacao autenticacao, List<string> roles, string usuarioId, bool master)
        {
            var sub = autenticacao.login;
            var company = autenticacao.empresaId;

            return new JsonWebToken().Encode(sub, roles, company, usuarioId, master);
        }

        internal string getMD5Hash(string input)
        {
            if (input.Length > 0)
            {
                int chave = geraasc(input);
                decimal temp = 9941 / (chave / ((decimal)Math.Pow(10, Convert.ToDouble(chave.ToString().Length)))) * 10000;
                input += Math.Round(temp, 0).ToString();
            }
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(Encrypt(input, input.Substring(4)).Substring(2));
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private static string Encrypt(string toEncrypt, string key)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length).Substring(3);
        }

        private int geraasc(string val)
        {
            byte[] ASCIIValues = Encoding.ASCII.GetBytes(val);
            int temp = 0;
            int value = 0;
            foreach (byte b in ASCIIValues)
            {
                temp++;
                value += (Convert.ToInt32(b) + 7) * temp;
            }
            return value;
        }
    }
}
