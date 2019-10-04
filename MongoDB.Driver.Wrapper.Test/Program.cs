using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Wrapper.Serializer;
using System;
using System.Collections.Generic;

namespace MongoDB.Driver.Wrapper.Test
{
    class Program
    {

        class TestEntityLong : MongoTableEntity<long>
        {
            public override string Table => "t1";
            public string Name { get; set; }
            [BsonSerializer(typeof(MongoSerializerDateTimeOriginal))]
            public DateTime Date { get; set; }
        }

        class TestEntityInt : MongoTableEntity<string>
        {
            public override string Table => "t2";
            public string Name { get; set; }
            public DateTime Date { get; set; }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("wrapper test...");

            var context = new MongoContext(new MongoConfiguration
            {
                Server = "mongodb://root:4YFwL8wPQE3lkaHO@mongodb-production-optio-ai.cluster-cb30ga5hpqdp.eu-west-1.docdb.amazonaws.com:27017",
                Database = "TestDB"
            });

            var tablelong = context.Table<TestEntityLong>();
            var objectlong = new TestEntityLong { Name = "ola", Date = new DateTime(2019, 01, 01) };
            var datalong = new List<TestEntityLong> { };

            tablelong.SaveEntityLong(objectlong);
            tablelong.DeleteEntity(objectlong.Code);

            tablelong.SaveEntityLongAsync(objectlong).Wait();

            objectlong.Name = "hey";
            tablelong.SaveEntityLong(objectlong);
            tablelong.DeleteEntityAsync(objectlong.Code).Wait();

            tablelong.SaveEntityLongAsync(objectlong).Wait();           

            datalong = tablelong.Load(Builders<TestEntityLong>.Filter.Where(i => i.Name == objectlong.Name));
            datalong = new List<TestEntityLong> { };
            datalong = new List<TestEntityLong> { tablelong.Get(Builders<TestEntityLong>.Filter.Where(i => i.Name == objectlong.Name)) };
            datalong = new List<TestEntityLong> { };
            datalong = tablelong.LoadEntity(new List<long> { objectlong.Code });
            datalong = new List<TestEntityLong> { };
            datalong = new List<TestEntityLong> { tablelong.GetEntity(objectlong.Code) };

            Console.WriteLine("all wrapper tests done...");
            Console.ReadKey();
        }
    }
}
