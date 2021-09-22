using Models;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;

namespace DataOperations
{
    public class ContactContext : IdentityDbContext<User>
    {
        public ContactContext(DbContextOptions<ContactContext> options) : base(options)
        {
             /////
        }        

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries<User>())
            {
                switch (item.State)
                {
                   
                    case EntityState.Modified:
                        item.Entity.CreatedAt = DateTime.UtcNow;
                        item.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Added:
                        item.Entity.CreatedAt = DateTime.UtcNow;
                        item.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                    default:
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
