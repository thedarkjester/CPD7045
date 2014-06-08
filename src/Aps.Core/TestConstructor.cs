using System;
using Seterlund.CodeGuard;

namespace Aps.Scheduling.ApplicationService
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
