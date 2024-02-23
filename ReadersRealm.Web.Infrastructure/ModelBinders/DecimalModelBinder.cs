namespace ReadersRealm.Web.Infrastructure.ModelBinders;

using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class DecimalModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext? bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        ValueProviderResult valueProviderResult = bindingContext
            .ValueProvider
            .GetValue(bindingContext.ModelName);

        if (valueProviderResult != ValueProviderResult.None && 
            !string.IsNullOrWhiteSpace(valueProviderResult.FirstValue))
        {
            decimal parsedValue = 0M;
            bool bindSucceeded = false;

            string currentCultureDecimalSeparator = CultureInfo
                .CurrentCulture
                .NumberFormat
                .NumberDecimalSeparator;

            try
            {
                string decimalValue = valueProviderResult.FirstValue;

                decimalValue = decimalValue.Replace(",",
                    currentCultureDecimalSeparator);
                decimalValue = decimalValue.Replace(".",
                    currentCultureDecimalSeparator);

                parsedValue = Convert.ToDecimal(decimalValue);
                bindSucceeded = true;
            }
            catch (FormatException fe)
            {
                bindingContext
                    .ModelState
                    .AddModelError(bindingContext.ModelName, 
                        fe, 
                        bindingContext.ModelMetadata);
            }


            if (bindSucceeded)
            {
                bindingContext.Result = ModelBindingResult.Success(parsedValue);
            }
        }

        return Task.CompletedTask;
    }
}