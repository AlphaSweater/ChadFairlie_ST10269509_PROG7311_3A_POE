using System.Threading.Tasks;

namespace WebApp.Services
{
    /// <summary>
    /// Base interface for common service operations
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    /// <typeparam name="TId">The type of the entity's identifier</typeparam>
    public interface IBaseService<T, TId>
    {
        /// <summary>
        /// Gets all entities
        /// </summary>
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Gets an entity by its identifier
        /// </summary>
        Task<T?> GetByIdAsync(TId id);

        /// <summary>
        /// Adds a new entity
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity by its identifier
        /// </summary>
        Task DeleteAsync(TId id);
    }
} 