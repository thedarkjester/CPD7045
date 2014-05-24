using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Aps.DomainBase
{
    public abstract class Entity : EntityBase
    {
        protected Entity()
        {
        }
    }

    public abstract class EntityBase : IIdentifiable
    {
        protected EntityBase()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
    }

    public interface IIdentifiable
    {
        Guid Id { get; }
    }
}
