using ExpenseTrackerApp.Services.IHttpClient;
using System.Collections.ObjectModel;

namespace ExpenseTrackerApp.Services.Expense
{
    public class ExpenseService : IExpenseService
    {
        IHttpProvider httpProvider;
        ICacheService cacheService;
        private string cacheKey = "expenses_cache";
        public ExpenseService(IHttpProvider httpProvider, ICacheService cacheService)
        {
            this.httpProvider = httpProvider;
            this.cacheService = cacheService;
        }

        public async Task<ObservableCollection<ExpenseModel>> GetExpensesAsync()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(400)); // Simulate network delay
            var result = await httpProvider.GetAsync("https://jsonplaceholder.typicode.com/posts"); // Placeholder for actual API call


            var res = await cacheService.GetAsync<List<ExpenseModel>>(cacheKey);
            if (res != null) return new ObservableCollection<ExpenseModel>(res);
            else return new ObservableCollection<ExpenseModel>();

            /*
            var conten = await result.Content.ReadAsStringAsync();
            // Fix: Deserialize the string content to List<ExpenseModel>
            var expenses = System.Text.Json.JsonSerializer.Deserialize<List<ExpenseModel>>(conten);

            // Optionally, handle null if deserialization fails
            if (expenses == null)
                expenses = new List<ExpenseModel>();

            cacheService.SetAsync(cacheKey, expenses);  //Simulate save  to Handle No Internet 
            // Remove unused/incorrect variable and use the deserialized list
            return new ObservableCollection<ExpenseModel>(expenses.OrderByDescending(e => e.Date));
            */
        }

        public async Task<bool> AddExpenseAsync(ExpenseModel expense)
        {
            try
            {
                await Task.Delay(TimeSpan.FromMilliseconds(500));
                expense.Id = Guid.NewGuid();

                await Task.Delay(TimeSpan.FromMilliseconds(400)); // Simulate network delay
                var result = await httpProvider.GetAsync("https://jsonplaceholder.typicode.com/postss"); // Placeholder for actual API call

                var listexpense = await cacheService.GetAsync<List<ExpenseModel>>(cacheKey);

                listexpense ??= new List<ExpenseModel>();

                listexpense.Add(expense);

                await cacheService.SetAsync(cacheKey, listexpense);
                return true;
            }
            catch (Exception Excep)
            {


            }

            return true;

        }

        public async Task DeleteExpenseAsync(Guid id)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(400)); // Simulate network delay
            var result = await httpProvider.GetAsync("https://jsonplaceholder.typicode.com/posts/Remove"); // Placeholder for actual API call

            var listexpense = await cacheService.GetAsync<List<ExpenseModel>>(cacheKey);
            var itm = listexpense.FirstOrDefault(x => x.Id == id);
            listexpense.Remove(itm);

            await cacheService.SetAsync<List<ExpenseModel>>(cacheKey, listexpense);
        }

        public async Task UpdateExpenseAsync(ExpenseModel expense)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(400)); // Simulate network delay
            var result = await httpProvider.GetAsync("https://jsonplaceholder.typicode.com/postsUpdate"); // Placeholder for actual API call

            var listexpense = await cacheService.GetAsync<List<ExpenseModel>>(cacheKey);
            var index = listexpense.FindIndex(x => x.Id == expense.Id);
            if (index >= 0)
            {
                listexpense[index] = expense;
                await cacheService.SetAsync(cacheKey, listexpense);
            }
        }
    }
}
