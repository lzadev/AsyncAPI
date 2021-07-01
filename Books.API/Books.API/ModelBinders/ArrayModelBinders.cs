using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Books.API.ModelBinders
{
    public class ArrayModelBinders : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            //Este model binder solo trabaja con enumerable types
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();

                return Task.CompletedTask;
            }

            //Oteniendo el valor enviado
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();

            //Si el valor es nulo o con espacios en blanco retorno nulo
            if (string.IsNullOrWhiteSpace(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);

                return Task.CompletedTask;
            }

            //Si el valor no es nulo o con espacios en blanco retorno nulo
            //Y el tipo del modelo es enumerable
            //Obtengo el tipo enumerable  y un convertidor
            var elementType = bindingContext.ModelType.GetTypeInfo()
                                .GenericTypeArguments[0];

            var converter = TypeDescriptor.GetConverter(elementType);

            //Convirtiendlo cada valor de la lista al tipo enumerable
            var values = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => converter.ConvertFromString(x.Trim()))
                            .ToArray();

            //Creando un array del tipo Guid y asignandolo a valor de Model
            var typedValues = Array.CreateInstance(elementType, values.Length);
            values.CopyTo(typedValues, 0);
            bindingContext.Model = typedValues;

            //Retornando el resulado exitoso y pasandolo al Model
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;

        }
    }
}
