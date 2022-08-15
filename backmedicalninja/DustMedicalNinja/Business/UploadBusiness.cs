using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustMedicalNinja.Business
{
    internal class UploadBusiness
    {
        private readonly HttpContext _HttpContext;

        internal UploadBusiness(HttpContext httpContext)
        {
            _HttpContext = httpContext;
        }
    }
}
