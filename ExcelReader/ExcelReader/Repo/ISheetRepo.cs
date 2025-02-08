using ExcelReader.Models;

namespace ExcelReader.Repo;
internal interface ISheetRepo
{
    public List<Sheet?> GetAll();
    public bool Add(Sheet sheet);
}
