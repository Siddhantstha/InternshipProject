using System;
using System.Collections.Generic;
using Npgsql;
using System.Data;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.DapperConnects
{
	public class DapperContext
	{
		private readonly string _connectionString;

		public DapperContext(IConfiguration connectionString)
		{
			_connectionString = connectionString.GetConnectionString("DefaultConnection")!;

		}

		public IDbConnection CreateConnection()
	{
			return new NpgsqlConnection(_connectionString);
		}	
	}
}