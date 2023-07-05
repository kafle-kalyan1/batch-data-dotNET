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

    public class FinalData
    {
        public string property_name { get; set; }
        public string report_name { get; set; }
        public List<ReportFilters> filters { get; set; }

        //Primary Table
        public TableData primary_table { get; set; }

        //Secondary Table
        public List<TableData> secondary_table  { get; set; }
    }

    public class ReportFilters
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class TableHeader
    {
        public List<string> value { get; set; }
        public bool is_bold { get; set; }
    }
    public class TableRow
    {
        public List<string> value { get; set; }
        public bool is_bold{ get; set; }
        public bool is_cell_merged{ get; set; }
        public string bgColor { get; set; }
    }
    public class TableData
    {
        public TableHeader header { get; set; }
        public List<TableRow> rows { get; set; }
    }
}
