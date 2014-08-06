using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Auction.Web.Attributes
{
    public class FileTypesAttribute : ValidationAttribute
    {
        private readonly List<string> _types;

        public FileTypesAttribute(string types)
        {
            _types = types.Split(',').ToList();
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            var httpPostedFileBase = value as HttpPostedFileBase;
            if (httpPostedFileBase != null)
            {
                var extension = System.IO.Path.GetExtension(httpPostedFileBase.FileName);
                if (extension != null)
                {
                    var fileExt = extension.Substring(1);
                    return _types.Contains(fileExt, StringComparer.OrdinalIgnoreCase);
                }
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            if (name == null)
            {
                return string.Format("Invalid file type. Only the following types {0} are supported.",
                    String.Join(", ", _types));
            }
            return string.Format("Invalid file type. Only the following types {0} are supported.; {1}",
                String.Join(", ", _types), name);
        }
    }
}