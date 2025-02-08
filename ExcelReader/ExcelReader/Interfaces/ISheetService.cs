using ExcelReader.Models;

namespace ExcelReader.Interfaces;
internal interface ISheetService
{
    public bool AddRecords(List<Sheet?> sheets);
    public List<Sheet?> ReadSpreadsheet();
    public List<Sheet?> GetAll();
}
