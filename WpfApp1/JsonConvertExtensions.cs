using Newtonsoft.Json;

namespace Demo
{
    public static class JsonConvertExtensions
    {
        public static T JsonDeserialize<T>(this string s)
        {
            return JsonConvert.DeserializeObject<T>(s);
        }
        public static string JsonSerialize<T>(this T o)
        {
            return JsonConvert.SerializeObject(o);
        }
    }
}