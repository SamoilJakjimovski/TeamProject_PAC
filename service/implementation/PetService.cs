using domain.Models;
using PetAdoptionCenter.Domain.Models;
using PetAdoptionCenter.Repository.Interface;
using PetAdoptionCenter.Service.Interface;
using repository.implementation;
using repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetAdoptionCenter.Service.implementation
{
    public class PetService : IPetService
    {
        private readonly IRepository<Pet> _petRepository;
        private readonly IPetRepository _petRepoInclude;
        private readonly IUserRepository _userRepository;

        public PetService(IRepository<Pet> petRepository, IPetRepository petRepoInclude, IUserRepository userRepository)
        {
            _petRepository = petRepository;
            _petRepoInclude = petRepoInclude;
            _userRepository = userRepository;
        }

        public void ChangePetStatus(Guid? id)
        {
            var pet = _petRepoInclude.GetPetById(id);
            pet.PetStatus = Domain.enums.PetStatus.ADOPTED;
            _petRepository.Update(pet);
        }

        public Pet CreateNewPet(string loggedInUser, Pet pet)
        {
            var loggedShelter = _userRepository.Get(loggedInUser);
            pet.ShelterId = loggedShelter.Id;
            pet.Shelter = loggedShelter;

            pet.PetStatus = Domain.enums.PetStatus.AVAILABLE_FOR_ADOPTION;
            return this._petRepository.Insert(pet);
        }

        public Pet DeletePet(Guid id)
        {
            return _petRepository.Delete(GetPetById(id));
        }

        public Pet GetPetById(Guid? id)
        {
            return _petRepoInclude.GetPetById(id);
        }

        public List<Pet> GetPets()
        {
            return _petRepoInclude.GetAllPets();
        }

        public List<Pet> GetPetsByShelterId(string? shelterId)
        {
            return _petRepoInclude.GetPetsByShelterId(shelterId);
        }

        public Pet UpdatePet(Pet pet)
        {
            return _petRepository.Update(pet);
        }
    }
}
