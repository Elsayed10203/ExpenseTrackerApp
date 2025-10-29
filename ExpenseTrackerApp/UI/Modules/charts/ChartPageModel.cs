using CommunityToolkit.Mvvm.Input;
using ExpenseTrackerApp.Services.Expense;
using ExpenseTrackerApp.UI.Modules.AddEditExpense;
using ExpenseTrackerApp.UI.Modules.Category;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExpenseTrackerApp.UI.Modules.charts
{
     public class ChartPageModel : BaseCategoryPageModel
    {
        IExpenseService expenseService;

        public ChartPageModel(IExpenseService expenseService)
        {
              this.expenseService = expenseService;
            ExportCommand = new AsyncRelayCommand(ExceuteExportCommand);
        }

        #region Properties
        private string summaryTotalCount;
        public string SummaryTotalCount
        {
            get => summaryTotalCount;
            set
            {
                summaryTotalCount = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<ChartUiModel> chartList;
        public ObservableCollection<ChartUiModel> ChartList
        {
            get => chartList;
            set
            {
                chartList = value;
                RaisePropertyChanged();
            }
        }



        #endregion

        public IRelayCommand ExportCommand { get; set; }

        private async Task ExceuteExportCommand()
        {
            ExportCSV_SONAsync();
           //await ExportAsync();
        }

        protected override   void ViewIsAppearing(object sender, EventArgs e)
        {
          _=  LoadSumaryTotal();
             base.ViewIsAppearing(sender, e);
        }
        private ExpenceUiModel MapToUiModel(ExpenseModel expense)
        {
            var categoryUiModel = Categories?.FirstOrDefault(c => c.CatId == expense.CategoryId);

            return new ExpenceUiModel
            {
                Id = expense.Id,
                Category = categoryUiModel,
                ExpenseDescrption = expense.Description,
                Amount = expense.Amount,
                ExpenseDate = expense.Date
            };
        }


        private async Task LoadSumaryTotal()
        {
            try
            {
                Task.Delay(100);
                 var allExpenses = await expenseService.GetExpensesAsync();
               var  expenses=new ObservableCollection<ExpenceUiModel>();    
                foreach (var expense in allExpenses.OrderByDescending(e => e.Date))
                {
                    expenses.Add(MapToUiModel(expense));
                }

                SummaryTotalCount = expenses?.Sum(e => e.Amount).ToString("C2") ?? "$0.00";

                var list = expenses?
                    .GroupBy(e => e.Category?.CatId)
                    .Select(g => new ChartUiModel
                    {
                        CatgId = g.Key ?? 0,
                        CatgName = g.FirstOrDefault()?.Category?.Name,
                        Amount = (double)g.Sum(e => e.Amount)
                    })
                    .ToList() ?? new List<ChartUiModel>();

                ChartList=new ObservableCollection<ChartUiModel>(list);                 
            }
            catch (Exception Excep)
            {
                   
            }
        }

        #region Export
        public   async Task ExportAsync(  )
        {

            string filePath = Path.Combine(FileSystem.AppDataDirectory, "expenses.csv");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // pretty print
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,  
                Converters = { new JsonStringEnumConverter() }  
            };

            using var fs = File.Create(filePath);
            var allExpenses = await expenseService.GetExpensesAsync();
           await JsonSerializer.SerializeAsync(fs, allExpenses, options);
        }

        #region saveReport
        public async Task ExportCSV_SONAsync()
        {
            try
            {
                var fileName = "expenses.csv";
                var cacheDir = FileSystem.CacheDirectory;
                var tempPath = Path.Combine(cacheDir, fileName);

                var allExpenses = await expenseService.GetExpensesAsync();

                var sb = new StringBuilder();
                sb.AppendLine("Id,Date,Description,Amount,CategoryId,CategoryName");

                static string EscapeCsv(string? s) => (s ?? string.Empty).Replace("\"", "\"\"");

                foreach (var expense in allExpenses.OrderByDescending(e => e.Date))
                {
                    var ui = MapToUiModel(expense);
                    var id = expense.Id.ToString();
                    var date = expense.Date.ToString("o", CultureInfo.InvariantCulture);
                    var description = EscapeCsv(expense.Description);
                    var amount = expense.Amount.ToString(CultureInfo.InvariantCulture);
                    var categoryId = expense.CategoryId.ToString();
                    var categoryName = EscapeCsv(ui?.Category?.Name ?? string.Empty);

                    sb.AppendLine($"\"{id}\",\"{date}\",\"{description}\",\"{amount}\",\"{categoryId}\",\"{categoryName}\"");
                }

                await File.WriteAllTextAsync(tempPath, sb.ToString(), Encoding.UTF8);

               

#if ANDROID
                try
                {
                    var downloadsDir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)?.AbsolutePath;
                    if (!string.IsNullOrEmpty(downloadsDir))
                    {
                        var downloadsPath = Path.Combine(downloadsDir, fileName);
                        File.Copy(tempPath, downloadsPath, true);
                        Helper.ShowToaster.show($"Saved to {downloadsPath}");
                        return;
                    }
                }
                catch { /* ignore and fall back to share */ }
#endif

                // Fallback: open Share dialog so user can save/open the CSV
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Export Expenses CSV",
                    File = new ShareFile(tempPath)
                });

                Helper.ShowToaster.show("Exported CSV (share dialog opened)");
            }
            catch (Exception)
            {
                Helper.ShowToaster.show("Export failed");
            }
        }

        #endregion
         
        #endregion
    }
}
