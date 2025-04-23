using domain.Models;
using PetAdoptionCenter.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetAdoptionCenter.Service.Interface
{
    public interface IPetService
    {
        public List<Pet> GetPets();
        public Pet GetPetById(Guid? id);
        public Pet CreateNewPet(string loggedInUser, Pet pet);
        public Pet UpdatePet(Pet pet);
        public Pet DeletePet(Guid id);
        public List<Pet> GetPetsByShelterId(string? shelterId);
        public void ChangePetStatus(Guid? id);

    }
}
