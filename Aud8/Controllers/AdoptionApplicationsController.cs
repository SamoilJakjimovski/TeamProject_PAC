using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetAdoptionCenter.Domain.enums;
using PetAdoptionCenter.Domain.Models;
using PetAdoptionCenter.Service.Interface;
using repository;
using repository.Interface;
using Stripe;

namespace PetAdoptionCenter.Web.Controllers
{
    public class AdoptionApplicationsController : Controller
    {
        private readonly IAdoptionApplicationService _adoptionApplicationService;
        private readonly IPetService _petService;
        private readonly IUserRepository _userRepository;

        public AdoptionApplicationsController(IAdoptionApplicationService adoptionApplicationService, IPetService petService, IUserRepository userRepository)
        {
            _adoptionApplicationService = adoptionApplicationService;
            _petService = petService;
            _userRepository = userRepository;
        }

        // GET: AdoptionApplications
        public IActionResult Index()
        {
            return View(_adoptionApplicationService.GetAdoptionApplications());
        }

        // GET: AdoptionApplications/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adoptionApplication = _adoptionApplicationService.GetAdoptionApplicationById(id);    
            if (adoptionApplication == null)
            {
                return NotFound();
            }

            return View(adoptionApplication);
        }

        // GET: AdoptionApplications/Create
        [Authorize(Roles = "Adopter")]
        public IActionResult Create()
        {
            ViewData["AdopterId"] = new SelectList(_userRepository.GetAll(), "Id", "Id");
            ViewData["PetId"] = new SelectList(_petService.GetPets(), "Id", "Id");
            ViewData["Status"] = new SelectList(
                                        Enum.GetValues(typeof(AdoptionApplicationStatus))
                                        .Cast<AdoptionApplicationStatus>()
                                        .Select(e => new { Id = (int)e, Name = e.ToString() }),
                                        "Id", "Name");
            return View();
        }

        // POST: AdoptionApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("AdoptionApplicationStatus,AdopterId,PetId,Question1,Question2,Question3,Question4,Question5,Question6,Question7,Question8,Question9,Question10,Id")] AdoptionApplication adoptionApplication)
        {
            if (ModelState.IsValid)
            {
                _adoptionApplicationService.CreateApplicationForm(adoptionApplication);
                return RedirectToAction(nameof(MyApplications));
            }
            ViewData["AdopterId"] = new SelectList(_userRepository.GetAll(), "Id", "Id");
            ViewData["PetId"] = new SelectList(_petService.GetPets(), "Id", "Id");
            return View(adoptionApplication);
        }


        [HttpGet]
        [Authorize(Roles = "Adopter")]
        public IActionResult Apply(Guid? petId)
        {
            //ViewBag.petId = petId;
            var pet = _petService.GetPetById(petId);
            var adoptapp = new AdoptionApplication
            {
                PetId = pet.Id
            };

            return View(adoptapp);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Apply([Bind("PetId,Question1,Question2,Question3,Question4,Question5,Question6,Question7,Question8,Question9,Question10,Id")] AdoptionApplication adoptionApplication)
        {
            if (ModelState.IsValid)
            {
                var loggedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                _adoptionApplicationService.Apply(loggedInUser, adoptionApplication);
                return RedirectToAction("Index", "Pets");
            }
            
            return View(adoptionApplication);
        }


        public IActionResult MyApplications(string? id)
        {
            return View(_adoptionApplicationService.GetAdoptionApplicationsByAdopterId(id));
        }

        [Authorize(Roles = "Shelter")]
        public IActionResult Applications(Guid? petId)
        {
            return View(_adoptionApplicationService.GetAdoptionApplicationsByPetId(petId));
        }

        [HttpGet]
        public IActionResult Accept(Guid? id)
        {
            var appForm = _adoptionApplicationService.GetAdoptionApplicationById(id);
            _adoptionApplicationService.AcceptAdoptionApplication(id);
            return RedirectToAction("Applications", new {petId = appForm.PetId});
        }

        public IActionResult PayOrder(string stripeEmail, string stripeToken, Guid? id)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "N/A";
            StripeConfiguration.ApiKey = "sk_test_51PrNaG06QKoQYSRpbAOKdi8nkp01VMSCSenn1adeU6EhfSpBkoD3GUOgqrnBsEHeoeDvHzDOUHRcbMTULJE29ApB008c9rfxBc";
            var appForm = _adoptionApplicationService.GetAdoptionApplicationById(id);

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = Convert.ToInt32(appForm.Pet.Price) * 100,
                Description = "EShop Application Payment",
                Currency = "eur",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                var result = this.Pay(id);
                return RedirectToAction("MyApplications", new { id = appForm.AdopterId });
            }

            return RedirectToAction("MyApplications", new { id = appForm.AdopterId });
        }


        public bool Pay(Guid? id)
        {
            var appForm = _adoptionApplicationService.GetAdoptionApplicationById(id);
            _adoptionApplicationService.PayForAdoption(id);
            return true;
        }



        // GET: AdoptionApplications/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adoptionApplication = _adoptionApplicationService.GetAdoptionApplicationById(id);
            if (adoptionApplication == null)
            {
                return NotFound();
            }
            ViewData["AdopterId"] = new SelectList(_userRepository.GetAll(), "Id", "Id");
            ViewData["PetId"] = new SelectList(_petService.GetPets(), "Id", "Id");
            return View(adoptionApplication);
        }

        // POST: AdoptionApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("AdoptionApplicationStatus,AdopterId,PetId,Question1,Question2,Question3,Question4,Question5,Question6,Question7,Question8,Question9,Question10,Id")] AdoptionApplication adoptionApplication)
        {
            if (id != adoptionApplication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _adoptionApplicationService.UpdateAdoptionApplication(adoptionApplication);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("MyApplications", new {id = adoptionApplication.AdopterId});
            }
           
            return View(adoptionApplication);
        }

        // GET: AdoptionApplications/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adoptionApplication = _adoptionApplicationService.GetAdoptionApplicationById(id);
            if (adoptionApplication == null)
            {
                return NotFound();
            }

            return View(adoptionApplication);
        }

        // POST: AdoptionApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var adoptionApplication = _adoptionApplicationService.GetAdoptionApplicationById(id);
            if (adoptionApplication != null)
            {
                _adoptionApplicationService.DeleteAdoptionApplication(id);
            }

           
            return RedirectToAction("MyApplications", new {id = adoptionApplication.AdopterId});
        }

       /* private bool AdoptionApplicationExists(Guid id)
        {
            return _context.AdoptionApplication.Any(e => e.Id == id);
        }*/
    }
}
