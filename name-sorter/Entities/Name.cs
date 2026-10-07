namespace NameSorterApp.Entities;

public sealed record Name(IReadOnlyList<string> GivenNames, string LastName)
{
    public override string ToString() => string.Join(' ', GivenNames.Append(LastName));
}