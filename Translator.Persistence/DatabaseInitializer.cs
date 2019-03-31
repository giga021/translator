using System.Data.SqlClient;

namespace Translator.Persistence
{
	public class DatabaseInitializer
	{
		public static void Init(string connectionString)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				try
				{
					connection.Open();
				}
				catch (SqlException)
				{
					var builder = new SqlConnectionStringBuilder(connectionString);
					string database = builder.InitialCatalog;
					var createCommand = GetCreateDbCommand(database);
					builder.InitialCatalog = string.Empty;
					var globalConnectionString = builder.ConnectionString;
					connection.ConnectionString = globalConnectionString;
					connection.Open();
					using (var cmd = new SqlCommand(createCommand, connection))
					{
						cmd.ExecuteNonQuery();

						connection.ChangeDatabase(database);

						cmd.CommandText = _createDbMembers;
						cmd.ExecuteNonQuery();
					}
				}
			}
		}

		private static string GetCreateDbCommand(string database)
		{
			return $"CREATE DATABASE [{database}];";
		}

		private static string _createDbMembers = @"
				CREATE SEQUENCE [dbo].[id_seq] AS bigint START WITH 1;
				CREATE TABLE [dbo].[Translations] (
				  [FromLanguage] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
				  [ToLanguage] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
				  [Data] xml NOT NULL,
				  CONSTRAINT [PK__Translat__1A0777BC943048A9] PRIMARY KEY CLUSTERED ([FromLanguage], [ToLanguage])
				WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
				ON [PRIMARY]
				)  
				ON [PRIMARY]
				TEXTIMAGE_ON [PRIMARY];";
	}
}
