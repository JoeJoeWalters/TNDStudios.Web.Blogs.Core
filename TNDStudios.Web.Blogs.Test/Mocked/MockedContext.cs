using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TNDStudios.Web.Blogs.Core.Test
{
    /// <summary>
    /// Create a mocked up HttpContext for the test classes to use
    /// which is more flexible that using Moq to specify the attributes
    /// when the mocked context can be set up in the test fixture
    /// </summary>
    public class MockedContext : DefaultHttpContext
    {
        /// <summary>
        /// Override the base session with another so a mocked up session handler
        /// can be injected instead
        /// </summary>
        internal ISession session;
        public override ISession Session { get => session; set => session = value; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MockedContext()
        {
            session = new MockedSession(); // Assign a new mocked session to the mocked Http Context
        }
    }

    /// <summary>
    /// Mocked up session handler for the mocked HTTP context
    /// </summary>
    public class MockedSession : ISession
    {
        /// <summary>
        /// The storage for the fake values for the session
        /// </summary>
        internal Dictionary<String, byte[]> values;

        /// <summary>
        /// Session is always available as it is fake
        /// </summary>
        bool ISession.IsAvailable => true;

        /// <summary>
        /// Return a fixed session that is defined by the constructor
        /// </summary>
        internal String sessionId;
        string ISession.Id => sessionId;

        /// <summary>
        /// Get a list of all of the keys for the fake session
        /// </summary>
        IEnumerable<string> ISession.Keys => values.Keys;

        /// <summary>
        /// Clear the fake session
        /// </summary>
        void ISession.Clear()
        {
            values = new Dictionary<string, byte[]>();
        }

        Task ISession.CommitAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task ISession.LoadAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove a value from the fake session
        /// </summary>
        /// <param name="key">The key for the value</param>
        void ISession.Remove(string key)
        {
            if (values.ContainsKey(key))
                values.Remove(key);
        }

        /// <summary>
        /// Set the value of the fake session
        /// </summary>
        /// <param name="key">The key for the value</param>
        /// <param name="value">The value to be stored</param>
        void ISession.Set(string key, byte[] value)
            => values[key] = value;

        /// <summary>
        /// Try and get a value from the fake session
        /// </summary>
        /// <param name="key">The key for the value</param>
        /// <param name="value">The value to be retrieved</param>
        /// <returns></returns>
        bool ISession.TryGetValue(string key, out byte[] value)
        {
            if (values.ContainsKey(key))
            {
                try
                {
                    value = values[key];
                    return true;
                }
                catch
                {
                }
            }

            value = null;
            return false;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MockedSession()
        {
            values = new Dictionary<string, byte[]>(); // Storage for the session values
            sessionId = Guid.NewGuid().ToString(); // Set a fake session id
        }
    }
}
