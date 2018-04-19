using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Collections.Specialized;

namespace TNDStudios.Blogs.Providers
{
    public class BlogDataProviderConnectionString
    {
        /// <summary>
        /// The properties of the connection string broken up into pairs
        /// </summary>
        public Dictionary<String, String> Properties;

        /// <summary>
        /// The origional Connection String Value
        /// </summary>
        private String connectionString;
        public String ConnectionString
        {
            get => connectionString;
            set
            {
                // Assign the private value
                connectionString = value;

                // Split up the connection string into it's property pairs
                Properties = Split(value);
            }
        }

        //

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BlogDataProviderConnectionString()
        {
            connectionString = ""; // No connection string by default
            Properties = new Dictionary<String, String>(); // No properties by default
        }

        /// <summary>
        /// Valued Constructor
        /// </summary>
        /// <param name="value"></param>
        public BlogDataProviderConnectionString(String value)
        {
            // Assign the private value
            connectionString = value;

            // Split up the connection string into it's property pairs
            Properties = Split(value);
        }

        /// <summary>
        /// Pull apart the connection string and provide a dictionary of it's contents
        /// </summary>
        /// <returns></returns>
        private Dictionary<String, String> Split(String value)
        {
            // The base result
            Dictionary<String, String> result = new Dictionary<String, String>();

            try
            {
                // Prepare the string to then pass to the hijack the http utility
                String parsedValue = value.Replace(';', '&');

                // Parse and convert
                NameValueCollection collection = HttpUtility.ParseQueryString(parsedValue);
                result = collection.AllKeys.ToDictionary(x => x, y => collection[y]);
            }
            catch (Exception ex)
            {
                // Raise the error
                throw new SplittingConnectionStringBlogException(ex);
            }

            // Return the result
            return result;
        }
    }
}