using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

using Example.Core.Contracts;
using Example.Core.Contracts.Data;
using Example.Core.DataStorage;

namespace Example.Core.Data;
public interface ITableManager
{
	/// <summary>
	/// Возвращает заданное кол-во объектов.
	/// </summary>
	/// <param name="start">Начало получения списка.</param>
	/// <param name="count">Кол-во объектов.</param>
	/// <param name="cancellationToken">Токен.</param>
	/// <returns>Заданное кол-во объектов.</returns>
	public Task<List<Table>> GetRangeTable(int? start = 0, int? count = 9, Filters filters = Filters.Code, CancellationToken cancellationToken = default);

	/// <summary>
	/// Очищает таблицу, добавляет объеты.
	/// </summary>
	/// <param name="tableList">Добавляемые объекты.</param>
	/// <param name="cancellationToken">Токен.</param>
	public Task AddTables(List<TableContract> tableList, CancellationToken cancellationToken = default);

	/// <summary>
	/// Возвращает кол-во объектов в БД.
	/// </summary>
	/// <param name="cancellationToken">Токен.</param>
	public Task<int> GetCount(CancellationToken cancellationToken = default);
}