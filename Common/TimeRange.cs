using System;
using System.ComponentModel.DataAnnotations;

namespace Common
{
    public class TimeRange
    {
        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }
    }
}