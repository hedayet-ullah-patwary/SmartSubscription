using DAL.EF;
using DAL.Interfaces;
using DAL.Repos;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class DataAccessFactory
    {
        SmartSubcriptionsContext db;

        public DataAccessFactory(SmartSubcriptionsContext db)
        {
            this.db = db;
        }

        // Generic repository access
        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(db);
        }


        public IFeatureRepository GetFeatureRepository()
        {
            return new FeatureRepository(db);
        }

        public IPaymentRepository GetPaymentRepository()
        {
            return new PaymentRepository(db);
        }

        public IPlanRepository GetPlanRepository()
        {
            return new PlanRepository(db);
        }

        public IRoleRepository GetRoleRepository()
        {
            return new RoleRepository(db);
        }

        public ISubscriptionRepository GetSubscriptionRepository()
        {
            return new SubscriptionRepository(db);
        }

        public IUserRepository GetUserRepository()
        {
            return new UserRepository(db);
        }

        public IUserRoleRepository GetUserRoleRepository()
        {
            return new UserRoleRepository(db);
        }

    }
}
