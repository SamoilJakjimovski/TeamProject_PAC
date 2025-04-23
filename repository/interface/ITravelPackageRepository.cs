using PetAdoptionCenter.Domain.OtherModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetAdoptionCenter.Repository.Interface
{
    public interface ITravelPackageRepository
    {
        TravelPackage GetTravelPackageById(Guid id);
        List<TravelPackage> GetAllTravelPackages();
    }
}
