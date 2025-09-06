using CareerSpark.DataAccessLayer.Context;
using CareerSpark.DataAccessLayer.Entities;
using CareerSpark.DataAccessLayer.Interfaces;

namespace CareerSpark.DataAccessLayer.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(CareerSparkDbContext context) : base(context)
        {
        }

    }
}
