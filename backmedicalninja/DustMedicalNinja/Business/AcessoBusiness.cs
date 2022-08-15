using DustMedicalNinja.DAO;
using DustMedicalNinja.Models;
using DustMedicalNinja.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DustMedicalNinja.Extensions;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace DustMedicalNinja.Business
{
    internal class AcessoBusiness : Uteis
    {
        Msg msg = new Msg();
        private readonly HttpContext _HttpContext;
        private string empresaId;

        internal AcessoBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
            empresaId = _HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sid).Select(c => c.Value).SingleOrDefault();
        }

        internal Acesso Input(string empresaId)
        {
            try
            {
                var acesso = new Acesso()
                {
                    empresaId = empresaId,
                    listaTela = InputTela()
                };

                return acesso;
            }
            catch (Exception)
            {
                return new Acesso();
            }
        }

        internal List<Tela> InputTela()
        {
            try
            {
                var _telas = Enum.GetNames(typeof(Telas));
                List<Tela> listTela = new List<Tela>();
                foreach (var item in _telas)
                {
                    listTela.Add(new Tela()
                    {
                        descricao = item,
                        master = (item == Telas.Empresa.ToString("g") || item == Telas.Perfil.ToString("g")) ? true : false,
                        permissao = Enum.GetNames(typeof(Permissoes)).ToList()
                    });
                }
                return listTela;
            }
            catch (Exception)
            {
                return new List<Tela>();
            }
        }

        internal Acesso Aleatorio()//add empresaid
        {
            var listaTela = new EmpresaBusiness(_HttpContext).ListAll().FirstOrDefault().listaTela;
            listaTela = ShuffleList(listaTela.ToList()).ToList();
            listaTela.ToList().ForEach(x => x.permissao.randomArray());

            return new Acesso(){
                empresaId = empresaId,
                listaTela = listaTela
            };
        }

        private List<E> ShuffleList<E>(List<E> inputList)
        {
            List<E> randomList = new List<E>();

            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList; //return the new random list
        }
    }
}
