using System.ComponentModel.DataAnnotations;

namespace Week5.Models
{
    public class ClassInformationModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Class Name is required.")]
        public string ClassName { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;
    }
}
