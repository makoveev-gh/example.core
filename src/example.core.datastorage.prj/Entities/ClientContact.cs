using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Example.Core.DataStorage;

namespace Example.Core.Datastorage.Entities;
public class ClientContact
{
	#region Helpers

	private sealed class Configuration : IEntityTypeConfiguration<ClientContact>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<ClientContact> builder)
		{
			if(builder is null) throw new ArgumentNullException(nameof(builder));

			builder.ToTable("ClientContacts");

			builder
				.Property(e => e.Id)
				.HasColumnName("Id")
				.ValueGeneratedOnAdd()
				.IsRequired();

			builder
				.Property(e => e.ClientId)
				.HasColumnName("ClientId")
				.IsRequired();

			builder
				.Property(e => e.ContactType)
				.HasColumnName("ContactType")
				.HasMaxLength(255)
				.IsRequired();

			builder
				.Property(e => e.ContactValue)
				.HasColumnName("ContactValue")
				.HasMaxLength(255)
				.IsRequired();
		}
	}

	#endregion

	#region Statics
	public static new IEntityTypeConfiguration<ClientContact> GetConfiguration() => new Configuration();

	#endregion

	#region Properties

	public int Id { get; set; }

	public required int ClientId { get; set; }

	public required string ContactType { get; set; }

	public required string ContactValue { get; set; }

	public Client Client { get; set; }

	#endregion

	#region ctor

	public ClientContact()
	{
	}

	#endregion
}
