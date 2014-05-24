using System;
using Aps.BillingCompanies;
using Autofac;
using Autofac.Core;
using Caliburn.Micro;
using Aps.Customer;

namespace Aps.Core
{
    class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            // Usually you're only interested in exposing the type
            // via its interface:
            builder.RegisterType<EventAggregator>().As<IEventAggregator>();
            builder.RegisterType<CustomerRepository>().As<CustomerRepository>();
            builder.RegisterType<BillingCompanyRepository>().As<BillingCompanyRepository>();
            builder.RegisterType<SchedulingEngine>().As<SchedulingEngine>();
            
            Container = builder.Build();

            var customer = Container.Resolve<CustomerRepository>().GetNewCustomer();

            Console.ReadLine();
        }
        }
}