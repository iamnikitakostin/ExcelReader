using ExcelReader;
using ExcelReader.Data;
using ExcelReader.Models;
using ExcelReader.Repo;
using ExcelReader.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;

internal class Program()
{
    static void Main()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string? connectionString = config.GetConnectionString("DefaultConnection");
        string? matchesFilePath = config["FilePath"];

        if (matchesFilePath == null) {
            Console.WriteLine("Please, make sure to provide the filepath to the excel file.");
        } else if (connectionString == null)
        {
            Console.WriteLine("Please, make sure to provide the connection string for the db.");
        }

        var services = new ServiceCollection();

        services.AddSingleton<IConfiguration>(config);
        services.AddSingleton<string>(matchesFilePath);

        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(connectionString));
        
        services.AddScoped<SheetService>();
        services.AddScoped<ISheetRepo, SheetRepo>();

        var servicesProvider = services.BuildServiceProvider();

        var dbContext = servicesProvider.GetRequiredService<DataContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        var sheetService = servicesProvider.GetRequiredService<SheetService>();
 
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        Console.WriteLine("Reading all the records from the spreadsheet...");
        List<Sheet?> sheets = sheetService.ReadSpreadsheet();

        Console.WriteLine("Adding all the records to the database...");
        bool isSuccess = sheetService.AddRecords(sheets);

        List<Sheet?> savedSheets = sheetService.GetAll();

        if (isSuccess)
        {
            View.DrawAllRecords(savedSheets);
        }
    }
}