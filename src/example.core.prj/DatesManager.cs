using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

using Example.Core.DataStorage;
using Example.Core.Datastorage.Entities;
using Example.Core.Contracts.Data;
using Example.Core.Data;

namespace Example.Core;

public class DatesManager : IDatesManager
{
	#region Data

	private static readonly Serilog.ILogger Log = Serilog.Log.ForContext<DatesManager>();
	private readonly IApplicationDbProvider _applicationDbProvider;

	#endregion

	#region ctor

	/// <summary>Создать <see cref="DatesManager"/>.</summary>
	/// <param name="applicationDbProvider">Провайдер базы данных.</param>
	public DatesManager(
		IApplicationDbProvider applicationDbProvider)
	{
		if(applicationDbProvider is null) throw new ArgumentNullException(nameof(applicationDbProvider));

		_applicationDbProvider = applicationDbProvider;
	}
	#endregion
	/// <inheritdoc/>>
	public async Task<List<IntervalsDate>> GetIntervals(
		CancellationToken cancellationToken = default)
	{
		using var dbContext = await _applicationDbProvider.GetAsync(cancellationToken);
		return await GetIntervalsAsync(dbContext, cancellationToken);
	}
	/// <inheritdoc/>>
	public async Task AddDates(
		List<DateData> datesList,
		CancellationToken cancellationToken = default)
	{
		using var dbContext = await _applicationDbProvider.GetAsync(cancellationToken);
		await AddDatesAsync(datesList, dbContext, cancellationToken);
	}
	#region private

	private async Task<List<IntervalsDate>> GetIntervalsAsync(
		ApplicationContext dbContext,
		CancellationToken cancellationToken = default)
	{
		if(dbContext is null) throw new ArgumentNullException(nameof(dbContext));
		var result = new List<IntervalsDate>();

		var query = dbContext
			.Dates
			.OrderBy(x => x.Key)
			.ThenBy(x => x.Date)
			.GroupBy(x => x.Key);

        foreach (var group in query)
        {
			var groupList = group.ToList();
			for(int i = 0; i < groupList.Count - 1; i++)
			{
				var startDate = groupList[i].Date;
				var endDate = groupList[i + 1].Date;
				result.Add(new IntervalsDate { Id = groupList[i].Key, StartDate = startDate, EndDate = endDate });
			}
		}
        return result;

		/* For pgAdmin
		 WITH start_dates AS (
			SELECT
				"Key",
				"Date" AS start_date,
				ROW_NUMBER() OVER (PARTITION BY "Key" ORDER BY "Date") AS rn
			FROM
				"Dates"
		),
		end_dates AS (
			SELECT
				"Key",
				"Date" AS end_date,
				ROW_NUMBER() OVER (PARTITION BY "Key" ORDER BY "Date") AS rn
			FROM
				"Dates"
		)
		SELECT
			start_dates."Key",
			start_dates.start_date,
			end_dates.end_date
		FROM
			start_dates
		JOIN
			end_dates ON start_dates."Key" = end_dates."Key" AND start_dates.rn = end_dates.rn - 1
		ORDER BY
			start_dates."Key",
			start_dates.start_date;
		 */
	}

	private async Task AddDatesAsync(
		List<DateData> datesList,
		ApplicationContext dbContext,
		CancellationToken cancellationToken)
	{
		if(datesList is null) throw new ArgumentNullException(nameof(datesList));
		if(dbContext is null) throw new ArgumentNullException(nameof(dbContext));

		var dates = new List<Dates>();
		datesList.ForEach(x => dates.Add(new Dates { Date = x.Date, Key = x.Id}));

		await dbContext
			.Dates
			.AddRangeAsync(dates, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	#endregion
}
