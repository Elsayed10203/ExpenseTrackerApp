namespace ExpenseTrackerApp.UI.Core
{
    public class NavParams : Dictionary<string, object>
    {
        public T Get<T>(string key)
        {
            TryGetValue(key, out var value);
            try
            {
                return (T)value;
            }
            catch { }
           
            return default;
        }
    }
}
