using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Freelance.Core.Models;

namespace Freelance.Infrastructure
{
    public class AvailabilityModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (bindingContext.ModelMetadata.PropertyName == "Availability" && value != null)
            {
                var rawValues = value.RawValue as string[];
                if (rawValues != null)
                {
                    int flagValue = 0;
                    foreach (var val in rawValues)
                    {
                        int currentValue = (int) Enum.Parse(typeof(Availability), val);
                        flagValue |= currentValue;
                    }
                    return Enum.ToObject(typeof(Availability), flagValue);
                }
                // In case it is a single value
                if (value.GetType().IsEnum)
                {
                    return Enum.ToObject(typeof(Availability), value);
                }
            }
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}