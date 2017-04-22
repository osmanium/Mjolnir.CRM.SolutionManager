using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mjolnir.CRM.SolutionManager.Utils
{
    public static class FileUtilities
    {
        public static string ReadFileContentInBase64(string filePath)
        {
            string fileContent = null;

            using (StreamReader sr = new StreamReader(filePath))
            {
                fileContent = sr.ReadToEnd();
            }

            var bytes = Encoding.UTF8.GetBytes(fileContent);
            return Convert.ToBase64String(bytes);
        }

        public static string ConvertContentToBase64(string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            return Convert.ToBase64String(bytes);
        }
    }
}
