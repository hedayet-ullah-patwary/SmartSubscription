using DAL.EF;
using DAL.EF.Tables;
using DAL.Interfaces;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
    public class FeatureRepository : Repository<Feature>, IFeatureRepository
    {
        public FeatureRepository(SmartSubcriptionsContext db)
            : base(db)
        {
        }

        public Feature GetByName(string name)
        {
            return db.Features.FirstOrDefault(x => x.Name == name);
        }

        public bool IsFeatureExists(string name)
        {
            return db.Features.Any(x => x.Name == name);
        }
    }
}