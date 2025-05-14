using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApp.Models;

namespace WebApp.Repositories
{
	public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
	{
		protected readonly AgriDbContext _context;
		protected readonly DbSet<TEntity> _dbSet;

		protected BaseRepository(AgriDbContext context)
		{
			_context = context;
			_dbSet = context.Set<TEntity>();
		}

		public virtual async Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
		{
			IQueryable<TEntity> query = _dbSet.Where(e => !e.IsDeleted);

			foreach (var include in includes)
			{
				query = query.Include(include);
			}

			return await query.ToListAsync();
		}

		public virtual async Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes)
		{
			IQueryable<TEntity> query = _dbSet.Where(e => !e.IsDeleted);

			foreach (var include in includes)
			{
				query = query.Include(include);
			}

			return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, GetIdPropertyName()) == id);
		}

		public virtual async Task AddAsync(TEntity entity)
		{
			entity.CreatedOn = DateTime.UtcNow;
			await _dbSet.AddAsync(entity);
		}

		public virtual async Task Update(TEntity entity)
		{
			entity.UpdatedOn = DateTime.UtcNow;
			_dbSet.Update(entity);
		}

		public virtual async Task DeleteAsync(int id)
		{
			var entity = await GetByIdAsync(id);
			if (entity != null)
			{
				entity.IsDeleted = true;
				entity.UpdatedOn = DateTime.UtcNow;
				_dbSet.Update(entity);
			}
		}

		public virtual async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
		{
			IQueryable<TEntity> query = _dbSet.Where(e => !e.IsDeleted).Where(predicate);

			foreach (var include in includes)
			{
				query = query.Include(include);
			}

			return await query.ToListAsync();
		}

		public virtual async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}

		protected abstract string GetIdPropertyName();
	}
}