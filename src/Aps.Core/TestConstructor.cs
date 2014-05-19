using System;
using Seterlund.CodeGuard;

namespace Aps.Core
{
    public class TestConstructor
    {
        public TestConstructor(Guid id)
        {
            Guard.That(id).IsNotEmpty();

            if (id == Guid.Empty)
            {
                throw new ArgumentException();
            }
        }
    }
}
