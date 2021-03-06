﻿using System;
using System.Threading;
using Aps.Integration;
using Caliburn.Micro;

namespace Aps.BillingCompanies.ApplicationService
{
    public class BillingCompanyService
    {
        private readonly EventIntegrationService eventIntegrationService;
        private readonly IEventAggregator eventAggregator;
        private readonly IBillingCompanyRepository billingCompanyRepository;

        public BillingCompanyService(IEventAggregator eventAggregator, EventIntegrationService eventIntegrationService, IBillingCompanyRepository billingCompanyRepository)
        {
            this.eventIntegrationService = eventIntegrationService;
            this.eventAggregator = eventAggregator;
            this.billingCompanyRepository = billingCompanyRepository;

            eventAggregator.Subscribe(this);
        }

        public void Start(System.Threading.CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Waiting for EventAggregator Events");
                Console.WriteLine("Pausing, and then redistributing internal events");
                Thread.Sleep(1000);
            }
        }
    }
}