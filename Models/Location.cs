using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaisGuinchos.Models
{
    public class Location : BaseEntity
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string DisplayName { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
