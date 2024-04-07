using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Example.Core.Contracts.Data;
using Example.Core.Data;
using Example.Core.Datastorage.Entities;
using Example.Core.DataStorage;

namespace Example.Core;

public class ClientsManager : IClientsManager
{
	#region Data

	private static readonly Serilog.ILogger Log = Serilog.Log.ForContext<ClientsManager>();
	private readonly IApplicationDbProvider _applicationDbProvider;

	#endregion

	#region ctor

	/// <summary>Создать <see cref="ClientsManager"/>.</summary>
	/// <param name="applicationDbProvider">Провайдер базы данных.</param>
	public ClientsManager(
		IApplicationDbProvider applicationDbProvider)
	{
		if(applicationDbProvider is null) throw new ArgumentNullException(nameof(applicationDbProvider));

		_applicationDbProvider = applicationDbProvider;
	}
	#endregion

	/// <inheritdoc/>>
	public async Task<List<ClientWithContactData>> GetClients(
		CancellationToken cancellationToken = default)
	{

		using var dbContext = await _applicationDbProvider.GetAsync(cancellationToken);
		return await GetClientsAsync(dbContext, cancellationToken);
	}

	/// <inheritdoc/>>
	public async Task<List<ClientWithContactData>> GetClientWithManyContacts(
		CancellationToken cancellationToken = default)
	{
		using var dbContext = await _applicationDbProvider.GetAsync(cancellationToken);
		return await GetClientsWithManyContactsAsync(dbContext, cancellationToken);
	}

	/// <inheritdoc/>>
	public async Task AddClient(
		ClientData clientData,
		CancellationToken cancellationToken = default)
	{
		if(clientData is null) throw new ArgumentNullException(nameof(clientData));
		using var dbContext = await _applicationDbProvider.GetAsync(cancellationToken);

		var contacts = new List<ClientContact>();
		var rand = new Random();

		var client = new Client { ClientName = clientData.Name, Id = rand.Next(0, 100000000) };
		clientData.Contacts.ForEach(cont =>
		{
			contacts.Add(new ClientContact
			{
				Client = client,
				ClientId = client.Id,
				ContactType = cont.ContactType,
				ContactValue = cont.ContactValue
			});
		});

		await AddClientAsync(client, contacts, dbContext, cancellationToken);
	}
	#region private

	private async Task<List<ClientWithContactData>> GetClientsAsync(
		ApplicationContext dbContext,
		CancellationToken cancellationToken = default)
	{
		if(dbContext is null) throw new ArgumentNullException(nameof(dbContext));

		var clients = await dbContext
			.Clients
			.Include(x => x.ClientsContacts)
			.ToListAsync(cancellationToken);

		var result = new List<ClientWithContactData>();

		clients.ForEach(client =>
		{
			result.Add(new ClientWithContactData { Name = client.ClientName, ContactCount = client.ClientsContacts.Count });
		});

		return result;
	}

	private async Task<List<ClientWithContactData>> GetClientsWithManyContactsAsync(
		ApplicationContext dbContext,
		CancellationToken cancellationToken = default)
	{
		if(dbContext is null) throw new ArgumentNullException(nameof(dbContext));

		//SELECT 
		// c.Id, с.ClientName, COUNT(*) as 2
		//FROM
		// customer c
		// JOIN ClientContacts clc ON c.Id = clc.ClientId
		//GROUP BY
		// c.Id, с.ClientName
		//HAVING COUNT(*) > 2

		var clients = await dbContext
			.Clients
			.Include(x => x.ClientsContacts)
			.Where(x => x.ClientsContacts.Count > 2)
			.ToListAsync(cancellationToken);

		var result = new List<ClientWithContactData>();

		clients.ForEach(client =>
		{
			result.Add(new ClientWithContactData { Name = client.ClientName, ContactCount = client.ClientsContacts.Count });
		});

		return result;
	}

	private async Task AddClientAsync(
		Client clientData,
		List<ClientContact> contacts,
		ApplicationContext dbContext,
		CancellationToken cancellationToken)
	{
		if(clientData is null) throw new ArgumentNullException(nameof(clientData));
		if(dbContext is null) throw new ArgumentNullException(nameof(dbContext));

		await dbContext
			.Clients
			.AddAsync(clientData, cancellationToken);

		await dbContext
			.ClientContacts
			.AddRangeAsync(contacts, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	#endregion
}
