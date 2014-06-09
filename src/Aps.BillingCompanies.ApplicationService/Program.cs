using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aps.Fakes;
using Aps.Integration;
using Aps.Integration.Queries.Events;
using Aps.Integration.Serialization;
using Autofac;
using Caliburn.Micro;

namespace Aps.BillingCompanies.ApplicationService
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

            builder.RegisterType<EventIntegrationService>().As<EventIntegrationService>().SingleInstance();
            builder.RegisterType<BinaryEventSerializer>().As<BinaryEventSerializer>().InstancePerDependency();
            builder.RegisterType<BinaryEventDeSerializer>().As<BinaryEventDeSerializer>().InstancePerDependency();
            builder.RegisterType<BillingCompanyRepositoryFake>().As<IBillingCompanyRepository>().InstancePerDependency();
            builder.RegisterType<EventIntegrationRepositoryFake>().As<IEventIntegrationRepository>().InstancePerDependency();
            builder.RegisterType<GetLatestEventsBySubScribedEventTypeQuery>()
                   .As<GetLatestEventsBySubScribedEventTypeQuery>()
                   .InstancePerDependency();
            builder.RegisterType<BillingCompanyService>().As<BillingCompanyService>().SingleInstance();
            builder.RegisterType<BillingCompanyFactory>().As<BillingCompanyFactory>().SingleInstance();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();


            Container = builder.Build();
        }



        private static void StartMainApplication()
        {
            cancellationToken = new CancellationToken();

            Task.Factory.StartNew(() =>
            {
                var billingCompanyService = Container.Resolve<BillingCompanyService>();
                billingCompanyService.Start(cancellationToken);

            }, cancellationToken);
        }


    }
}
