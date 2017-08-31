using OCS.BusinessLayer.Models;
using System.Collections.Generic;

namespace OCS.BusinessLayer.Services
{
    public interface ICategoryServices
    {
        IEnumerable<CategoryModel> GetAll();
    }
}
