using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClosedXML.Excel;
using domain.Identity;
using domain.Models;
using GemBox.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetAdoptionCenter.Domain.enums;
using PetAdoptionCenter.Domain.Models;
using PetAdoptionCenter.Service.Interface;
using repository;
using repository.Interface;

namespace PetAdoptionCenter.Web.Controllers
{
    public class PetsController : Controller
    {
        private readonly IPetService _petService;
        private readonly IAdoptionApplicationService _adoptionApplicationService;
        private readonly UserManager<PetAdoptionCenterUser> _userManager;
        private readonly IUserRepository _userRepository;

        public PetsController(IPetService petService, IAdoptionApplicationService adoptionApplicationService, UserManager<PetAdoptionCenterUser> userManager, IUserRepository userRepository)
        {
            _petService = petService;
            _adoptionApplicationService = adoptionApplicationService;
            _userManager = userManager;
            _userRepository = userRepository;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        // GET: Pets
        [AllowAnonymous]
        public IActionResult Index()
        {
            
            return View(_petService.GetPets());
        }

        // GET: Pets/Details/5
        [Authorize(Roles = "Shelter,Adopter")]
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = _petService.GetPetById(id);
            if (pet == null)
            {
                return NotFound();
            }

            ViewData["AppForms"] = _adoptionApplicationService.GetAdoptionApplicationsByPetId(pet.Id);

            /*var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = _userManager.FindByIdAsync(userId);
            
            ViewData["LoggedInUser"] = user;*/

            return View(pet);
        }


        [Authorize(Roles = "Shelter")]
        public IActionResult MyPets(string? id)
        {
            return View(_petService.GetPetsByShelterId(id));
        }

        // GET: Pets/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            var isAdopter = await _userManager.IsInRoleAsync(user, "Adopter");

            ViewData["IsAdopter"] = isAdopter;

            ViewData["PetType"] = new SelectList(
                Enum.GetValues(typeof(PetType))
                    .Cast<PetType>()
                    .Select(e => new { Id = (int)e, Name = e.ToString() }),
                "Id", "Name");

            if (isAdopter)
            {
                var allUsers = await _userManager.Users.ToListAsync();

                var shelters = new List<PetAdoptionCenterUser>();
                foreach (var u in allUsers)
                {
                    if (await _userManager.IsInRoleAsync(u, "Shelter"))
                    {
                        shelters.Add(u);
                    }
                }

                ViewData["Shelters"] = new SelectList(
                    shelters.Select(s => new { s.Id, FullName = s.FirstName + " " + s.LastName }),
                    "Id", "FullName");
            }
            else
            {
                ViewData["LoggedShelterId"] = user.Id;
            }

            return View();
        }

        // POST: Pets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Shelter,Adopter")]
        public async Task<IActionResult> Create([Bind("Name,Age,PetType,Breed,Sex,Description,IsHouseTrained,FavouriteThings,HomeRequirements,Price,PhotoUrl,Id")] Pet pet, string LoggedShelterId)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            if (ModelState.IsValid)
            {
                pet.ShelterId = string.IsNullOrEmpty(LoggedShelterId) ? loggedInUserId : LoggedShelterId;

                _petService.CreateNewPet(pet.ShelterId, pet);

                var isAdopter = await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(User), "Adopter");

                if (isAdopter)
                {
                    return RedirectToAction("Index", "Pets"); 
                }

