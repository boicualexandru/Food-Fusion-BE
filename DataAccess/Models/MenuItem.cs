using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class MenuItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int MenuId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public double Price { get; set; }


        public virtual Menu Menu { get; set; }
    }
}
