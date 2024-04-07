using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Example.Core.Contracts;
using Example.Core.Contracts.Data;
using Example.Core.Data;
using Example.Core.DataStorage;

namespace Example.Core;
public class TableManager : ITableManager
{
	#region Data

	private static readonly Serilog.ILogger Log = Serilog.Log.ForContext<TableManager>();
	private readonly IApplicationDbProvider _applicationDbProvider;

	#endregion

	#region ctor

	/// <summary>Создать <see cref="TableManager"/>.</summary>
	/// <param name="applicationDbProvider">Провайдер базы данных.</param>
	public TableManager(
		IApplicationDbProvider applicationDbProvider)
	{
		if(applicationDbProvider is null) throw new ArgumentNullException(nameof(applicationDbProvider));

		_applicationDbProvider = applicationDbProvider;
	}
	#endregion

	/// <inheritdoc/>>
	public async Task<int> GetCount(
		CancellationToken cancellationToken = default)
	{
		using var dbContext = await _applicationDbProvider.GetAsync(cancellationToken);
		var count = await GetCountAsync(dbContext, cancellationToken);
		return count;
	}

	/// <inheritdoc/>>
	public async Task<List<Table>> GetRangeTable(
		int? start,
		int? count,
		Filters filters = Filters.Code,
		CancellationToken cancellationToken = default)
	{
		using var dbContext = await _applicationDbProvider.GetAsync(cancellationToken);
		var tables = await GetRangeTablesAsync(dbContext, start, count, filters, cancellationToken);
		return tables;
	}
	/// <inheritdoc/>>
	public async Task AddTables(
		List<TableContract> tableList,
		CancellationToken cancellationToken = default)
	{

		if(tableList is null) throw new ArgumentNullException(nameof(tableList));
		using var dbContext = await _applicationDbProvider.GetAsync(cancellationToken);

		var newTables = new List<Table>();
		tableList.OrderBy(x => x.Code);
		for(int i = 0; i < tableList.Count; i++)
		{
			newTables.Add(new Table { Code = tableList[i].Code, Value = tableList[i].Value, Id = i + 1 });
		}

		await AddTablesAsync(newTables, dbContext, cancellationToken);
	}
	#region private

	private async Task<List<Table>> GetRangeTablesAsync(
		ApplicationContext dbContext,
		int? start,
		int? count,
		Filters filters = Filters.Code,
		CancellationToken cancellationToken = default)
	{
		if(dbContext is null) throw new ArgumentNullException(nameof(dbContext));

		var table = filters switch
		{
			Filters.Code => await dbContext
			.Tables
			.Skip(start ?? 0)
			.Take(count ?? 9)
			.OrderBy(x => x.Code)
			.ToListAsync(cancellationToken),

			Filters.Value => await dbContext
			.Tables
			.Skip(start ?? 0)
			.Take(count ?? 9)
			.OrderBy(x => x.Value)
			.ToListAsync(cancellationToken),
			_ => default
		};

		if(table is null) throw new ArgumentNullException(nameof(table));

		return table!;
	}

	private async Task AddTablesAsync(
		List<Table> tableList,
		ApplicationContext dbContext,
		CancellationToken cancellationToken)
	{
		if(tableList is null) throw new ArgumentNullException(nameof(tableList));
		if(dbContext is null) throw new ArgumentNullException(nameof(dbContext));

		dbContext
			.Tables
			.RemoveRange(dbContext.Tables.ToList());

		await dbContext
			.Tables
			.AddRangeAsync(tableList, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	private async Task<int> GetCountAsync(
		ApplicationContext dbContext,
		CancellationToken cancellationToken)
	{
		if(dbContext is null) throw new ArgumentNullException(nameof(dbContext));

		return await dbContext
			.Tables
			.CountAsync();
	}

	#endregion
}
