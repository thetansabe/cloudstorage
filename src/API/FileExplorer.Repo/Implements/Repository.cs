using FileExplorer.DataConnect;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Repo
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public FileExplorerContext Context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(FileExplorerContext iContext)
        {
            this.Context = iContext;
            this._dbSet = iContext.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            return await Context.SaveChangesAsync() > 0;
        }
    }
}
