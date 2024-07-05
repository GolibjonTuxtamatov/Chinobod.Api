using Chinobod.Api.Models.Foundations;
using EFxceptions;
using Microsoft.EntityFrameworkCore;

namespace Chinobod.Api.Brokers.Storages
{
    public partial class StorageBroker : EFxceptionsContext, IStorageBroker
    {
        public StorageBroker()
        {
            this.Database.Migrate();
        }

        public async ValueTask<T> InsertAsync<T>(T @object)
        {
            using var broker = new StorageBroker();
            broker.Entry(@object).State = EntityState.Added;
            await broker.SaveChangesAsync();

            return @object;
        }

        public IQueryable<T> SelectAll<T>() where T : class
        {
            using var broker = new StorageBroker();

            return broker.Set<T>();
        }

        public async ValueTask<T> SelectByIdAsync<T>(params object[] objectsId) where T : class
        {
            using var broker = new StorageBroker();
            
            return await broker.FindAsync<T>(objectsId);
        }

        public async ValueTask<T> UpdateAsync<T>(T @object)
        {
            using var broker = new StorageBroker();
            broker.Entry(@object).State = EntityState.Modified;
            await broker.SaveChangesAsync();

            return @object;
        }

        public async ValueTask<T> DeleteAsync<T>(T @object)
        {
            using var broker = new StorageBroker();
            broker.Entry(@object).State = EntityState.Deleted;
            await broker.SaveChangesAsync();

            return @object;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Data Source = ./Data/Chinobod.db";
            
            optionsBuilder.UseSqlite(connectionString);
        }

        public override void Dispose(){ }
    }
}
