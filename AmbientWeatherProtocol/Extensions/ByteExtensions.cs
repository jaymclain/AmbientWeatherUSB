using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientWeather.Extensions
{
    public static class ByteExtensions
    {
        public static sbyte MsbSigned(this byte input)
        {
            var sign = (((input & (byte)0x80) == (byte)0x80) ? (sbyte)-1 : (sbyte)+1);
            return (sbyte)(sign * (input & 0x7F));
        }
    }
}
