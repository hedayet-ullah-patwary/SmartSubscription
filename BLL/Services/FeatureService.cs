using BLL.DTOs;
using DAL;
using DAL.EF.Tables;
using System.Collections.Generic;

namespace BLL.Services
{
    public class FeatureService
    {
        private readonly DataAccessFactory data;

        public FeatureService(DataAccessFactory data)
        {
            this.data = data;
        }

        public List<FeatureDTO> GetAllFeatures()
        {
            var mapper = MapperConfig.GetMapper();
            var features = data.GetRepository<Feature>().GetAll();
            return mapper.Map<List<FeatureDTO>>(features);
        }

        public FeatureDTO GetByName(string name)
        {
            var mapper = MapperConfig.GetMapper();
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