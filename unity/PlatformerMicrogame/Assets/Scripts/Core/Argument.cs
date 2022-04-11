using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core
{
    /// <summary>
    /// Provides runtime checks for arguments, and throws exceptions.
    /// </summary>
    public static class Argument
    {
        /// <summary>
        /// Asserts that the specified argument not be null; otherwise throws.
        /// </summary>
        /// <param name="argument">The argument to verify</param>
        /// <param name="argumentName">The name of the argument</param>
        public static void AssertNotNull(object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}
