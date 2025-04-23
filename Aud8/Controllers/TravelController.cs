using Microsoft.AspNetCore.Mvc;
using PetAdoptionCenter.Domain.Models;
using PetAdoptionCenter.Domain.OtherModels;
using PetAdoptionCenter.Repository.Interface;
using PetAdoptionCenter.Service.Interface;

namespace PetAdoptionCenter.Web.Controllers
{
    public class TravelController : Controller
    {
        private readonly ITravelPackageService _travelPackageService;
        private readonly IAgencyService _agencyService;

        public TravelController(ITravelPackageService travelPackageService, IAgencyService agencyService)
        {
            _travelPackageService = travelPackageService;
            _agencyService = agencyService;
        }

        public IActionResult Index()
        {
            var data = _travelPackageService.GetAllTravelPackages();

            return View(data);
        }

        public IActionResult Import()
        {
            HttpClient client = new HttpClient();

            string URL = "https://pvstravel-dze7dmcxauf3csfd.eastus-01.azurewebsites.net/api/Admin/GetAllTravelPackages";

            HttpResponseMessage response = client.GetAsync(URL).Result;

            var data = response.Content.ReadAsAsync<List<TravelPackage>>().Result;

            foreach (var item in data)
            {
                var foundAgency = _agencyService.GetAgencyById(item.AgencyId);

                if (foundAgency == null)
                {
                    var agency = new Agency
                    {
                        Id = item.Agency.Id,
                        Name = item.Agency.Name,
                        Email = item.Agency.Email,
                        Phone = item.Agency.Phone,
                        Address = item.Agency.Address,
                        DateCreated = DateTime.Now

                    };

                    _agencyService.CreateNewAgency(agency);
                }


                var foundPackage = _travelPackageService.GetTravelPackage(item.Id);
                if (foundPackage == null)
                {
                    var package = new TravelPackage
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Price = item.Price,
                        Description = item.Description,
                        Images = item.Images,
                        ImageTextBox = item.ImageTextBox,
                        AgencyId = item.AgencyId,
                        Agency = _agencyService.GetAgencyById(item.AgencyId),
                        DateCreated = DateTime.Now
                    };

                    _travelPackageService.CreateNewTravelPackage(package);

                }

            }

            return RedirectToAction("Index", "Travel");

        }
    }
}
