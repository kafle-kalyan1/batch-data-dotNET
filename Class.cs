using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace batch_data
{
    [Table("data")]
    public class Data
    {
        [Key, Required]
        public int id { get; set; }

        [Required]
        public string name { get; set; } = null!;

        [Required]
        public string gender { get; set; } = null!;

        [Required]
        public string[] hobbies { get; set; } = null!;

        [Required]
        public int batch { get; set; }
    }

    [Table("batch")]
    public class Batch
    {
        [Key, Required]
        public int id { get; set; }
    }
    }
