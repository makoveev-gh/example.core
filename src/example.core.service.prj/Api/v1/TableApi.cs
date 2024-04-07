using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

using Example.Core.Data;
using Example.Core.Contracts;
using Example.Core.Contracts.Data;

namespace Example.Core.Service.Api.v1;


public static class TableApi
{
	/// <summary>Url.</summary>
	private static readonly string Url = "api/v1";

	public static WebApplication MapTableEndpoints(this WebApplication app)
	{
		app.MapGetTable()
			.MapPostTable()
			.MapGetCount();
		return app;
	}

	#region Table

	#region GET GetTable

	/// <summary>Возвращает кол-во объектов в БД.</summary>
	/// <param name="app">Веб приложение.</param>
	/// <returns>Веб приложение.</returns>
	private static WebApplication MapGetCount(this WebApplication app)
	{
		app.MapGet($"{Url}/tableCount", GetCountAsync).WithOpenApi(operation =>
		{
			operation.Responses.Add("500", new OpenApiResponse() { Description = "Данные получить не удалось." });
			operation.Responses["200"].Description = "Данные успешно получены.";
			operation.Summary = "Возвращает кол-во объектов в БД Table.";
			operation.Tags = [new OpenApiTag() { Name = "Table" }];
			return operation;
		});

		return app;
	}

	/// <summary>Возвращает кол-во объектов в БД.</summary>
	/// <returns>Результат по возвращаению кол-ва объектов в БД.</returns>
	private static async Task<IResult> GetCountAsync(
		[FromServices] ITableManager tableManager)
	{
		try
		{
			var result = await tableManager.GetCount();
			return TypedResults.Json(result);
		}
		catch(Exception ex)
		{
			return TypedResults.Problem(ex.Message);
		}
	}

	#endregion

	#region GET GetTable

	/// <summary>Возвращает данные БД Table.</summary>
	/// <param name="app">Веб приложение.</param>
	/// <returns>Веб приложение.</returns>
	private static WebApplication MapGetTable(this WebApplication app)
	{
		app.MapGet($"{Url}/table", GetTableAsync).WithOpenApi(operation =>
		{
			operation.Responses.Add("500", new OpenApiResponse() { Description = "Данные получить не удалось." });
			operation.Responses["200"].Description = "Данные успешно получены.";
			operation.Parameters[0].Description = "Индекс объекта с которого будет начинаться вывод.";
			operation.Parameters[1].Description = "Кол-во объектов.";
			operation.Parameters[2].Description = "Фильтрация, 0 - по полю Code, 1 - по полю Value.";
			operation.Summary = "Возвращает данные БД Table.";
			operation.Tags = [new OpenApiTag() { Name = "Table" }];
			return operation;
		});

		return app;
	}

	/// <summary>Возвращает данные БД Table.</summary>
	/// <returns>Результат по возвращению данных БД Table.</returns>
	private static async Task<IResult> GetTableAsync(
		[FromServices] ITableManager tableManager,
		[FromQuery] int start = 0,
		[FromQuery] int count = 9,
		[FromQuery] Filters filters = Filters.Code)
	{
		try
		{
			var result = await tableManager.GetRangeTable(start, count, filters);
			return TypedResults.Json(result);
		}
		catch(Exception ex)
		{
			return TypedResults.Problem(ex.Message);
		}
	}

	#endregion

	#region POST PostTable

	/// <summary>Добавляет данные в БД.</summary>
	/// <param name="app">Веб приложение.</param>
	/// <returns>Веб приложение.</returns>
	private static WebApplication MapPostTable(this WebApplication app)
	{
		app.MapPost($"{Url}/table", PostTableAsync).WithOpenApi(operation =>
		{
			operation.Responses.Add("500", new OpenApiResponse() { Description = "Данные добавить не удалось." });
			operation.Responses["200"].Description = "Данные добавлены.";
			operation.Summary = "Добавляет данные в БД";
			operation.Tags = [new OpenApiTag() { Name = "Table" }];
			return operation;
		});

		return app;
	}

	/// <summary>Добавляет данные в БД.</summary>
	/// <returns>Результат по добавлению данных в БД.</returns>
	private static async Task<IResult> PostTableAsync(
		[FromServices] ITableManager tableManager,
		[FromBody] List<TableContract> data)
	{
		try
		{
			await tableManager.AddTables(data);
			return TypedResults.Ok();
		}
		catch(Exception ex)
		{
			return TypedResults.Problem(ex.Message);
		}
	}

	#endregion

	#endregion
}

