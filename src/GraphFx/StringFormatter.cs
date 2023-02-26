namespace GraphFx;

public static class StringFormatter<T>
{
    public static readonly IStringFormatter<T> Default = new DefaultStringFormatter<T>();

    private sealed class DefaultStringFormatter<TValue> : IStringFormatter<TValue>
    {
        public string Format(TValue value)
        {
            return value?.ToString() ?? string.Empty;
        }
    }

    internal sealed class CustomStringFormatter<TValue> : IStringFormatter<TValue>
    {
        private readonly Func<TValue, string> format;

        public CustomStringFormatter(Func<TValue, string> format)
        {
            this.format = format ?? throw new ArgumentNullException(nameof(format));
        }

        public string Format(TValue value)
        {
            return this.format(value);
        }
    }
}

public static class StringFormatter
{
    public static IStringFormatter<T> Create<T>(Func<T, string> format)
    {
        return new StringFormatter<T>.CustomStringFormatter<T>(format);
    }
}
