namespace ChatAppServer.Util
{
    public class Utils
    {
        public static bool CheckStrForNull(string? str)=>string.IsNullOrEmpty(str);
        public static bool CheckForNull(object? obj)=>obj==null;

    }
}
