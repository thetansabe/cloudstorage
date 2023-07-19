using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Service
{
    public static class FileEntryHandleExtension
    {
        private readonly static string[] VIOLATED_CHARS = 
            { "\\", "/", ":", "*", "?", "\"", "<", ">", "|" };

        public static string ValidateFileName(this string fileName)
        {
            if (fileName.IsNullOrEmpty())
                return "File name cannot empty";

            
            bool isViolate = VIOLATED_CHARS.Any(c => fileName.Contains(c));

            if (isViolate)
                return "File name cannot contain special characters";

            return string.Empty;
        }

        public static string HandleFileExtension(this string fileExtension, string fileName)
        {
            if (fileExtension.IsNullOrEmpty())
                fileExtension = Path.GetExtension(fileName);

            return fileExtension;
        }
    }
}
