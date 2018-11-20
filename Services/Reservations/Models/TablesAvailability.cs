using Common;
using System.Collections.Generic;

namespace Services.Reservations.Models
{
    public class TablesAvailability
    {
        public TimeRange Range { get; set; }

        public List<TableModel> FreeTables { get; set; }
    }
}
