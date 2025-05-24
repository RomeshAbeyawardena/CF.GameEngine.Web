using FluentValidation;
using IDFCR.Shared.Exceptions;
using System.Text;

namespace IDFCR.Shared.FluentValidation;

public class ValidationDisplayException(ValidationException validationException) : Exception(validationException.Message, validationException), IExposableException
{

    string IExposableException.Details { 
        get
        {
            var stringBuilder = new StringBuilder();

            foreach(var error in validationException.Errors)
            {
                stringBuilder.AppendLine($"Code: {error.ErrorCode}\t{error.PropertyName}: {error.ErrorMessage}. (Actual hour: {error.AttemptedValue})");
            }

            return stringBuilder.ToString();
        } 
    }
}
