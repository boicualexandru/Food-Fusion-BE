using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int RestaurantId { get; set; }
        
        public int ParticipantsCount { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }


        public virtual Restaurant Restaurant { get; set; }
        public virtual ICollection<ReservedTable> ReservedTables { get; set; }
    }
}
