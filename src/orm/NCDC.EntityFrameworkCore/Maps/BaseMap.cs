using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCDC.EntityFrameworkCore.Entities.Base;

namespace NCDC.EntityFrameworkCore.Maps;

public abstract class BaseMap<TEntity>:IEntityTypeConfiguration<TEntity> where TEntity:BaseEntity
{
    protected abstract string TableName { get; }
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).IsRequired().HasMaxLength(50).IsUnicode(false);
        builder.Property(o => o.Version).IsConcurrencyToken().IsRequired().HasMaxLength(50).IsUnicode(false);
        builder.HasQueryFilter(o => o.IsDelete == false);
        builder.ToTable(TableName);
        Configure0(builder);
    }

    protected abstract void Configure0(EntityTypeBuilder<TEntity> builder);
}