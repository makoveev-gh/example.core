using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

using Example.Core.Contracts.Data;

namespace Example.Core.Data;

public interface IClientsManager
{
	/// <summary>
	/// Возвращает список клинетов.
	/// </summary>
	/// <param name="cancellationToken">Токен.</param>
	/// <returns>Список клинетов.</returns>
	public Task<List<ClientWithContactData>> GetClients(CancellationToken cancellationToken = default);

	/// <summary>
	/// Возвращает список клиентов имеющих более двух заказов.
	/// </summary>
	/// <param name="cancellationToken">Токен.</param>
	/// <returns>Список клиентов имеющих более двух заказов.</returns>
	public Task<List<ClientWithContactData>> GetClientWithManyContacts(CancellationToken cancellationToken = default);

	/// <summary>
	/// Заносит клиентов в базу данных.
	/// </summary>
	/// <param name="clientData">Данные клиента.</param>
	/// <param name="cancellationToken">Токен.</param>
	public Task AddClient(ClientData clientData, CancellationToken cancellationToken = default);
}
