using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seterlund.CodeGuard;

namespace ApsDomain
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
