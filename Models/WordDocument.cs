
using System.ComponentModel.DataAnnotations;

namespace WordDocEditor.Models
{
    public class WordDocument
    {
        [Required]
        public string Base64 { get; set; }

        public string FileName { get; set; }
    }
}
