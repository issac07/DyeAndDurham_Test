using NameSorterApp.Entities;

namespace NameSorterApp.Services;

public sealed class ComparerService : IComparer<Name>
{
    public int Compare(Name? left, Name? right)
    {
        if (ReferenceEquals(left, right))
        {
            return 0;
        }

        if (left is null)
        {
            return -1;
        }

        if (right is null)
        {
            return 1;
        }

        var comparison = StringComparer.OrdinalIgnoreCase.Compare(left.LastName, right.LastName);
        if (comparison != 0)
        {
            return comparison;
        }

        var sharedCount = Math.Min(left.GivenNames.Count, right.GivenNames.Count);
        for (var index = 0; index < sharedCount; index++)
        {
            comparison = StringComparer.OrdinalIgnoreCase.Compare(left.GivenNames[index], right.GivenNames[index]);
            if (comparison != 0)
            {
                return comparison;
            }
        }

        // If all shared given names match, the name with fewer given names sorts first.
        return left.GivenNames.Count.CompareTo(right.GivenNames.Count);
    }
}