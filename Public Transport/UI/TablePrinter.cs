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
        var columnWidths = new List<int>();

        BuildHeader(headers, propertyNames);
        rows = BuildRows<T>(items, headers);
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

    private void BuildHeader(List<string> headers, params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            headers.Add(propertyName);
        }
    }

    private List<List<string>> BuildRows<T>(IEnumerable<T> items, List<string> headers)
    {
        var rows = new List<List<string>>();
        foreach (var item in items)
        {
            var rowData = new List<string>();
            foreach (var property in headers)
            {
                var propertyInfo = item?.GetType().GetProperty(property);
                var value = propertyInfo?.GetValue(item);
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
