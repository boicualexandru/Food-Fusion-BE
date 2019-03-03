namespace Services.Reservations.Models
{
    public class TableStatus
    {
        public TableModel Table { get; set; }

        public bool IsAvailable { get; set; }
    }
}