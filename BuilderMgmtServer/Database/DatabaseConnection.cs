//using MongoDB.Driver;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace builder_mgmt_server.Database
//{
//	public static class DatabaseConnection
//	{
//		public static IMongoDatabase GetDatabaseConnection()
//		{
//			//var connectionString = "mongodb://localhost";

//			var connectionString = "mongodb+srv://builder-mgmt:Builder007@cluster0.dtw7t.mongodb.net/myFirstDatabase?retryWrites=true&w=majority";

//			var client = new MongoClient(connectionString);
//			//IMongoServer server = client. GetServer();

//			IMongoDatabase database = client.GetDatabase("builder-mgmt");
//			return database;
//		}
//	}
//}
