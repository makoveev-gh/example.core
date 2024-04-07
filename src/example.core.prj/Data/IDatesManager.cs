using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using Example.Core.Contracts.Data;

namespace Example.Core.Data;

public interface IDatesManager
{
	/// <summary>
	/// Возвращает интервалы.
	/// </summary>
	/// <param name="cancellationToken">Токен.</param>
	/// <returns>Интервалы</returns>
	public Task<List<IntervalsDate>> GetIntervals(CancellationToken cancellationToken = default);

	/// <summary>
	/// Добавляет даты.
	/// </summary>
	/// <param name="datesList">Даты.</param>
	/// <param name="cancellationToken">Токен.</param>
	public Task AddDates(List<DateData> datesList, CancellationToken cancellationToken = default);
}