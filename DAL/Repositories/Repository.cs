using DAL.EF;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly SmartSubcriptionsContext db;
        protected readonly DbSet<T> table;

        public Repository(SmartSubcriptionsContext db)
        {
            this.db = db;
            table = db.Set<T>();
        }

        public SmartSubcriptionsContext Db { get; }

        public bool Create(T entity)
        {
            table.Add(entity);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var entity = table.Find(id);
            if (entity == null)
                return false;

            table.Remove(entity);
            return db.SaveChanges() > 0;
        }

        public T Find(int id)
        {
            return table.Find(id);
        }

        public List<T> GetAll()
        {
            return table.ToList();
        }

        public bool Update(T entity)
        {
            var entry = db.Entry(entity);

            var key = entry.Metadata.FindPrimaryKey();
            if (key == null || key.Properties.Count == 0)
            {
                table.Update(entity);
                return db.SaveChanges() > 0;
            }

            var keyProperty = key.Properties[0];
            var keyValue = entry.Property(keyProperty.Name).CurrentValue;

            var existing = table.Find(keyValue);
            if (existing == null)
                return false;

            db.Entry(existing).CurrentValues.SetValues(entity);
            return db.SaveChanges() > 0;



        }
    }
}
