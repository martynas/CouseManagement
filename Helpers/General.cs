using System;
using System.Collections.Generic;
using System.Text;

namespace Atrendia.CourseManagement.Helpers
{
    public class General
    {

        /// <summary>
        /// Encode (and/or sign) given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Encode(string value)
        {
            try
            {
                byte[] toEncodeAsBytes = System.Text.UTF8Encoding.UTF8.GetBytes(value);
                return System.Convert.ToBase64String(toEncodeAsBytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Decode (and/or check signature) of given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Decode(string value)
        {
            try
            {
                byte[] encodedDataAsBytes = System.Convert.FromBase64String(value);
                return System.Text.UTF8Encoding.UTF8.GetString(encodedDataAsBytes);
            }
            catch
            {
                return string.Empty;
            }
        }

    }
}
