namespace ServicePortal.Data;

public static class OnboardingProductCatalog
{
    public static readonly IReadOnlyList<string> Products =
    [
        "Brokerage Portal",
        "eCERT",
        "eGTA",
        "TransitNet"
    ];

    public static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> Options =
        new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["eCERT"] =
            [
                "Arab Certificate of Origin",
                "ATA Carnet",
                "EU Certificate of Origin",
                "EUR1",
                "EUR-MED",
                "International Import Certificate",
                "UK Certificate of Origin"
            ],
            ["eGTA"] =
            [
                "CDS Exports",
                "CDS Imports",
                "eCustoms Brokerage",
                "eCustoms Portal",
                "EMCS",
                "ENS",
                "GVMS",
                "Send to Brokerage",
                "Send to TransitNet",
                "TransitNet"
            ]
        };

    public static ProductSelection Parse(string? value)
    {
        var selection = new ProductSelection();

        if (string.IsNullOrWhiteSpace(value))
        {
            return selection;
        }

        foreach (var entry in value.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
        {
            var product = Products.FirstOrDefault(item =>
                string.Equals(entry, item, StringComparison.OrdinalIgnoreCase) ||
                entry.StartsWith($"{item} (", StringComparison.OrdinalIgnoreCase));

            if (product is null) continue;

            selection.Products.Add(product);

            if (!Options.TryGetValue(product, out var availableOptions))
            {
                continue;
            }

            var openingBracket = entry.IndexOf('(');
            var closingBracket = entry.LastIndexOf(')');

            if (openingBracket < 0 || closingBracket <= openingBracket)
            {
                continue;
            }

            var optionText = entry[(openingBracket + 1)..closingBracket];
            var selectedOptions = optionText
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            selection.Options[product] = availableOptions
                .Where(selectedOptions.Contains)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
        }

        return selection;
    }

    public static string Serialize(ProductSelection selection)
    {
        var values = new List<string>();

        foreach (var product in Products.Where(selection.Products.Contains))
        {
            if (Options.TryGetValue(product, out var availableOptions) &&
                selection.Options.TryGetValue(product, out var selectedOptions))
            {
                var orderedOptions = availableOptions
                    .Where(selectedOptions.Contains)
                    .ToList();

                if (orderedOptions.Count > 0)
                {
                    values.Add($"{product} ({string.Join(", ", orderedOptions)})");
                    continue;
                }
            }

            values.Add(product);
        }

        return string.Join("; ", values);
    }
}

public sealed class ProductSelection
{
    public HashSet<string> Products { get; } =
        new(StringComparer.OrdinalIgnoreCase);

    public Dictionary<string, HashSet<string>> Options { get; } =
        new(StringComparer.OrdinalIgnoreCase);
}
