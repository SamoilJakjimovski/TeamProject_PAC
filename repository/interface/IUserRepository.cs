using domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<PetAdoptionCenterUser> GetAll();
        PetAdoptionCenterUser Get(string id);
        void Insert(PetAdoptionCenterUser entity);
        void Update(PetAdoptionCenterUser entity);
        void Delete(PetAdoptionCenterUser entity);
    }

}
