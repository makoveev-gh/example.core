using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

using Example.Core.Contracts.Data;
using Example.Core.Data;

namespace Example.Core.Service.Api.v1;

public static class DateApi
{
	/// <summary>Url.</summary>
	private static readonly string Url = "api/v1";

	public static WebApplication MapDatesEndpoints(this WebApplication app)
	{
		app.MapAddDate()
			.MapGetIntervals();
		return app;
	}

	#region Dates

	#region GET GetIntervals

	/// <summary>Возвращает интервалы дат с одинаковым Id.</summary>
	/// <param name="app">Веб приложение.</param>
	/// <returns>Веб приложение.</returns>
	private static WebApplication MapGetIntervals(this WebApplication app)
	{
		app.MapGet($"{Url}/intervals", GetIntervalsAsync).WithOpenApi(operation =>
		{
			operation.Responses.Add("500", new OpenApiResponse() { Description = "Данные получить не удалось." });
			operation.Responses["200"].Description = "Данные успешно получены.";
			operation.Summary = "Возвращает интервалы дат с одинаковым Id.";
			operation.Tags = [new OpenApiTag() { Name = "Dates" }];
			return operation;
		});

		return app;
	}

	/// <summary>Возвращает интервалы дат с одинаковым Id.</summary>
	/// <returns>Результат по возвращению интервалов дат с одинаковым Id.</returns>
	private static async Task<IResult> GetIntervalsAsync(
		[FromServices] IDatesManager dateManager)
	{
		try
		{
			var result = await dateManager.GetIntervals();
			return TypedResults.Json(result);
		}
		catch(Exception ex)
		{
			return TypedResults.Problem(ex.Message);
		}
	}

	#endregion

	#region POST AddDate

	/// <summary>Добавляет данные клиента.</summary>
	/// <param name="app">Веб приложение.</param>
	/// <returns>Веб приложение.</returns>
	private static WebApplication MapAddDate(this WebApplication app)
	{
		app.MapPost($"{Url}/dates", AddDateAsync).WithOpenApi(operation =>
		{
			operation.Responses.Add("500", new OpenApiResponse() { Description = "Данные добавить не удалось." });
			operation.Responses["200"].Description = "Данные успешно добавлены.";
			operation.Summary = "Добавляет данные.";
			operation.Tags = [new OpenApiTag() { Name = "Dates" }];
			return operation;
		});

		return app;
	}

	/// <summary>Добавляет данные.</summary>
	private static async Task<IResult> AddDateAsync(
		[FromServices] IDatesManager dateManager,
		[FromBody] List<DateData> data)
	{
		try
		{
			await dateManager.AddDates(data);
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
