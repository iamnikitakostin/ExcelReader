using System.ComponentModel.DataAnnotations;

namespace ExcelReader.Models;
internal class Record
{
    public int Id { get; set; }
    public string Team1 {  get; set; }
    public string Team2 { get; set; }
    public string Score { get; set; }
    public string Result { get; set; }
    public DateOnly Date { get; set; }
    public int SheetId {  get; set; }
}
