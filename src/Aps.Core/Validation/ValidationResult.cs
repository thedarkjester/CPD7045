using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aps.Core.Validation
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }

        public string ErrorDescription { get; set; }

        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the data pair that was validated.
        /// </summary>
        /// <value>
        /// The data pair.
        /// </value>
        public DataPair DataPair { get; set; }
    }
}
