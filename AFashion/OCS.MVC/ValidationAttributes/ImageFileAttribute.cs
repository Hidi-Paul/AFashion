using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OCS.MVC.ValidationAttributes
{
    public class ImageFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string[] AllowedExtensions = new string[]
            {
                ".jpg",
                ".png"
            };

            var file = value as HttpPostedFileBase;

            if (file == null)
            {
                ErrorMessage = "Please add an image";
                return false;
            }
            var extension = file.FileName.Substring(file.FileName.LastIndexOf("."));
            if (!AllowedExtensions.Contains(extension))
            {
                ErrorMessage = "Invalid extension: " + extension + "" +
                    "\n Valid Formats: " + string.Join(", ", AllowedExtensions);
                return false;
            }

            return true;
        }
    }
}