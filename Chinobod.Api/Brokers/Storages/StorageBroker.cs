using Chinobod.Api.Models.Foundations;
using EFxceptions;
using Microsoft.EntityFrameworkCore;

namespace Chinobod.Api.Brokers.Storages
{
    public partial class StorageBroker : EFxceptionsContext
    {
        public StorageBroker()
        {
            this.Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Data Source = ./Data/Chinobod.db";
            
            optionsBuilder.UseSqlite(connectionString);
        }

        public override void Dispose(){ }
    }
}
