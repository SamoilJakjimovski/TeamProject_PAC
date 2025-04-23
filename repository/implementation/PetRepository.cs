using domain.Models;
using Microsoft.EntityFrameworkCore;
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
    public class PetRepository : IPetRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Pet> entities;

        public PetRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entities = context.Set<Pet>();
        }
      

        public List<Pet> GetAllPets()
        {
            return entities
                .Include(z => z.Shelter)
                .ToList();
        }

        public Pet GetPetById(Guid? id)
        {
            var tmp =  entities
                .Include(z => z.Shelter)
                .SingleOrDefaultAsync(x => x.Id == id).Result;

            return tmp;
        }

        public List<Pet> GetPetsByShelterId(string? shelterId)
        {
            return entities
                .Include(z => z.Shelter)
                .Where(x => x.ShelterId.Equals(shelterId))
                .ToList();
        }
    }
}
