using Autofac;
using Autofac.Builder;
using Autofac.Features.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aps.Scheduling.ApplicationService.Extensions
{
    public static class AutofacExtensions
    {
        private const string OrderString = "WithOrderTag";
        private static int OrderCounter;

        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> WithOrder<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registrationBuilder)
        {
            return registrationBuilder.WithMetadata(OrderString, Interlocked.Increment(ref OrderCounter));
        }

        public static IEnumerable<TComponent> ResolveOrdered<TComponent>(this IComponentContext context)
        {
            return from m in context.Resolve<IEnumerable<Meta<TComponent>>>()
                   orderby m.Metadata[OrderString]
                   select m.Value;
        }
    }
}
