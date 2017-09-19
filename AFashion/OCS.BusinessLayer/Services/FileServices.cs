using System.Configuration;
using System.IO;
using System.Web.Hosting;

namespace OCS.BusinessLayer.Services
{
    public class FileServices : IFileServices
    {
        private static string StaticFilePath => ConfigurationManager.AppSettings["StaticFilePath"];

        public FileServices()
        {
            bool folderExists = Directory.Exists(HostingEnvironment.MapPath(StaticFilePath));
            if (!folderExists)
            {
                Directory.CreateDirectory(HostingEnvironment.MapPath(StaticFilePath));
            }
        }


        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        public string SaveFile(byte[] postedFile, string fileName)
        {
            string path = Path.Combine(HostingEnvironment.MapPath(StaticFilePath), fileName);

            File.WriteAllBytes(path, postedFile);

            return fileName;
        }
    }
}
