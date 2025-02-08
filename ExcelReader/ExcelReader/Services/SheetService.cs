using ExcelReader.Interfaces;
using ExcelReader.Models;
using ExcelReader.Repo;
using OfficeOpenXml;

namespace ExcelReader.Services;
internal class SheetService : ISheetService
{

    private readonly ISheetRepo _sheetRepo;
    private readonly string _filePath;

    public SheetService(ISheetRepo sheetRepo, string matchesFilePath)
    {
        _sheetRepo = sheetRepo;
        _filePath = matchesFilePath;
    }
    public bool AddRecords(List<Sheet?> sheets)
    {
        try
        {
            if (sheets.Count == 0)
                return false;

            foreach (var sheet in sheets)
            {
                if (sheet.Records.Count == 0)
                {
                    Console.WriteLine($"There are 0 records in the sheet {sheet.Name}. Skipping...");
                    continue;
                }

                _sheetRepo.Add(sheet);
            }
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error is observed: {ex.Message}");
            return false;
        }
    }

    public List<Sheet?> ReadSpreadsheet()
    {
        List<Sheet?> sheets = new();

        using (var package = new ExcelPackage(new FileInfo(_filePath)))
        {
            var parsedSheets = package.Workbook.Worksheets;

            foreach (var sheet in parsedSheets) {
                var currentSheet = new Sheet();
                currentSheet.Records = new List<Record?>();
                currentSheet.Name = sheet.Name;

                int rowCount = sheet.Dimension.Rows;
                int columnCount = sheet.Dimension.Columns;
                
                for(int row = 2; row <= rowCount; row++)
                {
                    Record record = new();
                    var currentRow = sheet.Cells[row, 1, row, columnCount];

                    string[] extractedInformation = new string[6];

                    int index = 0;
                    foreach(var cell in currentRow)
                    {
                        if (index < extractedInformation.Length)
                        {
                            extractedInformation[index] = cell.Text;
                        }
                        index++;
                    }

                    record.Team1 = extractedInformation[1];
                    record.Team2 = extractedInformation[2];
                    record.Score = extractedInformation[3];
                    record.Result = extractedInformation[4];
                    record.Date = DateOnly.Parse(extractedInformation[5]);
                    record.SheetId = sheet.Index + 1;

                    currentSheet.Records.Add(record);
                }

                sheets.Add(currentSheet);
            }
        }

        return sheets;
    }

    public List<Sheet?> GetAll()
    {
        try 
        { 
            List<Sheet?> sheets = _sheetRepo.GetAll();
            return sheets;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error is observed: {ex.Message}");
            return null;
        }
    }
}
