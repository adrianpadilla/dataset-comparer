using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataset_converters
{
    public static class CsvToDataSetConverter
    {
        public static DataTable Convert(string csvSeparatedString, bool containsHeaders = false)
        {
            var dataTable = new DataTable();
            var lines = csvSeparatedString.Split('\n');


            if (containsHeaders)
            {
                var headerLine = lines.ElementAt(0);
                var columnsNames = headerLine.Split(',');
                SetColumnsInDataTable(dataTable, columnsNames);
            }

            for (int i = containsHeaders? 1:0; i < lines.Length; i++)
            {
                var row = dataTable.NewRow();
                var line = lines[i];
                if (line == string.Empty)
                {
                    continue;
                }
                var columns = line.Split(',');
                
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    row[dataTable.Columns[j]] = columns[j];
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        internal static void SetColumnsInDataTable(DataTable datatable, string[] columnNames)
        {
            foreach (var columnName in columnNames)
            {
                datatable.Columns.Add(columnName);
            }
        }
    }
}
