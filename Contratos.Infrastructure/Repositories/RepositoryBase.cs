using Contratos.Domain.Repositories;
using Contratos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _ctx;
        protected readonly DbSet<T> _db;

        public RepositoryBase(AppDbContext ctx)
        {
            _ctx = ctx;
            _db = _ctx.Set<T>();
        }
        public async Task AddAsync(T entity) => await _db.AddAsync(entity);
        public async Task<T?> GetByIdAsync(int id) => await _db.FindAsync(id);
        public async Task<List<T>> ListAsync(Expression<Func<T, bool>>? predicate = null)
            => predicate is null ? await _db.AsNoTracking().ToListAsync()
                                 : await _db.AsNoTracking().Where(predicate).ToListAsync();
        public void Update(T entity) => _db.Update(entity);
        public void Remove(T entity) => _db.Remove(entity);
    }
}
