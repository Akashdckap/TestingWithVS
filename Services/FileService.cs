using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace P21_latest_template.Services
{
    public interface IFileService
    {
        int ColumnCount { get; set; }
        void AppendRow(IEnumerable<string> rows);
        void ExportFile(string filename);
    }

    public class FileService : IFileService
    {
        protected IList<string> Lines = new List<string>();
        public int ColumnCount { get; set; } = 0;

        public virtual void AppendRow(IEnumerable<string> rows)
        {
            if (ColumnCount == 0)
                throw new Exception("column count is required");
            if (rows == null)
                throw new ArgumentException("rows is required.");
            if (rows.Count() != ColumnCount)
                throw new ArgumentException("mismatch column count");

            Lines.Add(string.Join("\t", rows));
        }

        public virtual void ExportFile(string filename)
        {
            File.WriteAllLines(filename, Lines);
            Lines.Clear();
        }
    }
}
