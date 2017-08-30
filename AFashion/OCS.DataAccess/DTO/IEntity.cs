using System;

namespace OCS.DataAccess.DTO
{
    public interface IEntity
    {
        Guid ID { get; set; }
        String Name { get; set; }
    }
}
