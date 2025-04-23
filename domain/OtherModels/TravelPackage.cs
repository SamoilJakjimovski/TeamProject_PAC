using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetAdoptionCenter.Domain.OtherModels
{
    public class TravelPackage : BaseEntity
    {
        public string? Name { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }
        public List<String>? Images { get; set; }
        public string? ImageTextBox { get; set; }
        public virtual Agency? Agency { get; set; }
        public Guid AgencyId { get; set; }

        public DateTime? DateCreated { get; set; }


    }
}
