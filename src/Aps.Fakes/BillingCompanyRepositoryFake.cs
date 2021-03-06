﻿using System;
using System.Linq;
using System.Collections.Generic;
using Aps.BillingCompanies;
using Aps.BillingCompanies.Aggregates;
using Aps.BillingCompanies.ValueObjects;
using Caliburn.Micro;
using Seterlund.CodeGuard;

namespace Aps.Fakes
{
    public class BillingCompanyRepositoryFake : IBillingCompanyRepository
    {
        private readonly List<BillingCompany> billingCompanies;

        private readonly IEventAggregator eventAggregator;
        private readonly BillingCompanyFactory billingCompanyFactory;

        public BillingCompanyRepositoryFake(IEventAggregator eventAggregator, BillingCompanyFactory billingCompanyFactory)
        {
            this.eventAggregator = eventAggregator;
            this.billingCompanyFactory = billingCompanyFactory;
            this.billingCompanies = new List<BillingCompany>();
        }

        public void StoreBillingCompany(BillingCompany billingCompany)
        {
            // validate Ids?
            this.billingCompanies.Add(billingCompany);
        }

        public BillingCompany GetBillingCompanyById(Guid id)
        {
            return this.billingCompanies.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<BillingCompany> GetAllBillingCompanies()
        {
            return billingCompanies;
        }

        public void RemoveBillingCompanyById(Guid billingCompanyId)
        {
            var billingCompany = billingCompanies.FirstOrDefault(company => company.Id == billingCompanyId);

            if (billingCompany != null)
            {
                this.billingCompanies.Remove(billingCompany);
            }
        }
    }
}