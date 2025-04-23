using Microsoft.EntityFrameworkCore;
using PetAdoptionCenter.Domain.OtherModels;
using PetAdoptionCenter.Repository.Interface;
using repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetAdoptionCenter.Repository.implementation
{
    public class AgencyRepository : IAgencyRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Agency> entities;

        public AgencyRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entities = context.Set<Agency>();
        }

        public Agency GetAgencyById(Guid id)
        {
            var tmp = entities
                .SingleOrDefaultAsync(x => x.Id == id).Result;

            return tmp;
        }
    }
}
