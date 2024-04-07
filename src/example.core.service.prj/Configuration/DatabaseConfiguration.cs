using System.Runtime.Serialization;

namespace Example.Core.Service.Configuration;

[DataContract]
public class DatabaseConfiguration
{
	/// <summary>
	/// Возвращает или задаёт параметры подключения к базе данных PostgreSQL.
	/// </summary>
	/// <value>
	/// Параметры подключения к базе данных PostgreSQL.
	/// </value>
	[DataMember]
	public PostgreSqlConnectionStringParameters PostgreSql { get; set; } = new();
}
