namespace AmbientWeather.Extensions
{
    public static class UshortExensions
    {
        public static ushort Swap(this ushort input)
        {
            return (ushort)(((input & 0xFF00) >> 8) | ((input & 0x00FF) << 8));
        }

        public static short MsbSigned(this ushort input)
        {
            var sign = (((input & (ushort)0x8000) == (ushort)0x8000) ? (short)-1 : (short)+1);
            return (short)(sign * (input & 0x7FFF));
        }
    }
}