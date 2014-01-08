using System.Collections.Generic;

namespace AmbientWeather
{
    public interface IReport
    {
        void AddRawData(IEnumerable<byte> data);

        T GetObject<T>();
    }
}
