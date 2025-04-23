using PetAdoptionCenter.Domain.OtherModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetAdoptionCenter.Service.Interface
{
    public interface ITravelPackageService
    {
        void CreateNewTravelPackage(TravelPackage travelPackage);
        TravelPackage GetTravelPackage(Guid id);
        List<TravelPackage> GetAllTravelPackages();
    }
}
