using System;
using TNDStudios.Web.Blogs.Core.Helpers;

namespace TNDStudios.ScratchPad
{
    class Program
    {
        static void Main(string[] args)
        {
            CryptoHelper helper = new CryptoHelper();
            String compareTo = "password";
            String comparingHash = helper.CalculateHash(compareTo);
            Boolean result = false;

            // Act
            result = helper.CheckMatch(comparingHash, compareTo);

        }
    }
}
