using OCS.BusinessLayer.Models;
using System.Collections.Generic;

namespace OCS.BusinessLayer.Services
{
    public interface IBrandServices
    {
        IEnumerable<BrandModel> GetAll();
    }
}
