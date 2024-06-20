using JsonProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JsonProject.Domain.Common.ValueObjects;

namespace JsonProject.Presentation.Configurations;

/// <summary>
/// Represents the configuration for the <see cref="Message"/> entity.
/// </summary>
internal sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("messages");
        
        builder.HasKey(p => p.Id).HasName("id");
        
        builder.HasIndex(x => x.Id)
            .HasDatabaseName("IdMessageIndex")
            .IsUnique();
        
        builder.HasKey(post => post.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        builder.OwnsOne(message => message.Description, description =>
        {
            description.WithOwner();

            description.Property(name => name.Value)
                .HasColumnName(nameof(Message.Description))
                .HasMaxLength(Name.MaxLength)
                .IsRequired();
        });

        builder.Property(post => post.CreatedOnUtc).IsRequired();
    }
}