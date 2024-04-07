using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Example.Core.Datastorage.Entities;

namespace Example.Core.DataStorage;

public class Client
{
	#region Helpers

	private sealed class Configuration : IEntityTypeConfiguration<Client>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<Client> builder)
		{
			if(builder is null) throw new ArgumentNullException(nameof(builder));

			builder.ToTable("Clients");

			builder
				.Property(e => e.Id)
				.HasColumnName("Id")
				.HasColumnType("integer")
				.ValueGeneratedOnAdd()
				.IsRequired();

			builder
				.Property(e => e.ClientName)
				.HasColumnName("ClientName")
				.HasMaxLength(200)
				.IsRequired();

			builder
				.HasMany(e => e.ClientsContacts)
				.WithOne(e => e.Client)
				.HasForeignKey(e => e.ClientId)
				.IsRequired();
		}
	}

	#endregion

	#region Statics

	public static new IEntityTypeConfiguration<Client> GetConfiguration() => new Configuration();

	#endregion

	#region Properties

	public int Id { get; set; }

	public required string ClientName { get; set; }

	public List<ClientContact> ClientsContacts { get; set; }

	#endregion

	#region ctor
	public Client()
	{
	}

	#endregion
}
