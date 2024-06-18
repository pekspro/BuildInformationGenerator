namespace Pekspro.BuildInformationGenerator;

public struct ValueSource<T>
{
    public T Value { get; }

    public string Source { get; }

    public ValueSource(T value, string source)
    {
        Value = value;
        Source = source;
    }

    public void Deconstruct(out T value, out string source)
    {
        value = Value;
        source = Source;
    }
}
