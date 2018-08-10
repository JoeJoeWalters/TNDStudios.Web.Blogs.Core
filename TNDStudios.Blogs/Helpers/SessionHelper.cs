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
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="session"></param>
        public SessionHelper()
        {

        }

        /// <summary>
        /// Get a GUID from the session
        /// </summary>
        /// <param name="key">The key in the session to be read</param>
        /// <returns></returns>
        public Nullable<Guid> GetGuid(ISession session, String key)
        {
            // Try and parse the sesion value
            Guid.TryParse(GetString(session, key), out Guid result);
            return (result == Guid.Empty) ? new Nullable<Guid>() : result; // Return the result
        }

        /// <summary>
        /// Set a Guid value to the session
        /// </summary>
        /// <param name="key">The key in the session to be written</param>
        /// <param name="value">The Guid to be written</param>
        /// <returns>If it was successful</returns>
        public Boolean SetGuid(ISession session, String key, Guid value)
            => SetString(session, key, ((value != null && value != Guid.Empty) ? value.ToString() : ""));

        /// <summary>
        /// Set the value as text to the session
        /// </summary>
        /// <param name="key">The key in the session to be set</param>
        /// <param name="value">The value to be set</param>
        /// <returns>If it was successful or not</returns>
        public Boolean SetString(ISession session, String key, String value)
            => SetBytes(session, key, Encoding.UTF8.GetBytes(value ?? ""));

        /// <summary>
        /// Get the string value from the session
        /// </summary>
        /// <param name="key">The key in the session to be read</param>
        /// <returns>The value of the key in the session</returns>
        public String GetString(ISession session, String key, String defaultValue)
            => GetString(session, key) ?? defaultValue;

        public String GetString(ISession session, String key)
        {
            Byte[] value = GetBytes(session, key);
            return (value == null) ? null : Encoding.UTF8.GetString(value);
        }

        /// <summary>
        /// Remove a key from the session
        /// </summary>
        /// <param name="session">The session to apply he removal from</param>
        /// <param name="key">The key in the session to be removed</param>
        /// <returns></returns>
        public Boolean Remove(ISession session, String key)
        {
            try
            {
                // Is the session available?
                if (session.IsAvailable)
                {
                    session.Remove(key); // Remove the item

                    // Did the remove work?
                    return (GetBytes(session, key) == null);
                }
            }
            catch
            {
            }

            return false; // Could not remove the item from the session
        }

        /// <summary>
        /// Set the bytes of a value in the session
        /// </summary>
        /// <param name="key">The key in the session to be set</param>
        /// <param name="value">The bytes to be written to the session</param>
        /// <returns>If the write is successful or not</returns>
        public Boolean SetBytes(ISession session, String key, byte[] value)
        {
            try
            {
                // Is the session available?
                if (session.IsAvailable)
                {
                    session.Set(key, value); // Do the set

                    // Check that the value and set value are the same
                    return (GetBytes(session, key) == value);
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
        public byte[] GetBytes(ISession session, String key)
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
