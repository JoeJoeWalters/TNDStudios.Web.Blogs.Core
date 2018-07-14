using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TNDStudios.Web.Blogs.Core.Helpers
{
    /// <summary>
    /// Helpers for dealing with session values
    /// </summary>
    public class SessionHelper
    {
        // Reference to the session object
        private ISession session;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="session"></param>
        public SessionHelper(ISession session)
            => this.session = session;

        /// <summary>
        /// Set the value as text to the session
        /// </summary>
        /// <param name="value">The value to be set</param>
        /// <returns>If it was successful or not</returns>
        public Boolean SetString(String key, String value)
        {
            //"tndstudios.web.blogs.core.token"
            Byte[] securityTokenValue = new byte[0];
            session.TryGetValue(key, out securityTokenValue);
            String pulledToken = Encoding.UTF8.GetString(securityTokenValue ?? new byte[0]);

            return true;
        }

        /// <summary>
        /// Get the string value from the session
        /// </summary>
        /// <param name="key">The key in the session to be read</param>
        /// <returns>The value of the key in the session</returns>
        public String GetString(String key, String defaultValue) 
            => GetString(key) ?? defaultValue;

        public String GetString(String key)
        {
            Byte[] value = GetBytes(key);
            return (value == null) ? null : Encoding.UTF8.GetString(value);
        }

        /// <summary>
        /// Set the bytes of a value in the session
        /// </summary>
        /// <param name="key">The key in the session to be set</param>
        /// <param name="value">The bytes to be written to the session</param>
        /// <returns>If the write is successful or not</returns>
        public Boolean SetBytes(String key, byte[] value)
        {
            try
            {
                // Is the session available?
                if (session.IsAvailable)
                {
                    session.Set(key, value); // Do the set

                    // Check that the value and set value are the same
                    return (GetBytes(key) == value);
                }
                else
                    return false; // Session was not available
            }
            catch
            {
                return false; // Something died
            }
        }

        /// <summary>
        /// Read a set of bytes from the session
        /// </summary>
        /// <param name="key">The key in the session to be read</param>
        /// <returns>The byte array read from the session</returns>
        public byte[] GetBytes(String key)
        {
            try
            {
                // Is the session available?
                if (session.IsAvailable)
                {
                    Byte[] sessionValue = null; // Create an null reference to indicate a fail by default
                    session.TryGetValue(key, out sessionValue); // Try and get the session value and fail to a null if failed
                    return sessionValue; // Return the session value (or the fail state)
                }
                else
                    return null; // Session was not available
            }
            catch
            {
                return null; // Return a fail state
            }
        }
    }
}
