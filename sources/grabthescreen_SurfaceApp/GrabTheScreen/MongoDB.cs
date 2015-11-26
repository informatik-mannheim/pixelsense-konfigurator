using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using GrabTheScreen;


namespace GrabTheScreen
{
    class MongoDB
    {
        private static String CONNECTION_STRING = "mongodb://141.19.142.50:27017";

        public static void save(Auto auto)
        {
            Auto temp = auto;
            var client = new MongoClient(CONNECTION_STRING);
            var server = client.GetServer();
            var gts = server.GetDatabase("gts");
            var pictures = gts.GetCollection<BsonDocument>("pictures");
            var autoCollection = gts.GetCollection<Auto>("auto");

            try
            {
                pictures.Save(new Auto { model = temp.getModel(), modelDescription = temp.getModelDescription(), price = temp.getPrice(), source = temp.getSource(), id = temp.getId(), color = temp.getColor(), status = temp.getStatus() });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
