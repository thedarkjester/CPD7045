using System;
using Aps.BillingCompanies;
using Aps.IntegrationEvents;
using Autofac;
using Caliburn.Micro;
using Aps.Customer;

namespace Aps.Core
{
    class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            // construct the dependency injection builder that when built, will structure and "know"
            // the dependency hierarchy/chains

            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<EventIntegrationService>().As<EventIntegrationService>().SingleInstance();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<CustomerRepository>().As<CustomerRepository>().InstancePerDependency();
            builder.RegisterType<BillingCompanyRepository>().As<BillingCompanyRepository>().InstancePerDependency();
            builder.RegisterType<SchedulingEngine>().As<SchedulingEngine>().InstancePerDependency();

            Container = builder.Build();

            var schedulingEngine = Container.Resolve<SchedulingEngine>();
            schedulingEngine.Start();

            Console.ReadLine();
        }
    }
}