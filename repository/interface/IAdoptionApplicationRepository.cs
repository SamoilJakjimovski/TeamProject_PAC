using PetAdoptionCenter.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetAdoptionCenter.Repository.Interface
{
    public interface IAdoptionApplicationRepository
    {
        List<AdoptionApplication> GetAllAdoptionApplications();
        public AdoptionApplication GetAdoptionApplicationById(Guid? id);
        List<AdoptionApplication> GetAdoptionApplicationByPetId(Guid? petId);
        List<AdoptionApplication> GetAdoptionApplicationByAdopterId(string? AdopterId);
    }
}
