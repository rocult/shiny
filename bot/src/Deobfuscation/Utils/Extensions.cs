namespace MoonsecDeobfuscator.Deobfuscation.Utils;

public static class Extensions
{
    public static IEnumerable<List<T>> WindowedByDiscardedPairs<T>(this IEnumerable<T> source, Func<T, T, bool> predicate)
    {
        using var e = source.GetEnumerator();

        if (!e.MoveNext())
            yield break;

        var window = new List<T> { e.Current };
        var previous = e.Current;

        while (e.MoveNext())
        {
            var current = e.Current;

            if (predicate(previous, current))
            {
                yield return window.Count > 1 ? [..window[..^1]] : [];
                window.Clear();
                previous = default!;
                continue;
            }

            window.Add(current);
            previous = current;
        }

        if (window.Count > 0)
            yield return [..window];
    }
}