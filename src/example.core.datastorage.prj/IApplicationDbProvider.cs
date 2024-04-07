using System.Threading;
using System.Threading.Tasks;

namespace Example.Core.DataStorage;

public interface IApplicationDbProvider
{
	/// <summary>
	/// Получает асинхронно экземпляр контекста базы.
	/// </summary>
	/// <param name="cancellationToken">Токен на отмену операции.</param>
	/// <returns>Асинхронная задача по получению экземпляра <see cref="ApplicationContext"/>.</returns>
	ValueTask<ApplicationContext> GetAsync(CancellationToken cancellationToken = default);
}
