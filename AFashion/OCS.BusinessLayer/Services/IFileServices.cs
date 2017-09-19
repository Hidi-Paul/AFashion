using System.Web;

namespace OCS.BusinessLayer.Services
{
    public interface IFileServices
    {
        //Post file on server
        //Returns file url
        string SaveFile(byte[] postedFile, string fileName);

        //Delete file by filename
        void DeleteFile(string filePath);
    }
}
