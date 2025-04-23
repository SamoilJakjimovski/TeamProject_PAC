using PetAdoptionCenter.Domain.OtherModels;
using PetAdoptionCenter.Repository.Interface;
using PetAdoptionCenter.Service.Interface;
using repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetAdoptionCenter.Service.implementation
{
    public class TravelPackageService : ITravelPackageService
    {
        private readonly IRepository<TravelPackage> _travelPackageRepo;
        private readonly ITravelPackageRepository _travelPackageRepoInclude;

        public TravelPackageService(IRepository<TravelPackage> travelPackageRepo, ITravelPackageRepository travelPackageRepoInclude)
        {
            _travelPackageRepo = travelPackageRepo;
            _travelPackageRepoInclude = travelPackageRepoInclude;
        }

        public void CreateNewTravelPackage(TravelPackage travelPackage)
        {
            _travelPackageRepo.Insert(travelPackage);
        }

        public List<TravelPackage> GetAllTravelPackages()
        {
            return _travelPackageRepoInclude.GetAllTravelPackages();
        }

        public TravelPackage GetTravelPackage(Guid id)
        {
            return _travelPackageRepoInclude.GetTravelPackageById(id);
        }
    }
}
