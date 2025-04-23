using domain.Identity;
using Microsoft.EntityFrameworkCore;
using repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repository.implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<PetAdoptionCenterUser> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<PetAdoptionCenterUser>();
        }
        public IEnumerable<PetAdoptionCenterUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public PetAdoptionCenterUser Get(string id)
        {
            var strGuid = id.ToString();
            return entities
                .First(s => s.Id == strGuid);
        }
        public void Insert(PetAdoptionCenterUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(PetAdoptionCenterUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(PetAdoptionCenterUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }

}
