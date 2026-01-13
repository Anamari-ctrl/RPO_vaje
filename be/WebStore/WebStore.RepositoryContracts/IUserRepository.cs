using WebStore.Entities.Identity;

namespace WebStore.RepositoryContracts
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetUserData(Guid userId);
    }
}
