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
    public class AgencyService : IAgencyService
    {

        private readonly IRepository<Agency> _agencyRepository;
        private readonly IAgencyRepository _agencyRepoInclude;


        public AgencyService(IRepository<Agency> agencyRepository, IAgencyRepository agencyRepoInclude)
        {
            _agencyRepository = agencyRepository;
            _agencyRepoInclude = agencyRepoInclude;
        }


        public void CreateNewAgency(Agency agency)
        {
            _agencyRepository.Insert(agency);
        }

        public Agency GetAgencyById(Guid id)
        {
            return _agencyRepoInclude.GetAgencyById(id);
        }

        public List<Agency> GetAllAgencies()
        {
            return _agencyRepository.GetAll().ToList();
        }
    }
}
