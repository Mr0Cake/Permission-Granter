using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PermissionGranter.ViewModel.Services
{
    public class Messenger
    {
        private static readonly object CreationLock = new object();
        private static readonly ConcurrentDictionary<MessengerKey, object> Dictionary = new ConcurrentDictionary<MessengerKey, object>();


        private static Messenger _Instance;
        public static Messenger Default
        {
            get
            {
                if (_Instance == null)
                {
                    lock (CreationLock)
                    {
                        if (_Instance == null)
                            _Instance = new Messenger();
                    }
                }
                return _Instance;
            }
        }

        private Messenger()
        {

        }

        public void Register<T>(object recipient, Action<T> action)
        {
            Register(recipient, action, null);
        }

        public void Register<T>(object recipient, Action<T> action, object context)
        {
            MessengerKey key = new MessengerKey(recipient, context);
            Dictionary.TryAdd(key, action);
        }

        public void Unregister(object recipient)
        {
            Unregister(recipient, null);
        }

        public void Unregister(object recipient, object context)
        {
            object action;
            MessengerKey key = new MessengerKey(recipient, context);
            Dictionary.TryRemove(key, out action);
        }

        public void Send<T>(T message)
        {
            Send(message, null);
        }

        public void Send<T>(T message, object context)
        {
            IEnumerable<KeyValuePair<MessengerKey, object>> result;
            //if (context == null)
            //    result = Dictionary.Where(r => r.Key.Context == null);
            //else
            //    result = Dictionary.Where(r => r.Key.Context != null && r.Key.Context.Equals(context));
            if (context == null)
            {
                result = from r in Dictionary where r.Key.Context == null select r;
            }
            else
            {
                result = from r in Dictionary where r.Key.Context != null && r.Key.Context.Equals(context) select r;
            }

            foreach (var action in result.Select(x => x.Value).OfType<Action<T>>())
            {
                action(message);
            }
        }

        protected class MessengerKey
        {
            public object Recipient { get; private set; }
            public object Context { get; private set; }

            public MessengerKey(object recipient, object context)
            {
                Recipient = recipient;
                Context = context;
            }

            protected bool Equals(MessengerKey other)
            {
                return Equals(Recipient, other.Recipient) && Equals(Context, other.Context);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals(obj as MessengerKey);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Recipient != null ? Recipient.GetHashCode() : 0) * 397) ^ (Context != null ? Context.GetHashCode() : 0);
                }
            }
        }
    }
}
