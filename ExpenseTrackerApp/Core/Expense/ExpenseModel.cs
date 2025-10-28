namespace ExpenseTrackerApp.Services.Expense
{
    public class ExpenseModel
    {
        public Guid ? Id { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
