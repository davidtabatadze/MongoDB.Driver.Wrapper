using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Wrapper.Serializer;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MongoDB.Driver.Wrapper.Test
{
    class Program
    {

        class TestEntityLong : MongoTableEntity<long>
        {
            public override string Table => "t1";
            public string Name { get; set; }
            [BsonSerializer(typeof(MongoSerializerDateTimeLocal))]
            public DateTime Date { get; set; }
            [BsonSerializer(typeof(MongoSerializerDateTimeLocalNullable))]
            public DateTime? Date1 { get; set; }
        }

        class TestEntityInt : MongoTableEntity<string>
        {
            public override string Table => "t2";
            public string Name { get; set; }
            public DateTime Date { get; set; }
        }

        static void Main(string[] args)
        {

            var longs = new List<long> { };
            DateTime dtdt = new DateTime(1988, 11, 11);
            for (int i = 0; i < 1000; i++)
            {

                //var ddd = ((DateTime.Now.ToUniversalTime() - dtdt).TotalMilliseconds + 0.5);

                //longs.Add(Convert.ToInt64(ddd));


                string ticks = DateTime.Now.Ticks.ToString();
                //ticks = ticks.Substring(ticks.Length - 14);
                longs.Add(Convert.ToInt64(ticks));

                //longs.Add(Convert.ToInt64(DateTime.Now.ToString("yyMMddHHmmssff")));
            }
            longs = longs.Distinct().ToList();

            var x = 100;

            // ??? სტრიქონ-გასაღებებზე აუცილებელია ToLower() ???

            Console.WriteLine("wrapper test...");

            var context = new MongoContext(new MongoConfiguration
            {
                Server = "mongodb://root:4YFwL8wPQE3lkaHO@mongodb-production-optio-ai.cluster-cb30ga5hpqdp.eu-west-1.docdb.amazonaws.com:27017",
                Database = "TestDB"
            });

            var tablelong = context.Table<TestEntityLong>();
            var objectlong = new TestEntityLong { Name = "ola", Date = new DateTime(2019, 1, 1) };
            var datalong = new List<TestEntityLong> { };

            tablelong.SaveEntityLong(objectlong);
            tablelong.DeleteEntity(objectlong.Code);

            tablelong.SaveEntityLongAsync(objectlong).Wait();

            objectlong.Name = "hey";
            objectlong.Date1 = DateTime.Now;
            tablelong.SaveEntityLong(objectlong);
            tablelong.DeleteEntityAsync(objectlong.Code).Wait();

            tablelong.SaveEntityLongAsync(objectlong).Wait();

            //datalong = tablelong.Load(Builders<TestEntityLong>.Filter.Where(i => i.Name == objectlong.Name));
            datalong = tablelong.Load(tablelong.Filter.Where(i => i.Name == objectlong.Name));
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
