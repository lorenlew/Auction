using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Auction.Domain.Attributes
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
            if (value == null)
            {
                return false;
            }
            var httpPostedFileBase = value as HttpPostedFileBase;
            if (httpPostedFileBase != null)
            {
                return httpPostedFileBase.ContentLength <= _maxSize;  
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            if (name == null)
            {
                return string.Format("The file size should not exceed {0}", _maxSize);
            }
                return string.Format("The file size should not exceed {0}; {1}", _maxSize, name);
        }
    }
}