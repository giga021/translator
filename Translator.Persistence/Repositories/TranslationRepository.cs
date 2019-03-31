using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Translator.Domain.Entities;
using Translator.Domain.Repositories;

namespace Translator.Persistence.Repositories
{
	public class TranslationRepository : ITranslationRepository
	{
		private readonly string _connectionString;

		public TranslationRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task<Translation> FindAsync(string query, string fromLanguage, string toLanguage)
		{
			string sql = @"SELECT Data.query('for $t in /translations/translation			
				where lower-case($t/from[1]) = lower-case(sql:variable(""@from""))
				return $t') from Translations 
				WHERE FromLanguage=@fromLanguage AND ToLanguage=@toLanguage";
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.Add(new SqlParameter("from", query));
					cmd.Parameters.Add(new SqlParameter("fromLanguage", fromLanguage.ToUpper()));
					cmd.Parameters.Add(new SqlParameter("toLanguage", toLanguage.ToUpper()));
					using (var reader = await cmd.ExecuteXmlReaderAsync())
					{
						var serializer = new XmlSerializer(typeof(Translation));
						var translation = (Translation)serializer.Deserialize(reader);
						return translation;
					}
				}
			}
		}

		public async Task AddAsync(Translation translation)
		{
			string sqlLanguagesExist = "SELECT COUNT(*) FROM Translations WHERE FromLanguage=@fromLanguage AND ToLanguage=@toLanguage";
			string sqlNextId = @"SELECT NEXT VALUE FOR id_seq";
			string sqlInsert = @"INSERT INTO Translations(FromLanguage, ToLanguage, Data) VALUES (@fromLanguage, @toLanguage, '<translations></translations>');";
			string sqlUpdate = @"UPDATE Translations set Data.modify('insert sql:variable(""@translation"") into (/translations[1])') 
				WHERE FromLanguage=@fromLanguage AND ToLanguage=@toLanguage";

			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var cmd = new SqlCommand(sqlLanguagesExist, connection))
				{
					cmd.Parameters.AddWithValue("@fromLanguage", translation.FromLanguage.ToUpper());
					cmd.Parameters.AddWithValue("@toLanguage", translation.ToLanguage.ToUpper());
					bool languagesExist = ((int)await cmd.ExecuteScalarAsync()) > 0;

					cmd.CommandText = sqlNextId;
					var id = (long)await cmd.ExecuteScalarAsync();
					translation.Id = id;

					if (!languagesExist)
					{
						cmd.CommandText = sqlInsert;
						await cmd.ExecuteNonQueryAsync();
					}

					cmd.CommandText = sqlUpdate;
					using (var writer = new StringWriter())
					{
						var serializer = new XmlSerializer(typeof(Translation));
						var xns = new XmlSerializerNamespaces();
						xns.Add(string.Empty, string.Empty);
						serializer.Serialize(writer, translation, xns);
						var xml = writer.ToString();
						cmd.Parameters.Add(new SqlParameter("@translation", System.Data.SqlDbType.Xml) { Value = xml });
					}
					await cmd.ExecuteNonQueryAsync();
				}
			}
		}
	}
}
