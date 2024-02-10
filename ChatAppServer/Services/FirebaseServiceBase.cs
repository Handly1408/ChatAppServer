namespace ChatAppServer.Services
{
    public abstract class FirebaseServiceBase<T>where T : class
    {
        public bool Initialized { get; protected set; }

        private static readonly object _lock = new();

        private static T? _instance;
        public static T Instance { get {
                lock (_lock) {
                   return _instance ??= Activator.CreateInstance<T>();
                }
            }
        }
         
        
    }
}
