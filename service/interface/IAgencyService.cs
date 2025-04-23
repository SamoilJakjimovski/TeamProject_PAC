using PetAdoptionCenter.Domain.OtherModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetAdoptionCenter.Service.Interface
{
    public interface IAgencyService
    {
        void CreateNewAgency(Agency agency);
        Agency GetAgencyById(Guid id);
        List<Agency> GetAllAgencies();
    }
}
