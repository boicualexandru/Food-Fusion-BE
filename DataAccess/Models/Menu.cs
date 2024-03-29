﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int RestaurantId { get; set; }


        public virtual Restaurant Restaurant { get; set; }
        public virtual ICollection<MenuItem> Items { get; set; }
    }
}
