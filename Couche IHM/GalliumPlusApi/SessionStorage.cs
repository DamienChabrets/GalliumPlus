using System.Collections.Concurrent;

namespace GalliumPlusApi
{
    public class SessionStorage
    {
        private static SessionStorage? current;

        public static SessionStorage Current
        {
            get
            {
                if (current == null)
                {
                    current = new SessionStorage();
                }
                return current;
            }
        }

        ConcurrentDictionary<string, object> storage;

        private SessionStorage()
        {
            storage = new();
        }

        public SessionStorage Put(string key, object value)
        {
            storage[key] = value;
            return this;
        }

        public object Get(string key)
        {
            return storage[key];
        }

        public T Get<T>(string key)
        {
            return (T)storage[key]; 
        }

        public SessionStorage Remove(string key)
        {
            storage.TryRemove(key, out var _);
            return this;
        }
    }
}
