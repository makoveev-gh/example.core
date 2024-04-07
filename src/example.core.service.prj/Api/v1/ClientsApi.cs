using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;

using Example.Core.Data;
using Example.Core.Contracts.Data;

namespace Example.Core.Service.Api.v1;


public static class ClientsApi
{
	/// <summary>Url.</summary>
	private static readonly string Url = "api/v1";

	public static WebApplication MapClientsEndpoints(this WebApplication app)
	{
		app.MapGetClients()
			.MapGetClientWithManyContacts()
			.MapAddClient();
		return app;
	}

	#region Clients

	#region GET GetClients

	/// <summary>Возвращает наименование клиентов и кол-во контактов клиентов.</summary>
	/// <param name="app">Веб приложение.</param>
	/// <returns>Веб приложение.</returns>
	private static WebApplication MapGetClients(this WebApplication app)
	{
		app.MapGet($"{Url}/clients", GetClientsAsync).WithOpenApi(operation =>
		{
			operation.Responses.Add("500", new OpenApiResponse() { Description = "Данные получить не удалось." });
			operation.Responses["200"].Description = "Данные успешно получены.";
			operation.Summary = "Возвращает наименование клиентов и кол-во контактов клиентов.";
			operation.Tags = [new OpenApiTag() { Name = "Clients" }];
			return operation;
		});

		return app;
	}

	/// <summary>Возвращает наименование клиентов и кол-во контактов клиентов.</summary>
	/// <returns>Результат по возвращению наименований клиентов и кол-во контактов клиентов.</returns>
	private static async Task<IResult> GetClientsAsync(
		[FromServices] IClientsManager clientManager)
	{
		try
		{
			var result = await clientManager.GetClients();
			return TypedResults.Json(result);
		}
		catch(Exception ex)
		{
			return TypedResults.Problem(ex.Message);
		}
	}

	#endregion

	#region GET GetClientWithManyContacts

	/// <summary>Возвращает список клиентов, у которых есть более 2 контактов.</summary>
	/// <param name="app">Веб приложение.</param>
	/// <returns>Веб приложение.</returns>
	private static WebApplication MapGetClientWithManyContacts(this WebApplication app)
	{
		app.MapGet($"{Url}/clientsWithManyContacts", GetClientWithManyContactsAsync).WithOpenApi(operation =>
		{
			operation.Responses.Add("500", new OpenApiResponse() { Description = "Данные получить не удалось." });
			operation.Responses["200"].Description = "Данные успешно получены.";
			operation.Summary = "Возвращает список клиентов, у которых есть более 2 контактов.";
			operation.Tags = [new OpenApiTag() { Name = "Clients" }];
			return operation;
		});

		return app;
	}

	/// <summary>Возвращает список клиентов, у которых есть более 2 контактов.</summary>
	/// <returns>Результат по возвращению списка клиентов, у которых есть более 2 контактов.</returns>
	private static async Task<IResult> GetClientWithManyContactsAsync(
		[FromServices] IClientsManager clientManager)
	{
		try
		{
			var result = await clientManager.GetClientWithManyContacts();
			return TypedResults.Json(result);
		}
		catch(Exception ex)
		{
			return TypedResults.Problem(ex.Message);
		}
	}

	#endregion

	#region POST AddClient

	/// <summary>Добавляет данные клиента.</summary>
	/// <param name="app">Веб приложение.</param>
	/// <returns>Веб приложение.</returns>
	private static WebApplication MapAddClient(this WebApplication app)
	{
		app.MapPost($"{Url}/clients", AddClientAsync).WithOpenApi(operation =>
		{
			operation.Responses.Add("500", new OpenApiResponse() { Description = "Данные добавить не удалось." });
			operation.Responses["200"].Description = "Данные успешно добавлены.";
			operation.Summary = "Добавляет данные клиента.";
			operation.Tags = [new OpenApiTag() { Name = "Clients" }];
			return operation;
		});

		return app;
	}

	/// <summary>Добавляет данные клиента.</summary>
	private static async Task<IResult> AddClientAsync(
		[FromServices] IClientsManager clientManager,
		[FromBody] ClientData data)
	{
		try
		{
			await clientManager.AddClient(data);
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

