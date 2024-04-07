using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Example.Core.Contracts;
using Example.Core.Datastorage.Entities;

namespace Example.Core.DataStorage;

public class ApplicationContext : DbContext
{
	#region Statics

	/// <summary>Возвращает строчку подключения к базе данных по-умолчанию.</summary>
	/// <value>Строчка подключения к базе данных по-умолчанию.</value>
	public static string DefaultConnectionString { get; }

	#endregion

	#region Properties

	public DbSet<Table> Tables => Set<Table>();
	public DbSet<Client> Clients => Set<Client>(); 
	public DbSet<ClientContact> ClientContacts => Set<ClientContact>();
	public DbSet<Dates> Dates => Set<Dates>();



	#endregion
	#region Data
	private readonly string? _connectionString;

	#endregion

	#region ctor

	/// <summary>Инициализирует тип <see cref="ApplicationContext"/>.</summary>
	static ApplicationContext()
	{
		Directory.CreateDirectory(ProfileLocationStorage.DatabaseDirPath);
		var defaultDbPath = Path.Combine(
			ProfileLocationStorage.DatabaseDirPath,
			"example.db");

		DefaultConnectionString = $"Data Source={defaultDbPath}";
	}

	/// <summary>Создаёт экземпляр <see cref="ApplicationContext"/>.</summary>
	public ApplicationContext()
	{
		_connectionString = DefaultConnectionString;
	}

	/// <summary>Создаёт экземпляр <see cref="ApplicationContext"/>.</summary>
	/// <param name="options">Параметры создания контекст базы данных.</param>
	public ApplicationContext(DbContextOptions<ApplicationContext> options)
		: base(options)
	{
	}

	#endregion

	#region Methods

	/// <inheritdoc/>
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if(optionsBuilder is null) throw new ArgumentNullException(nameof(optionsBuilder));

		base.OnConfiguring(optionsBuilder);

		if(optionsBuilder.IsConfigured) return;

		optionsBuilder.UseNpgsql(_connectionString ?? DefaultConnectionString);
	}

	/// <inheritdoc/>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		if(modelBuilder is null) throw new ArgumentNullException(nameof(modelBuilder));

		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfiguration(Client.GetConfiguration());
		modelBuilder.ApplyConfiguration(ClientContact.GetConfiguration());
		modelBuilder.ApplyConfiguration(Table.GetConfiguration());
		modelBuilder.ApplyConfiguration(Datastorage.Entities.Dates.GetConfiguration());
		SeedData(modelBuilder);
	}

	private void SeedData(ModelBuilder modelBuilder)
	{

	}
	#endregion
}
