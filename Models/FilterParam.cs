using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P21_latest_template.Models
{
    public class FilterParam
    {
        public int page_size { get; set; } = 15;
        public int page_number { get; set; } = 1;
        public DateTime date_last_modified { get; set; } = DateTime.Now.AddHours(-24);
    }
}
