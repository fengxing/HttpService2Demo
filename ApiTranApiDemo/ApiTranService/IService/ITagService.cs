using SmartBaseEntity;
using System;

namespace ApiTranService
{
    public interface ITagService
    {
        IHttpTag GetTag();

        Guid UID { get; }

        string RequestID { get; }
    }
}
