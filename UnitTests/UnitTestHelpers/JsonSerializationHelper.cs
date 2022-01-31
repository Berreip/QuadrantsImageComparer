using System.IO;
using Newtonsoft.Json;
using QuadrantsImageComparerLib.Core;

namespace UnitTests.UnitTestHelpers
{
    public static class JsonSerializationHelper
    {
        public static T SerializeThenDeserialize<T>(this T dataToSerialize)
        {
            var json = JsonConvert.SerializeObject(dataToSerialize);
            return JsonConvert.DeserializeObject<T>(json);
        } 
        public static T Deserialize<T>(this FileInfo file)
        {
            return JsonConvert.DeserializeObject<T>(file.ReadAllText());
        }
    }
}