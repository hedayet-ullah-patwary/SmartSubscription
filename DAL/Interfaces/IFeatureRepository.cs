using DAL.EF.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface IFeatureRepository
    {
        Feature GetByName(string name);
        bool IsFeatureExists(string name);
    }
}
