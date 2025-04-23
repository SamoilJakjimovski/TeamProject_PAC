
using domain.Models;
using PetAdoptionCenter.Domain.Identity;
using PetAdoptionCenter.Domain.Models;



namespace domain.Identity
{
    public class PetAdoptionCenterUser : Microsoft.AspNetCore.Identity.IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        
        /*public Role? Role { get; set; }*/

        public virtual ICollection<Pet>? Pets { get; set; }

    }
}
