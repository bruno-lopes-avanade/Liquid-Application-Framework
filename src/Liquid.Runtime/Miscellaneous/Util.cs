using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace Liquid.Runtime.Miscellaneous
{
    public static class Utils
    {
        /// <summary>
        /// Convert Array of byte to object
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="data">Array of byte</param>
        /// <returns>object</returns>
        public static T FromByteArray<T>(byte[] data)
        {
            if (data != null)
            {
                using (var m = new MemoryStream(data))
                {
                    var ser = new DataContractSerializer(typeof(T));
                    return (T)ser.ReadObject(m);
                }
            }
            else
            {
                return default(T);
            }
        }
        /// <summary>
        /// Convert object to ByteArray
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="obj">object</param>
        /// <returns>Array of byte</returns>
        public static byte[] ToByteArray(object obj)
        {
            using (var m = new MemoryStream())
            {
                var ser = new DataContractSerializer(obj.GetType());
                ser.WriteObject(m, obj);
                return m.ToArray();
            }
        }
    }
}
