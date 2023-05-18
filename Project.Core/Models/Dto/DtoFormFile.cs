using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Models.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Core.Models.Dto
{
    public class DtoFormFile<TDto>
    {
        public IFormFile File { get; set; }

        [ModelBinder(BinderType = typeof(FormDataJsonBinder))]
        public TDto Data { get; set; }
    }
}
