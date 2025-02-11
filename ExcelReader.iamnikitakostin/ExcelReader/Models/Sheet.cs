using System.ComponentModel.DataAnnotations;

namespace ExcelReader.Models;
internal class Sheet
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Record?> Records { get; set; }
}
