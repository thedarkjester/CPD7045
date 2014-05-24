using System;

namespace Aps.DomainBase
{
    public interface IIdentifiable
    {
        Guid Id { get; }
    }
}