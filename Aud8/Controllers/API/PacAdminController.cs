using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetAdoptionCenter.Domain.Models;
using PetAdoptionCenter.Service.Interface;

namespace PetAdoptionCenter.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacAdminController : ControllerBase
    {
        private readonly IPetService _petService;
        private readonly IAdoptionApplicationService _adoptionApplicationService;

        public PacAdminController(IPetService petService, IAdoptionApplicationService adoptionApplicationService)
        {
            _petService = petService;
            _adoptionApplicationService = adoptionApplicationService;
        }

        [HttpGet("[action]/{id}")]
        public Pet GetPetDetails(Guid id)
        {
            return _petService.GetPetById(id);
        }

        [HttpGet("[action]/{id}")]
        public List<Pet> GetMyPets(string id)
        {
            return _petService.GetPetsByShelterId(id);
        }

        [HttpGet("[action]")]
        public List<Pet> ListAllPets()
        {
            return _petService.GetPets();
        }
    }
}
