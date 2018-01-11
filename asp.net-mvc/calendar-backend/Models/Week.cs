using System.ComponentModel.DataAnnotations;

namespace calendar_backend.Models
{
    public class Week
    {
        [Key]
        public int No { get; set; }
        public Day[] Days { get; set; }
        public int Year { get; set; }
    }
}