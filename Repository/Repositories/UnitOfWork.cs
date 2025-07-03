using System.Threading.Tasks;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IContext _context;

        public IFoodPreferenceRepository FoodPreferenceRepository { get; }

        public UnitOfWork(IContext context, IFoodPreferenceRepository foodPreferenceRepository)
        {
            _context = context;
            FoodPreferenceRepository = foodPreferenceRepository;
        }

        public async Task SaveAsync()
        {
            await _context.SaveAsync();
        }

       
    }
}
