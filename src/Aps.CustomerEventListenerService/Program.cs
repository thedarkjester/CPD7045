using Aps.Customers;
using Aps.Fakes;
using Aps.Integration;
using Aps.Integration.Queries.Events;
using Aps.Integration.Serialization;
using Autofac;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aps.CustomerEventListenerService
{
    class Program
    {
        private static IContainer Container { get; set; }
        private static CancellationToken cancellationToken;
        static void Main(string[] args)
        {
            // construct the dependency injection builder that when built, will structure and "know"
            // the dependency hierarchy/chains
            RegisterAllDependencies();

            StartMainApplication();

            Console.ReadLine();
        }

        private static void RegisterAllDependencies()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<CustomerRepositoryFake>().As<ICustomerRepository>().InstancePerDependency();
            builder.RegisterType<EventIntegrationService>().As<EventIntegrationService>().SingleInstance();
            builder.RegisterType<BinaryEventSerializer>().As<BinaryEventSerializer>().InstancePerDependency();
            builder.RegisterType<BinaryEventDeSerializer>().As<BinaryEventDeSerializer>().InstancePerDependency();
            builder.RegisterType<EventIntegrationRepositoryFake>().As<EventIntegrationRepositoryFake>().InstancePerDependency();
            builder.RegisterType<CustomerCreator>().As<CustomerCreator>().InstancePerDependency();
            builder.RegisterType<GetLatestEventsBySubScribedEventTypeQuery>()
                   .As<GetLatestEventsBySubScribedEventTypeQuery>()
                   .InstancePerDependency();
            builder.RegisterType<CustomerService>().As<CustomerService>().SingleInstance();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();


            Container = builder.Build();
        }



        private static void StartMainApplication()
        {
            cancellationToken = new CancellationToken();

            Task.Factory.StartNew(() =>
            {
                var customerService = Container.Resolve<CustomerService>();
                customerService.Start(cancellationToken);

            }, cancellationToken);
        }


    }
}
