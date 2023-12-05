namespace day5;
public static class Extensions
{
    public static T GetValueOrDefault<T>(this Dictionary<T,T> dictionary, T key)
    {
        return dictionary.TryGetValue(key, out T outValue) ? outValue : key;
    }
}
