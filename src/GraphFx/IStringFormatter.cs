namespace GraphFx;

public interface IStringFormatter<in T>
{
    string Format(T value);
}