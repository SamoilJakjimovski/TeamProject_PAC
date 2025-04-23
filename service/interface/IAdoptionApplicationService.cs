using PetAdoptionCenter.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetAdoptionCenter.Service.Interface
{
    public interface IAdoptionApplicationService
    {
        public List<AdoptionApplication> GetAdoptionApplications();
        public AdoptionApplication GetAdoptionApplicationById(Guid? id);
        public AdoptionApplication CreateApplicationForm(AdoptionApplication adoptionApplication);
        public AdoptionApplication Apply(string loggedInUser, AdoptionApplication adoptionApplication);
        public AdoptionApplication UpdateAdoptionApplication(AdoptionApplication adoptionApplication);
        public AdoptionApplication DeleteAdoptionApplication(Guid id);
        public List<AdoptionApplication> GetAdoptionApplicationsByPetId(Guid? petId);
        public List<AdoptionApplication> GetAdoptionApplicationsByAdopterId(string? adopterId);
        public void AcceptAdoptionApplication(Guid? id);
        public void PayForAdoption(Guid? id);
    }
}
