using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ExcaliburCodingAssignment.Models
{
    public class OrderCombinedParamModel : IValidatableObject
    {
        public string descParam { get; set; }

        public string fromDateParam { get; set; }

        public string toDateParam { get; set; }

        public string fromAmountParam { get; set; }

        public string toAmountParam { get; set; }

        public Double? fromAmountVal { get; set; }

        public Double? toAmountVal { get; set; }

        public DateTime fromDateVal { get; set; }
        
        public DateTime toDateVal { get; set; }

        public void ParseParams(OrderCombinedParamModel model)
        {
            Double fromAmountParsedVal, toAmountParsedVal;
            DateTime fromDateVal1, toDateVal1;

            if (Double.TryParse(model.fromAmountParam, out fromAmountParsedVal))
            {
                model.fromAmountVal = fromAmountParsedVal;
            }
            if (Double.TryParse(model.toAmountParam, out toAmountParsedVal))
            {
                model.toAmountVal = toAmountParsedVal;
            }

            if (DateTime.TryParse(model.fromDateParam, out fromDateVal1))
            {
                model.fromDateVal = fromDateVal1;
            }

            if (DateTime.TryParse(model.toDateParam, out toDateVal1))
            {
                model.toDateVal = toDateVal1;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ParseParams(this);

            if (!String.IsNullOrEmpty(fromDateParam) && !String.IsNullOrEmpty(toDateParam))
            {
              
                if (this.toDateVal < this.fromDateVal)
                {
                    yield return new ValidationResult("To Date must be greater than the From Date. Check your values.");
                }
            }

            if (!String.IsNullOrEmpty(fromAmountParam) && !String.IsNullOrEmpty(toAmountParam))
            {

                if (this.toAmountVal < this.fromAmountVal)
                {
                    yield return new ValidationResult("To Amount must be greater than the From Amount. Check your values.");
                }
            }
        }
    }
}
