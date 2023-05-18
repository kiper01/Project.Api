using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Models.Request;
using System.Collections.Generic;

namespace RegistryNPA.Core.Models.Dto
{
    public class DtoFormFiles<TDto>
    {
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();

        public IFormFile ProjectText { get; set; }

        [ModelBinder(BinderType = typeof(FormDataJsonBinder))]
        public TDto Data { get; set; }
    }
}
