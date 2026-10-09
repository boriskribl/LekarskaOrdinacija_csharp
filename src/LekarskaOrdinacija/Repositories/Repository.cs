using LekarskaOrdinacija.Data;
using Microsoft.EntityFrameworkCore;
namespace LekarskaOrdinacija.Repositories;
public class Repository<T> : IRepository<T> where T:class
{
    protected readonly ApplicationDbContext Db;
    protected readonly DbSet<T> Set;
    public Repository(ApplicationDbContext db){Db=db;Set=db.Set<T>();}
    public Task<List<T>> GetAllAsync()=>Set.AsNoTracking().ToListAsync();
    public Task<T?> GetByIdAsync(int id)=>Set.FindAsync(id).AsTask();
    public async Task AddAsync(T entity)=>await Set.AddAsync(entity);
    public void Update(T entity)=>Set.Update(entity);
    public void Delete(T entity)=>Set.Remove(entity);
    public Task SaveAsync()=>Db.SaveChangesAsync();
}