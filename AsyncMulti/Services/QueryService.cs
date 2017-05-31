using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AsyncMulti.Services {

	public class Staging {
		public int Id { get; set; }
		public int ProductUrn { get; set; }
		public string Sku { get; set; }
		public string ProductModel { get; set; }
		public string Size { get; set; }
		public int Stock { get; set; }
		public int WeekInYear { get; set; }
		public int Year { get; set; }
	}

	public class StagingOne {

		public int Id { get; set; }
		public int ProductUrn { get; set; }
		public string Sku { get; set; }
		public string ProductModel { get; set; }
	}



	public class QueryService<T> where T: class {

		public string ConnectionString { get; set; }

		public QueryService() {
			ConnectionString = "Server=net8;Database=Extranet_POZU;Trusted_Connection=Yes;MultipleActiveResultSets=true;Connection Timeout=1200;";
		}

		public async Task<List<T>> GetResults() {

			var retVal = new List<T>();

			using (var sqlConnection = new SqlConnection(ConnectionString)) {
				sqlConnection.Open();

				var command = "select * from dbo.tblStockListStaging";
				using (var sqlCommand = new SqlCommand(command, sqlConnection)) {
					using (var sqlReader = await sqlCommand.ExecuteReaderAsync()) {
						while (sqlReader.Read()) {
							switch (typeof(T).Name) {
								case "Staging":
									retVal.Add(new Staging() {
										Id = Convert.ToInt32(sqlReader["Id"]),
										ProductModel = sqlReader["ProductModel"].ToString(),
										ProductUrn = Convert.ToInt32(sqlReader["ProductURN"]),
										Size = sqlReader["Size"].ToString(),
										Sku = sqlReader["Sku"].ToString(),
										Stock = Convert.ToInt32(sqlReader["Stock"]),
										WeekInYear = Convert.ToInt32(sqlReader["WeekInYear"]),
										Year = Convert.ToInt32(sqlReader["Year"]),
									} as T);
									break;
								case "StagingOne":
									retVal.Add(new StagingOne() {
										Id = Convert.ToInt32(sqlReader["Id"]),
										ProductModel = sqlReader["ProductModel"].ToString(),
										ProductUrn = Convert.ToInt32(sqlReader["ProductURN"]),
										Sku = sqlReader["Sku"].ToString()
									} as T);
									break;
							}
						}

					}
				}
			}
			Thread.Sleep(5000);
			return retVal;
		}
	}
}