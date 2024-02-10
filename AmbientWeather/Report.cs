using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AmbientWeather;

public class Report : IReport, IDisposable
{
    private readonly int _reportBufferLength;
    private readonly IntPtr _reportBuffer;

    public Report(int bufferLength)
    {
        _reportBufferLength = bufferLength;
        _reportBuffer = Marshal.AllocHGlobal(_reportBufferLength);
    }

    ~Report()
    {
        Dispose();
    }

    public void AddRawData(IEnumerable<byte> data)
    {
        if (Length > _reportBufferLength)
            throw new InvalidOperationException();

        var dataArray = data.ToArray();

        Trace(dataArray);

        // Copy data
        Marshal.Copy(dataArray, 0, _reportBuffer + Length, dataArray.Length);

        Length += dataArray.Length;
    }

    public int Length { get; private set; }

    public T? GetObject<T>()
    {
        return (T?)Marshal.PtrToStructure(_reportBuffer, typeof(T));
    }

    private static void Trace(IEnumerable<byte> data)
    {
        foreach (var b in data)
        {
            Debug.Write($"{b:X02} ");
        }
        Debug.WriteLine(string.Empty);
    }

    public void Dispose()
    {
        Marshal.FreeHGlobal(_reportBuffer);
        GC.SuppressFinalize(this);
    }
}