using System;
using System.Runtime.Serialization;

namespace Example.Core.Service.Configuration;

[DataContract]
public record PostgreSqlConnectionStringParameters
{
	/// <summary>Возвращает или задаёт хост, на котором расположен сервер базы данных.</summary>
	/// <value>Хост, на котором расположен сервер базы данных.</value>
	[DataMember]
	public string Host { get; init; } = "localhost";

	/// <summary>Возвращает или задаёт имя базы данных.</summary>
	/// <value>Имя базы данных.</value>
	[DataMember]
	public string DatabaseName { get; init; } = "example";

	/// <summary>Возвращает или задаёт порт, на котором расположен сервер базы данных.</summary>
	/// <value>Порт, на котором расположен сервер базы данных.</value>
	[DataMember]
	public int Port { get; init; } = 5435;

	/// <summary>Возвращает или задаёт имя пользователя для подключения к базе данных.</summary>
	/// <value>Имя пользователя для подключения к базе данных.</value>
	[DataMember]
	public string Username { get; init; } = "admin";

	/// <summary>Возвращает или задаёт пароль пользователя для подключения к базе данных.</summary>
	/// <value>Пароль пользователя для подключения к базе данных.</value>
	[DataMember]
	public string Password { get; init; } = "admin";

	/// <summary>Возвращает или задаёт время ожидания подключения к базе данных.</summary>
	/// <value>Время ожидания подключения к базе данных.</value>
	[DataMember]
	public TimeSpan ConnectTimeout { get; init; } = TimeSpan.FromSeconds(15);

	/// <summary>Возвращает или задаёт время ожидания выполнения команды.</summary>
	/// <value>Время ожидания выполнения команды.</value>
	[DataMember]
	public TimeSpan CommandTimeout { get; init; } = TimeSpan.FromSeconds(30);

	/// <summary>Возвращает или задаёт время ожидания выполнения асинхронного запроса.</summary>
	/// <value>Время ожидания выполнения асинхронного запроса.</value>
	[DataMember]
	public TimeSpan CancellationTimeout { get; init; } = TimeSpan.FromSeconds(5);
}
