using domain.Identity;
using domain.Models;
using PetAdoptionCenter.Domain.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetAdoptionCenter.Domain.Models
{
    public class AdoptionApplication : BaseEntity
    {
        public AdoptionApplicationStatus AdoptionApplicationStatus { get; set; }
        public string? AdopterId { get; set; }
        public PetAdoptionCenterUser? Adopter {  get; set; }
        public Guid? PetId { get; set; }
        public Pet? Pet { get; set; }
        public string? Question1 { get; set; }
        public string? Question2 { get; set; }
        public string? Question3 { get; set; }
        public string? Question4 { get; set; }
        public string? Question5 { get; set; }
        public string? Question6 { get; set; }
        public string? Question7 { get; set; }
        public string? Question8 { get; set; }
        public string? Question9 { get; set; }
        public string? Question10 { get; set; }
    }
}
