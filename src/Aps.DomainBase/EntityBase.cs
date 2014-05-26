﻿using System;

namespace Aps.DomainBase
{
    public abstract class EntityBase : IIdentifiable
    {
        protected EntityBase()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
    }
}