using ExcelReader.Data;
using ExcelReader.Models;

namespace ExcelReader.Repo;
internal class SheetRepo : ISheetRepo
{
    private readonly DataContext _context;
    public SheetRepo(DataContext context)
    {
        _context = context;
    }
    public List<Sheet?> GetAll() {
        List<Sheet?> sheets = _context.Sheets.ToList();

        return sheets;
    }
    public bool Add(Sheet sheet) {
        if (sheet == null)
        {
            return false;
        }

        _context.Sheets.Add(sheet);
        _context.SaveChanges();
        return true;
    }
}
