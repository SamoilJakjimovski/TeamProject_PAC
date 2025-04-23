using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PetAdoptionCenter.Domain.Models;
using PetAdoptionCenter.Repository.Interface;
using repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetAdoptionCenter.Repository.implementation
{
    public class AdoptionApplicationRepository : IAdoptionApplicationRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<AdoptionApplication> entities;

        public AdoptionApplicationRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entities = context.Set<AdoptionApplication>();
        }

        public List<AdoptionApplication> GetAdoptionApplicationByAdopterId(string? adopterId)
        {
            return entities
                .Include("Pet")
                .Include("Pet.Shelter")
                .Include("Adopter")
                .Where(x => x.AdopterId.Equals(adopterId))
                .ToList();
        }

        public AdoptionApplication GetAdoptionApplicationById(Guid? id)
        {
            var tmp = entities
                .Include("Pet")
                .Include("Pet.Shelter")
                .Include("Adopter")
                .SingleOrDefaultAsync(x  => x.Id == id).Result;

            return tmp;
        }

        public List<AdoptionApplication> GetAdoptionApplicationByPetId(Guid? petId)
        {
            return entities
                .Include("Pet")
                .Include("Pet.Shelter")
                .Include("Adopter")
                .Where(x => x.PetId == petId)
                .ToList();

        }

        public List<AdoptionApplication> GetAllAdoptionApplications()
        {
            return entities
                .Include("Pet")
                .Include("Pet.Shelter")
                .Include("Adopter")
                .ToList();
        }
    }
}
