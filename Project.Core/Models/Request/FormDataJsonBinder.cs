using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Project.Core.Models.Request
{
    public class FormDataJsonBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }
            var fieldName = bindingContext.FieldName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(fieldName);
            var value = string.Empty;
            if (valueProviderResult == ValueProviderResult.None)
            {
                if (!TryReadDataFile(bindingContext, out value))
                    return Task.CompletedTask;
            }
            else
            {
                bindingContext.ModelState.SetModelValue(fieldName, valueProviderResult);
                value = valueProviderResult.FirstValue;
            }

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            try
            {
                var result = JsonConvert.DeserializeObject(value, bindingContext.ModelType);
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            catch (JsonException)
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;
        }

        private bool TryReadDataFile(
            ModelBindingContext bindingContext,
            out string value)
        {
            var dataFile =
                bindingContext.ActionContext.HttpContext.Request.Form.Files.GetFile(bindingContext.FieldName);

            if (dataFile == null)
            {
                value = string.Empty;
                return false;
            }

            using (var stream = dataFile.OpenReadStream())
            using (var reader = new StreamReader(stream))
            {
                value = reader.ReadToEnd();
            }

            return true;
        }
    }
}
