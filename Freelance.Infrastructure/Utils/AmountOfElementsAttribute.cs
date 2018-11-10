using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Infrastructure.Utils
{
    public class AmountOfElementsAttribute : ValidationAttribute
    {
        private readonly int _maxAmountOfElements;
        public AmountOfElementsAttribute(int maxAmountOfElements)
        {
            _maxAmountOfElements = maxAmountOfElements;
        }

        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count <= _maxAmountOfElements;
            }
            return false;
        }
    }
}
