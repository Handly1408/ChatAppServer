using ChatAppServer.Model;

namespace ChatAppServer.Util
{
    internal class EnumUtil
    {
        public static T GetEnumFromString<T>(string enumStr) where T : struct
        {
            T messageEvent = default;
            Enum.TryParse(enumStr, out messageEvent);
            return messageEvent;
        }
        public static string GetStrFromEnum<T>(T value) where T : struct
        {
            
          return  Enum.GetName(typeof(T),value)??default!;
            
        }
    }
}
