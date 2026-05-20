using BLL.DTOs;
using DAL;
using DAL.EF.Tables;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class FeatureService
    {
        private readonly DataAccessFactory data;

        public FeatureService(DataAccessFactory data)
        {
            this.data = data;
        }

        public List<FeatureDTO> GetAllFeatures(string sortBy = "name")
        {
            var mapper   = MapperConfig.GetMapper();
            var features = data.GetRepository<Feature>().GetAll();

            features = sortBy switch
            {
                "id"   => features.OrderBy(f => f.Id).ToList(),
                _      => features.OrderBy(f => f.Name).ToList()
            };

            return mapper.Map<List<FeatureDTO>>(features);
        }

        public FeatureDTO GetById(int id)
        {
            var mapper = MapperConfig.GetMapper();
            var entity = data.GetRepository<Feature>().Find(id);
            return mapper.Map<FeatureDTO>(entity);
        }

        public FeatureDTO GetByName(string name)
        {
            var mapper  = MapperConfig.GetMapper();
            var feature = data.GetFeatureRepository().GetByName(name);
            return mapper.Map<FeatureDTO>(feature);
        }

        public bool CreateFeature(FeatureDTO feature)
        {
            if (feature == null)
                return false;

            var mapper = MapperConfig.GetMapper();
            var entity = mapper.Map<Feature>(feature);
            return data.GetRepository<Feature>().Create(entity);
        }

        public bool UpdateFeature(FeatureDTO feature)
        {
            var exists = data.GetRepository<Feature>().Find(feature.Id);
            if (exists == null)
                return false;

            var mapper = MapperConfig.GetMapper();
            var entity = mapper.Map<Feature>(feature);
            return data.GetRepository<Feature>().Update(entity);
        }

        public bool DeleteFeature(int id)
        {
            return data.GetRepository<Feature>().Delete(id);
        }

        public bool IsFeatureExists(string name)
        {
            return data.GetFeatureRepository().IsFeatureExists(name);
        }
    }
}
