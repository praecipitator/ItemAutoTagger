namespace ItemTagger.Helper
{
    internal class CaseInsensitiveStringComparer: IComparer<string>
    {
        int IComparer<string>.Compare(string? x, string? y)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(x, y);
        }
    }
}
