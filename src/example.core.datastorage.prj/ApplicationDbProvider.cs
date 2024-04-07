using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace Example.Core.DataStorage;

/// <inheritdoc cref="IApplicationDbProvider"/>
/// <summary>
/// Создаёт экземпляр <see cref="ApplicationDbProvider"/>.
/// </summary>
/// <param name="serviceProvider">Провайдер сервисов.</param>
public class ApplicationDbProvider(IServiceProvider serviceProvider) : IApplicationDbProvider
{
	private static readonly ILogger Log = Serilog.Log.ForContext<ApplicationDbProvider>();
	private readonly IServiceProvider _serviceProvider = serviceProvider;
	private readonly SemaphoreSlim _locker = new(1, 1);

	/// <summary>Проинициализирована ли база данных</summary>
	public bool IsInit { get; private set; }

	public async ValueTask<ApplicationContext> GetAsync(CancellationToken cancellationToken = default)
	{
		var serviceScope = _serviceProvider.CreateAsyncScope();
		var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationContext>();
		if(IsInit) return dbContext;

		await LockAsync(async () =>
		{
			if(!IsInit)
			{
				Log.Information("Begin database initialization");

				await dbContext.Database.MigrateAsync(cancellationToken);

				Log.Information("Database initialized");
				IsInit = true;
			}
		}, cancellationToken);

		return dbContext;
	}

	private async Task LockAsync(Func<Task> action, CancellationToken token)
	{
		await _locker.WaitAsync(default(CancellationToken));
		try
		{
			await action();
		}
		finally
		{
			_locker.Release();
		}
	}
}
