using System;
using System.Threading;
using System.Threading.Tasks;
using Example.Core.Contracts;

namespace Example.Core.DataStorage;

/// <summary>Инициализатор для базы данных.</summary>
public class DatabaseInitializer : IInitializable
{
	#region Data

	private readonly IApplicationDbProvider _applicationDbProvider;

	#endregion

	#region Properties

	public bool IsInitialized { get; private set; }

	#endregion

	#region ctor

	/// <summary>Создает <see cref="DatabaseInitializer"/>.</summary>
	/// <param name="applicationDbProvider">Провайдер базы данных.</param>
	public DatabaseInitializer(IApplicationDbProvider applicationDbProvider)
	{
		if(applicationDbProvider is null) throw new ArgumentNullException(nameof(applicationDbProvider));

		_applicationDbProvider = applicationDbProvider;
	}

	#endregion

	#region Implementation IInitializable

	/// <inheritdoc/>
	public async Task InitializeAsync(CancellationToken cancellationToken)
	{
		using var dbContext = await _applicationDbProvider.GetAsync(cancellationToken);
		await dbContext.SaveChangesAsync(cancellationToken);
		IsInitialized = true;
	}

	#endregion
}
