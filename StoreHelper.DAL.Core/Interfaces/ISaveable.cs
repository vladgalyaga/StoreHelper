using System.Threading.Tasks;

namespace StoreHelper.Dal.Core.Interfaces
{
    /// <summary>
    /// Defines a members for saving data
    /// </summary>
    public interface ISaveable
    {
        /// <summary>
        /// Save data context changes
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
        /// <summary>
        /// Asynchronously save data context changes
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}