using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using System;
using System.IO;

namespace AEAssist
{
    public static class MongoHelper
    {
        public static JsonWriterSettings Settings = new JsonWriterSettings
        {
            Indent = true,
            OutputMode = JsonOutputMode.CanonicalExtendedJson
        };

        static MongoHelper()
        {
            var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };

            ConventionRegistry.Register("IgnoreExtraElements", conventionPack, type => true);

            BsonClassMap.LookupClassMap(typeof(Language));
        }

        public static void Init()
        {
        }

        public static string ToJson(object obj)
        {
            return obj.ToJson(Settings);
        }

        public static string ToJson(object obj, JsonWriterSettings settings)
        {
            return obj.ToJson(settings);
        }

        public static T FromJson<T>(string str)
        {
            try
            {
                return BsonSerializer.Deserialize<T>(str);
            }
            catch (Exception e)
            {
                throw new Exception($"{str}\n{e}");
            }
        }

        public static object FromJson(Type type, string str)
        {
            return BsonSerializer.Deserialize(str, type);
        }

        public static byte[] ToBson(object obj)
        {
            return obj.ToBson();
        }

        public static void ToStream(object message, MemoryStream stream)
        {
            using (var bsonWriter = new BsonBinaryWriter(stream, BsonBinaryWriterSettings.Defaults))
            {
                var context = BsonSerializationContext.CreateRoot(bsonWriter);
                BsonSerializationArgs args = default;
                args.NominalType = typeof(object);
                var serializer = BsonSerializer.LookupSerializer(args.NominalType);
                serializer.Serialize(context, args, message);
            }
        }

        public static object FromBson(Type type, byte[] bytes)
        {
            try
            {
                return BsonSerializer.Deserialize(bytes, type);
            }
            catch (Exception e)
            {
                throw new Exception($"from bson error: {type.Name}", e);
            }
        }

        public static object FromBson(Type type, byte[] bytes, int index, int count)
        {
            try
            {
                using (var memoryStream = new MemoryStream(bytes, index, count))
                {
                    return BsonSerializer.Deserialize(memoryStream, type);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"from bson error: {type.Name}", e);
            }
        }

        public static object FromStream(Type type, Stream stream)
        {
            try
            {
                return BsonSerializer.Deserialize(stream, type);
            }
            catch (Exception e)
            {
                throw new Exception($"from bson error: {type.Name}", e);
            }
        }

        public static T FromBson<T>(byte[] bytes)
        {
            try
            {
                using (var memoryStream = new MemoryStream(bytes))
                {
                    return (T)BsonSerializer.Deserialize(memoryStream, typeof(T));
                }
            }
            catch (Exception e)
            {
                throw new Exception($"from bson error: {typeof(T).Name}", e);
            }
        }

        public static T FromBson<T>(byte[] bytes, int index, int count)
        {
            return (T)FromBson(typeof(T), bytes, index, count);
        }

        public static T Clone<T>(T t)
        {
            return FromBson<T>(ToBson(t));
        }
    }
}