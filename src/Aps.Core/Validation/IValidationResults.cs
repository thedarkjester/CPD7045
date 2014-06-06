using System;
using System.Collections.Generic;
namespace Aps.Core.Validation
{
    interface IValidationResults
    {
        void AddResult(ValidationResult validationResult);

        List<ValidationResult> Results { get; set; }
    }
}
