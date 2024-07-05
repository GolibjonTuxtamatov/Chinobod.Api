using Chinobod.Api.Models.Foundations;
using Microsoft.EntityFrameworkCore;

namespace Chinobod.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<News> News { get; set; }
    }
}
