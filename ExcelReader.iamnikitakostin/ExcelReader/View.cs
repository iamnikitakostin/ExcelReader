using ExcelReader.Models;

namespace ExcelReader;
internal class View
{
    public static void DrawAllRecords(List<Sheet?> sheets)
    {
        foreach(var sheet in sheets)
        {
            Console.WriteLine($"Sheet #{sheet.Id} - {sheet.Name}");
            Console.WriteLine("=================================");

            foreach (var record in sheet.Records)
            {
                Console.WriteLine($"{record.Id.ToString()} - {record.Date} - {record.Team1} vs. {record.Team2} - {record.Score} - {record.Result}");
            }
        }

        Console.WriteLine("\n\nPlease, press any key to close the app...");
        Console.ReadKey(false);
    }
}
