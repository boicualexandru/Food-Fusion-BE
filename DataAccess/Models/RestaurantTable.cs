﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class RestaurantTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int RestaurantMapId { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        public int Seats { get; set; }


        public virtual RestaurantMap Map { get; set; }
        public virtual ICollection<ReservedTable> ReservedTables { get; set; }
    }
}
