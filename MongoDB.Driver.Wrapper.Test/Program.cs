using System;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver.Wrapper.CRUD;
using MongoDB.Driver.Wrapper.Transaction;
using MongoDB.Bson.Serialization.Attributes;
using CoreKit.Extension.String;

namespace MongoDB.Driver.Wrapper.Test
{
    class Program
    {

        class TestShared
        {
            public string Description { get; set; }
        }

        class TestEntityString : MongoEntity<string>
        {
            public string Name { get; set; }
        }

        class TestEntityLong : TestShared, IMongoEntity<long>
        {
            public TestEntityLong()
            {
                MongoHelper.GenerateKey(this);
            }
            [BsonId]
            public long Key { get; set; }
            [BsonIgnore]
            public bool Delete { get; set; }
        }

        [BsonIgnoreExtraElements]
        class TestEntityObject : IMongoEntityKeyable<object>, IMongoEntityDeletable
        {
            public object Key { get; set; }
            public bool Delete { get; set; }
        }

        static void Log(bool ok)
        {
            if (ok) Console.WriteLine(" >>> OK");
            else Console.WriteLine(" !!! NO");
        }

        static void Main(string[] args)
        {// ??? სტრიქონ-გასაღებებზე აუცილებელია ToLower() ???

            Console.WriteLine("wrapper test...");

            try
            {
                var ctx = new MongoContext(
                    new MongoConfiguration
                    {
                        Server = "mongodb://root:12#Optio34@10.0.1.2:27017",
                        Database = "TestDB",
                        UseTransactions = true
                    },
                    new Dictionary<Type, string> {
                        { typeof(TestEntityLong), "longs" },
                        { typeof(TestEntityString), "strings" }
                    }
                );

                //
                ctx.Delete<TestEntityLong>();
                ctx.Delete<TestEntityString>();
                //
                var elong = new TestEntityLong { Key = 1 };
                var elongs = new List<TestEntityLong> { };
                var estring = new TestEntityString { Key = "1" };
                var estrings = new List<TestEntityString> { };
                //
                Console.Write("testing key");
                Log(
                    elong.Key == 1 &&
                    estring.Key == "1" &&
                    new TestEntityLong { }.Key >= 0 &&
                    new TestEntityString { }.Key.HasValue()
                );
                //
                Console.Write("testing insert");
                ctx.Insert(elong);
                elongs.Add(new TestEntityLong { Key = 2, Description = "test" });
                elongs.Add(new TestEntityLong { Key = 3, Description = "desc" });
                elongs.Add(new TestEntityLong { Key = 4, Description = "test" });
                elongs.Add(new TestEntityLong { Key = 5, Description = "desc" });
                elongs.Add(new TestEntityLong { Key = 6, Description = "test" });
                elongs.Add(new TestEntityLong { Key = 7, Description = "desc" });
                ctx.Insert(elongs);
                ctx.Insert(estring);
                estrings.Add(new TestEntityString { Key = "2", Name = "test" });
                estrings.Add(new TestEntityString { Key = "3", Name = "desc" });
                estrings.Add(new TestEntityString { Key = "4", Name = "test" });
                estrings.Add(new TestEntityString { Key = "5", Name = "desc" });
                estrings.Add(new TestEntityString { Key = "6", Name = "test" });
                estrings.Add(new TestEntityString { Key = "7", Name = "desc" });
                ctx.Insert(estrings);
                Log(
                    ctx.Load<TestEntityString>().Count == 7 &&
                    ctx.Load(Builders<TestEntityLong>.Filter.Empty).Count == 7
                );
                //
                Console.Write("testing delete");
                ctx.Delete<TestEntityLong>(0);
                ctx.Delete<TestEntityLong>(1);
                ctx.Delete<TestEntityLong>(new List<long> { 2, 3, 0, -1 });
                ctx.Delete(new TestEntityLong { Key = 4 });
                ctx.Delete(new List<TestEntityLong> {
                    new TestEntityLong { Key = 5, Description = "Description" },
                    null
                });
                ctx.Delete<TestEntityString>("0");
                ctx.Delete<TestEntityString>("1");
                ctx.Delete<TestEntityString>(new List<string> { "2", "3", "", null });
                ctx.Delete(new TestEntityString { Key = "4" });
                ctx.Delete(new List<TestEntityString> {
                    new TestEntityString { Key = "5", Name = "Name" },
                    null
                });
                Log(
                    ctx.Load<TestEntityLong>().Count == 2 &&
                    ctx.Load<TestEntityString>().Count == 2
                );
                //
                Console.Write("testing count");
                var c1 = ctx.Count<TestEntityLong>();
                var c2 = ctx.Count(Builders<TestEntityString>.Filter.Empty);
                var c3 = ctx.Count(Builders<TestEntityLong>.Filter.Where(f => f.Key > 6));
                var c4 = ctx.Count(Builders<TestEntityString>.Filter.Where(f => f.Name.Contains("es")));
                var c5 = ctx.Count(Builders<TestEntityLong>.Filter.Where(f => f.Description == "TEST"));
                var c6 = ctx.Count(Builders<TestEntityString>.Filter.Where(f => f.Name.ToLower() == "TEST".ToLower()));
                Log(
                    c1 == 2 &&
                    c2 == 2 &&
                    c3 == 1 &&
                    c4 == 2 &&
                    c5 == 0 &&
                    c6 == 1
                );
                //
                Console.Write("testing read");
                var r1 = ctx.Get<TestEntityLong>(6);
                var r2 = ctx.Get(Builders<TestEntityString>.Filter.Where(f => f.Key == "6"));
                var r3 = ctx.Load(Builders<TestEntityLong>.Filter.Where(f => f.Description.Contains("es")));
                var r4 = ctx.Load<TestEntityString>(new List<string> { "6", "7", "", null });
                Log(
                    r1 != null &&
                    r2 != null &&
                    r3.Count == 2 &&
                    r4.Count == 2
                );
                //
                Console.Write("testing sort");
                var s1 = ctx.Get<TestEntityLong>();
                var s2 = ctx.Get<TestEntityLong>(null, new MongoFilterKit { OrderDirection = MongoFilterKit.EnumOrderDirection.Desc });
                var s3 = ctx.Get<TestEntityString>(null, new MongoFilterKit { OrderBy = "KEY", OrderDirection = MongoFilterKit.EnumOrderDirection.Desc });
                var s4 = ctx.Load<TestEntityString>(null, new MongoFilterKit { OrderBy = "name" });
                Log(
                    s1.Key == 6 &&
                    s2.Key == 7 &&
                    s3.Key == "7" &&
                    s4[0].Key == "7" &&
                    s4[1].Key == "6"
                );
                //
                Console.Write("testing transactions");
                ctx.BeginTransaction();
                ctx.Insert(new TestEntityLong { Key = 777 });
                ctx.BeginTransaction();
                ctx.Insert(new TestEntityLong { Key = 888 });
                ctx.Insert(new TestEntityLong { Key = 999 });
                ctx.CommitTransaction();
                var t1 = ctx.Count<TestEntityLong>();
                ctx.Delete<TestEntityLong>(999);
                //ctx.RollbackTransaction();
                ctx.CommitTransaction();
                var t2 = ctx.Count<TestEntityLong>();
                Log(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("all wrapper tests done...");
            Console.ReadKey();
        }

    }
}
