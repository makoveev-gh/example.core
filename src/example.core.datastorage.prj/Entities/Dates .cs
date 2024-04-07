using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Example.Core.Datastorage.Entities;
public class Dates
{
	#region Helpers

	private sealed class Configuration : IEntityTypeConfiguration<Dates>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<Dates> builder)
		{
			if(builder is null) throw new ArgumentNullException(nameof(builder));

			builder.ToTable("Dates");

			builder
				.Property(e => e.Id)
				.HasColumnName("Id")
				.ValueGeneratedOnAdd()
				.IsRequired();

			builder
				.Property(e => e.Key)
				.HasColumnName("Key")
				.IsRequired();

			builder
				.Property(e => e.Date)
				.HasColumnName("Date")
				.HasColumnType("date")
				.IsRequired();
		}
	}

	#endregion

	#region Statics

	public static new IEntityTypeConfiguration<Dates> GetConfiguration() => new Configuration();

	#endregion

	#region Properties

	public int Id { get; set; }
	public required int Key { get; set; }

	public required DateOnly Date { get; set; }

	#endregion

	#region ctor

	public Dates()
	{
	}

	#endregion
}
