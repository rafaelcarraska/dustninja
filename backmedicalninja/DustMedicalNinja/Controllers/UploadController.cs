using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DustMedicalNinja.Business;
using DustMedicalNinja.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DustMedicalNinja.Controllers
{
    [ApiController]
    public class UploadController : Controller
    {
        private IHostingEnvironment hostingEnvironment;

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("/[controller]/[action]")]
        [AllowAnonymous]
        [DisableRequestSizeLimit]
        public ActionResult Assinatura()
        {
            //var files = Request.Form.Files; // now you have them

            try
            {
                var file = Request.Form.Files[0];
                var fileName = file.Name+DateTime.Now.ToString("yyyyMMddhhmmss");
                //string folderName = "src\\assets\\images\\upload\\assinaturas";
                //string webRootPath = hostingEnvironment.WebRootPath
                //    .Replace("BackMedicalNinja\\DustMedicalNinja\\wwwroot", "FrontMedicalNinja");

                string folderName = "/images/signature/";
                string webRootPath = "";


                string newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string extensao = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    extensao = extensao.Substring(extensao.Length - 4);
                    fileName = fileName + extensao;
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }                

                new EventoBusiness(HttpContext).Sucesso(Telas.Usuario, "", fileName, file.Name, "Upload", "");
                Msg msg = new Msg() {
                    erro = new List<string>(),
                    id = fileName
                };
                return Json(msg);
            }
            catch (System.Exception ex)
            {
                Msg msg = new Msg()
                {
                    erro = new List<string>() { "Falha ao realizar o upload."},
                    id = string.Empty
                };
                new EventoBusiness(HttpContext).Erro(ex.Message, Telas.Usuario, string.Empty, "Upload", "Erro ao realizar o upload da assinatura.");
                return Json(msg);
            }
        }

        [HttpPost("/[controller]/[action]")]
        [AllowAnonymous]
        [DisableRequestSizeLimit]
        public ActionResult Anexo()
        {
            try
            {
                var file = Request.Form.Files[0];
                var fileName = string.Empty;
                var extensao = string.Empty;

                string folderName = "/images/files/";
                string webRootPath = "";

                string newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    fileName =  Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "") + DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(file.FileName);
                    extensao = Path.GetExtension(file.FileName);
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    Anexo anexo = new Anexo(){
                        fileDCMId = file.Name,
                        arquivo = fileName,
                        size = file.Length,
                        descricao = file.FileName.Trim(),
                        extensao = extensao
                    };

                    new AnexosBusiness(HttpContext).Salvar(anexo);

                    new EventoBusiness(HttpContext).Sucesso(Telas.Worklist, "", fileName, anexo.fileDCMId, "Upload", "");
                }
               
                Msg msg = new Msg()
                {
                    erro = new List<string>(),
                    id = file.Name
                };
                return Json(msg);
            }
            catch (System.Exception ex)
            {
                Msg msg = new Msg()
                {
                    erro = new List<string>() { "Falha ao realizar o upload." },
                    id = string.Empty
                };
                new EventoBusiness(HttpContext).Erro(ex.Message, Telas.Usuario, string.Empty, "Upload", "Erro ao realizar o upload da assinatura.");
                return Json(msg);
            }
        }


        [HttpPost("/[controller]/[action]")]
        [AllowAnonymous]
        [DisableRequestSizeLimit]
        public ActionResult AnexoTemplate()
        {
            try
            {
                var file = Request.Form.Files[0];
                var fileName = string.Empty;
                var extensao = string.Empty;

                string folderName = "/images/files/";
                string webRootPath = "";

                string newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    fileName = Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "") + DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(file.FileName);
                    extensao = Path.GetExtension(file.FileName);
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    AnexoTemplate anexoTemplate = new AnexoTemplate()
                    {
                        arquivo = fileName,
                        size = file.Length,
                        descricao = file.FileName.Trim(),
                        extensao = extensao
                    };

                    new AnexoTemplatesBusiness(HttpContext).Salvar(anexoTemplate);

                    new EventoBusiness(HttpContext).Sucesso(Telas.Worklist, "", fileName, string.Empty, "Upload anexo template", "");
                }

                Msg msg = new Msg()
                {
                    erro = new List<string>(),
                    id = file.Name
                };
                return Json(msg);
            }
            catch (System.Exception ex)
            {
                Msg msg = new Msg()
                {
                    erro = new List<string>() { "Falha ao realizar o upload." },
                    id = string.Empty
                };
                new EventoBusiness(HttpContext).Erro(ex.Message, Telas.TemplateImpressao, string.Empty, "Upload", "Erro ao realizar o upload.");
                return Json(msg);
            }
        }
    }
}