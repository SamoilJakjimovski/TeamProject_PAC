using Microsoft.EntityFrameworkCore;
using PetAdoptionCenter.Domain.Models;
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
    public class TravelPackageRepository : ITravelPackageRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<TravelPackage> entities;

        public TravelPackageRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entities = context.Set<TravelPackage>();
        }

        public List<TravelPackage> GetAllTravelPackages()
        {
            return entities
                .Include(x => x.Agency)
                .ToList();
        }

        public TravelPackage GetTravelPackageById(Guid id)
        {
            var tmp = entities
                .Include(x => x.Agency)
                .SingleOrDefaultAsync(x => x.Id == id).Result;


            return tmp;
        }
    }
}
