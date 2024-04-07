using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Example.Core.DataStorage;

public class Table 
{
	#region Helpers

	private sealed class Configuration : IEntityTypeConfiguration<Table>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<Table> builder)
		{
			if(builder is null) throw new ArgumentNullException(nameof(builder));

			builder.ToTable("Table");

			builder
				.HasKey(e => e.Id);

			builder
				.Property(e => e.Code)
				.HasColumnName("Code")
				.IsRequired();

			builder
				.Property(e => e.Value)
				.HasColumnName("Value");

		}
	}

	#endregion

	#region Statics

	public static new IEntityTypeConfiguration<Table> GetConfiguration() => new Configuration();

	#endregion

	#region Properties
	public int Id { get; set; }

	public required int Code { get; set; }

	public string Value { get; set; }

	#endregion

	#region ctor

	public Table()
	{
	}

	#endregion
}