                return RedirectToAction("MyPets", new { id = pet.ShelterId }); 
            }

            ViewData["PetType"] = new SelectList(
                Enum.GetValues(typeof(PetType))
                    .Cast<PetType>()
                    .Select(e => new { Id = (int)e, Name = e.ToString() }),
                "Id", "Name");

            var currentUser = await _userManager.GetUserAsync(User);
            var isCurrentUserAdopter = await _userManager.IsInRoleAsync(currentUser, "Adopter");
            ViewData["IsAdopter"] = isCurrentUserAdopter;

            if (isCurrentUserAdopter)
            {
                var allUsers = await _userManager.Users.ToListAsync();
                var shelters = new List<PetAdoptionCenterUser>();

                foreach (var u in allUsers)
                {
                    if (await _userManager.IsInRoleAsync(u, "Shelter"))
                    {
                        shelters.Add(u);
                    }
                }

                ViewData["Shelters"] = new SelectList(
                    shelters.Select(s => new { s.Id, FullName = s.FirstName + " " + s.LastName }),
                    "Id", "FullName");
            }
            else
            {
                ViewData["LoggedShelterId"] = loggedInUserId;
            }

            return View(pet);
        }



        // GET: Pets/Edit/5
        [Authorize(Roles = "Shelter")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = _petService.GetPetById(id);
            if (pet == null)
            {
                return NotFound();
            }
            //ViewData["ShelterId"] = new SelectList(_context.Users, "Id", "Id", pet.ShelterId);
            return View(pet);
        }

        // POST: Pets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Name,Age,PetType,Breed,Sex,Description,IsHouseTrained,FavouriteThings,HomeRequirements,Price,PhotoUrl,PetStatus,ShelterId,Id")] Pet pet)
        {
            if (id != pet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _petService.UpdatePet(pet);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("MyPets", new { id = pet.ShelterId });
            }
            //ViewData["ShelterId"] = new SelectList(_context.Users, "Id", "Id", pet.ShelterId);
            return View(pet);
        }

        // GET: Pets/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = _petService.GetPetById(id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // POST: Pets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var pet = _petService.GetPetById(id);
            if (pet != null)
            {
                _petService.DeletePet(pet.Id);
            }

            return RedirectToAction("MyPets", new { id = pet.ShelterId });
        }

        /*private bool PetExists(Guid id)
        {
            return _context.Pet.Any(e => e.Id == id);
        }*/


        public FileContentResult CreatePetDetailsDocument(Guid? id)
        {
            var pet = _petService.GetPetById(id);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "PetDetails.docx");
            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{PetName}}", pet.Name);
            document.Content.Replace("{{Shelter}}", pet.Shelter.FirstName);
            document.Content.Replace("{{Type}}", pet.PetType.ToString());
            document.Content.Replace("{{Breed}}", pet.Breed);
            document.Content.Replace("{{Sex}}", pet.Sex);
            document.Content.Replace("{{Description}}", pet.Description);

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportPetDetails.pdf");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RateUsers(Guid userId, int rating, Guid id)
        {
            var user = _userRepository.Get(userId.ToString());
            user.NumberRatings++;
            user.Rating = (user.Rating * (user.NumberRatings - 1) + rating) / (double)user.NumberRatings;
            _userRepository.Update(user);


            return RedirectToAction("Details", new { id = id });
        }


        public FileContentResult ExportAllPetsByShelter(string? id)
        {
            string fileName = "MyPets.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Pets");
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "Type";
                worksheet.Cell(1, 4).Value = "Breed";
                worksheet.Cell(1, 5).Value = "Sex";
                worksheet.Cell(1, 6).Value = "Description";
                worksheet.Cell(1, 7).Value = "Number of applications";
                worksheet.Cell(1, 8).Value = "Status";

                var pets = _petService.GetPetsByShelterId(id);

                for (int i = 0; i < pets.Count(); i++)
                {
                    var pet = pets[i];

                    worksheet.Cell(i + 2, 1).Value = pet.Id.ToString();
                    worksheet.Cell(i + 2, 2).Value = pet.Name;
                    worksheet.Cell(i + 2, 3).Value = pet.PetType.ToString();
                    worksheet.Cell(i + 2, 4).Value = pet.Breed;
                    worksheet.Cell(i + 2, 5).Value = pet.Sex;
                    worksheet.Cell(i + 2, 6).Value = pet.Description;

                    var num = _adoptionApplicationService.GetAdoptionApplicationsByPetId(pet.Id).Count();
                    worksheet.Cell(i + 2, 7).Value = num.ToString();
                    worksheet.Cell(i + 2, 8).Value = pet.PetStatus.ToString();

                }

                using(var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

    }
}
