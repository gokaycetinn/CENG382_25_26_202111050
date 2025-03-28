using System.ComponentModel.DataAnnotations;

namespace Week5.Models
{
    public class ClassInformationModel
    {
        public int Id { get; set; }

        [Required]
        public string ClassName { get; set; } = string.Empty;

        
        public int StudentCount { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
