using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aps.Core.Validation
{
    public class ValidationResults : IValidationResults
    {
        public List<ValidationResult> Results { get; set; }

        public bool IsValid { get; set; }

        public int Count { get; set; }

        public void AddResult(ValidationResult validationResult)
        {
            Results.Add(validationResult);
        }
    }
}
