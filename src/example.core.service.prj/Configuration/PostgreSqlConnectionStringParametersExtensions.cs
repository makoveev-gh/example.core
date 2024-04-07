using Npgsql;

namespace Example.Core.Service.Configuration;

/// <summary>Содержит расширение для <see cref="PostgreSqlConnectionStringParameters"/>.</summary>
public static class PostgreSqlConnectionStringParametersExtensions
{
	/// <summary>Позволяет преобразовать <paramref name="parameters"/> в строчку подключения к СУБД PostgreSQL.</summary>
	/// <param name="parameters">Параметры подключения к СУБД PostgreSQL.</param>
	/// <returns>Строчка подключения к СУБД PostgreSQL.</returns>
	public static string ToConnectionString(this PostgreSqlConnectionStringParameters parameters)
	{
		var connectionStringBuilder = new NpgsqlConnectionStringBuilder
		{
			Database            = parameters.DatabaseName,
			Host                = parameters.Host,
			Port                = parameters.Port,
			Username            = parameters.Username,
			Password            = parameters.Password,
			Timeout             = (int)parameters.ConnectTimeout.TotalSeconds,
			CommandTimeout      = (int)parameters.CommandTimeout.TotalSeconds,
			CancellationTimeout = (int)parameters.CancellationTimeout.TotalMilliseconds,
			IncludeErrorDetail  = true
		};

		return connectionStringBuilder.ConnectionString;
	}
}
