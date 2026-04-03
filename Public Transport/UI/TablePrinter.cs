using System.Reflection;

namespace Public_Transport.UI;

public class TablePrinter
{
    public void PrintTable<T>(IEnumerable<T> items, params string[] propertyNames)
    {
        if (items is null)
        {
            Console.WriteLine("No data found.");
            return;
        }

        if (propertyNames is null || propertyNames.Length == 0)
            throw new ArgumentException("No header table values are provided.");

        var properties = typeof(T).GetProperties();
        var headers = new List<string>();
        var rows = new List<List<string>>();
        var selectedProperties = new List<PropertyInfo>();
        var columnWidths = new List<int>();

        BuildHeader<T>(properties, headers, selectedProperties, propertyNames);
        rows = BuildRows<T>(items, selectedProperties);
        columnWidths = CalculateColumnWidths(headers, rows);

        PrintHorizontalSeparator(columnWidths);
        PrintRow(headers, columnWidths);
        PrintHorizontalSeparator(columnWidths);
        foreach (var row in rows)
        {
            PrintRow(row, columnWidths);
        }
        PrintHorizontalSeparator(columnWidths);
    }

    private void PrintHorizontalSeparator(List<int> columnWidths)
    {
        var numOfChars = columnWidths.Sum() + 2 * columnWidths.Count + 1;
        Console.WriteLine(new string('-', numOfChars));
    }

    private void PrintRow(List<string> row, List<int> columnWidths)
    {
        Console.Write('|');
        for (int i = 0; i < row.Count; i++)
        {
            int lengthDifference = columnWidths[i] - row[i].Length;
            var paddedRow = ' ' + row[i] + new string(' ', lengthDifference) + '|';
            Console.Write(paddedRow);
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Builds a header by adding the specified property names to the headers list and tracks the corresponding properties.
    /// </summary>
    /// <remarks>All specified property names must exist in the provided properties array. The method ensures that only valid property names are added to the headers and selected properties lists.</remarks>
    /// <param name="properties">An array of PropertyInfo objects to search for properties matching the specified names.</param>
    /// <param name="headers">A list to which the names of the found properties are added.</param>
    /// <param name="selectedProperties">A list to which the PropertyInfo objects corresponding to the specified property names are added.</param>
    /// <param name="propertyNames">An array of property names to be added to the headers and selected properties lists.</param>
    /// <exception cref="ArgumentException">Thrown if any of the specified property names do not match a property in the properties array.</exception>
    private void BuildHeader<T>(
        PropertyInfo[] properties,
        List<string> headers,
        List<PropertyInfo> selectedProperties,
        params string[] propertyNames
    )
    {
        foreach (var propertyName in propertyNames)
        {
            PropertyInfo? propertyPresent = null;
            foreach (var property in properties)
            {
                if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    headers.Add(property.Name);
                    propertyPresent = property;
                    selectedProperties.Add(propertyPresent);
                    break;
                }
            }
            if (propertyPresent is null)
                throw new ArgumentException(
                    $"Property '{propertyName}' does not exist on type {typeof(T).Name}"
                );
        }
    }

    private List<List<string>> BuildRows<T>(
        IEnumerable<T> items,
        List<PropertyInfo> selectedProperties
    )
    {
        var rows = new List<List<string>>();
        foreach (var item in items)
        {
            var rowData = new List<string>();
            foreach (var property in selectedProperties)
            {
                var value = property.GetValue(item);
                rowData.Add(value?.ToString() ?? "");
            }
            rows.Add(rowData);
        }

        return rows;
    }

    private List<int> CalculateColumnWidths(List<string> headers, List<List<string>> rows)
    {
        int padding = 2;
        var columnWidths = new List<int>();
        for (int i = 0; i < headers.Count; i++)
        {
            var max = headers[i].Length;
            foreach (var row in rows)
            {
                if (row[i].Length > max)
                    max = row[i].Length;
            }
            columnWidths.Add(max + padding);
        }
        return columnWidths;
    }
}
