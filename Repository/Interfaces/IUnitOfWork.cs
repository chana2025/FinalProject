using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IFoodPreferenceRepository FoodPreferenceRepository { get; }
        //void Save(); // אפשר להשאיר אם רוצים תמיכה גם בסינכרוני
        Task SaveAsync(); // המתודה האסינכרונית
    }
}
