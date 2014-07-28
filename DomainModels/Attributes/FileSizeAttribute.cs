using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Auction.DAL.Attributes
{
    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSize;

        public FileSizeAttribute(int maxSize)
        {
            _maxSize = maxSize;
        }

        public override bool IsValid(object value)
        {
            if (value == null) throw new ArgumentNullException("value");

            var httpPostedFileBase = value as HttpPostedFileBase;
            return httpPostedFileBase != null && httpPostedFileBase.ContentLength <= _maxSize;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("The file size should not exceed {0}", _maxSize);
        }
    }
}