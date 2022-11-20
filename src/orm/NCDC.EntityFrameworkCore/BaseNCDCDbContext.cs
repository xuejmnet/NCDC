using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NCDC.EntityFrameworkCore.Entities.Base;

namespace NCDC.EntityFrameworkCore;

public abstract class BaseNCDCDbContext<T>:DbContext where T:DbContext
{
    
    protected BaseNCDCDbContext(DbContextOptions<T> options):base(options)
    {
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        HandlePropertiesBeforeSave();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        HandlePropertiesBeforeSave();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    protected  void HandlePropertiesBeforeSave()
    {
        foreach (var entry in ChangeTracker.Entries().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    ApplyConceptsForAddedEntity(entry);
         
                    break;
                case EntityState.Modified:
                    ApplyConceptsForModifiedEntity(entry);
                    UpdateConcurrencyStamp(entry);
                    break;
            }
        }
    }
    protected  void UpdateConcurrencyStamp(EntityEntry entry)
    {
        var entity = entry.Entity as IVersion;
        if (entity == null)
        {
            return;
        }

        Entry(entity).Property(x => x.Version).OriginalValue = entity.Version;
        entity.Version = Guid.NewGuid().ToString("N");
    }
    protected  void ApplyConceptsForAddedEntity(EntityEntry entry)
    {
        SetConcurrencyStampIfNull(entry);
        SetCreationProperties(entry);
    }
    protected  void ApplyConceptsForModifiedEntity(EntityEntry entry)
    {
        SetConcurrencyStampIfNull(entry);
        SetUpdateProperties(entry);
    }
    protected  void CheckAndSetId(EntityEntry entry)
    {
        if (entry.Entity is IEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString("n");
            }
        }
    }
    protected  void SetConcurrencyStampIfNull(EntityEntry entry)
    {
        var entity = entry.Entity as IVersion;
        if (entity == null)
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(entity.Version))
        {
            return;
        }

        entity.Version = Guid.NewGuid().ToString("N");
    }
    protected  void SetCreationProperties(EntityEntry entry)
    {
        if (entry.Entity is ICreateTime entity)
        {
            if (entity.CreateTime == default)
            {
                entity.CreateTime=DateTime.Now;
            }
        }

        SetUpdateProperties(entry);
    }
    protected  void SetUpdateProperties(EntityEntry entry)
    {
        if (entry.Entity is IUpdateTime entity)
        {
            if (entity.UpdateTime == default)
            {
                entity.UpdateTime=DateTime.Now;
            }
        }
    }
}