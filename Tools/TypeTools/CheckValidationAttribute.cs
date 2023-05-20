using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace SRLCore.Tools
{
    public partial class TypeTools
    {
        public static List<string> CheckValidationAttribute(object o, List<string> ignore_fields = null)
        {
            List<string> errors = new List<string>();
            IEnumerable<PropertyDescriptor> prop_des = TypeDescriptor.GetProperties(o.GetType()).Cast<PropertyDescriptor>();
            var validations =
                prop_des.SelectMany(pd => pd.Attributes.OfType<ValidationAttribute>().Where(va => !va.IsValid(pd.GetValue(o))),
                (field, attr) => new
                {
                    field.Name,
                    field.DisplayName,
                    attr.ErrorMessage,
                    attr.TypeId,
                    Minimum = attr.GetType().GetProperty("Minimum")?.GetValue(attr) ?? null,
                    Maximum = attr.GetType().GetProperty("Maximum")?.GetValue(attr) ?? null
                }).ToList();
            foreach (var item in validations)
            {
                if (ignore_fields != null)
                {
                    if (ignore_fields.Contains(item.Name)) continue;
                }
                string error = item.ErrorMessage;
                string attr_name = (item as dynamic).TypeId.Name;
                if (error != null)
                    while (error.Contains("{") && !string.IsNullOrWhiteSpace(error))
                    {
                        error = error.Replace("{0}", item.DisplayName);
                        switch (attr_name)
                        {
                            case nameof(RangeAttribute):
                                error = error.Replace("{1}", item.Minimum.ToString());
                                error = error.Replace("{2}", item.Maximum.ToString());
                                break;
                        }
                        if (error == null) break;
                    }
                if (error != null)
                    errors.Add(error);
            }
            return errors;
        }
    }
}
