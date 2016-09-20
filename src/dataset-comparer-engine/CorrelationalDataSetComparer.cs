using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace GrumpyDev.Net.DataTools.DataSetComparer
{
    public class CorrelationalDataSetComparer : IDataSetComparer
    {
        public DataSet GetDataSetDifferences(DataSet firstDataSet, DataSet secondDataSet)
        {
            DataSet differences = new DataSet();
            foreach (DataTable firstTable in firstDataSet.Tables)
            {
                if (secondDataSet.Tables.Contains(firstTable.TableName))
                {
                    var secondTable = secondDataSet.Tables[firstTable.TableName];
                    differences.Tables.Add(GetTableDifferences(firstTable, secondTable));
                }
            }
            return differences;
        }

        //TODO: Use the column objects
        public DataTable GetTableDifferences(DataTable firstTable, DataTable secondTable, string[] ignoredColummsNames, string[] orderColumnNames, string dataWork1Name = "Data in first", string dataWork2Name = "Data in second")
        {
            //Handles table names

            if (string.IsNullOrWhiteSpace(dataWork1Name))
            {
                dataWork1Name = "Data in first";
            }

            if (string.IsNullOrWhiteSpace(dataWork2Name))
            {
                dataWork2Name = "Data in second";
            }

            string diffTableName;
            if (firstTable.TableName.ToUpperInvariant().Equals(secondTable.TableName.ToUpperInvariant()))
            {
                diffTableName = String.Format("{0}_Differences", firstTable.TableName);
                firstTable.TableName = firstTable.TableName + "1";
                secondTable.TableName = firstTable.TableName + "2";

            }
            else
            {
                diffTableName = String.Format("{0}_{1}_Differences", firstTable.TableName, secondTable.TableName);
            }

            DataTable ResultDataTable = new DataTable(diffTableName);

            //use a Dataset to make use of a DataRelation object  
            using (DataSet ds = new DataSet())
            {
                //Add tables  

                ds.Tables.AddRange(new DataTable[] { firstTable.Copy(), secondTable.Copy() });


                //Get Columns for DataRelation  
                DataColumn[] firstColumns = new DataColumn[ds.Tables[0].Columns.Count - ignoredColummsNames.Length];
                int offset = 0;
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                //for (int i = 0; i < firstColumns.Count<DataColumn>(); i++)
                {
                    if (!ignoredColummsNames.Contains<string>(ds.Tables[0].Columns[i].ColumnName))
                    {
                        firstColumns[i - offset] = ds.Tables[0].Columns[i];
                    }
                    else
                    {
                        offset++;
                    }
                }
                //for (int i = 0; i < firstColumns.Length; i++)
                //{
                //    if (!ignoredColummsNames.Contains<string>(ds.Tables[0].Columns[i].ColumnName))
                //    {
                //        firstColumns[i] = ds.Tables[0].Columns[i];

                //    }
                //    else
                //    {
                //        i--;
                //    }

                //}

                DataColumn[] secondColumns = new DataColumn[ds.Tables[1].Columns.Count - ignoredColummsNames.Length];
                offset = 0;
                for (int i = 0; i < ds.Tables[1].Columns.Count; i++)
                //for (int i = 0; i < secondColumns.Count<DataColumn>(); i++)
                {
                    if (!ignoredColummsNames.Contains<string>(ds.Tables[1].Columns[i].ColumnName))
                    {
                        secondColumns[i - offset] = ds.Tables[1].Columns[i];
                    }
                    else
                    {
                        offset++;
                    }
                }

                //for (int i = 0; i < secondColumns.Length; i++)
                //{
                //    if (!ignoredColummsNames.Contains<string>(ds.Tables[1].Columns[i].ColumnName))
                //    {
                //        secondColumns[i] = ds.Tables[1].Columns[i];

                //    }
                //    else
                //    {
                //        i--;
                //    }
                //}

                //Create DataRelation  
                DataRelation r1 = new DataRelation(string.Empty, firstColumns, secondColumns, false);
                ds.Relations.Add(r1);

                DataRelation r2 = new DataRelation(string.Empty, secondColumns, firstColumns, false);
                ds.Relations.Add(r2);

                //Create columns for return table  
                for (int i = 0; i < firstTable.Columns.Count; i++)
                {
                    ResultDataTable.Columns.Add(firstTable.Columns[i].ColumnName, firstTable.Columns[i].DataType);
                }

                ResultDataTable.Columns.Add("Comparison_Result", typeof(String));


                //If FirstDataTable Row not in SecondDataTable, Add to ResultDataTable.  
                ResultDataTable.BeginLoadData();
                foreach (DataRow parentrow in ds.Tables[0].Rows)
                {
                    DataRow[] childrows = parentrow.GetChildRows(r1);
                    if (childrows == null || childrows.Length == 0)
                    {
                        //parentrow["Comparison_Result"] = "Found on table 1 but not in 2";
                        List<object> cols = new List<object>();
                        cols.AddRange(parentrow.ItemArray);
                        cols.Add(dataWork1Name);
                        ResultDataTable.LoadDataRow(cols.ToArray<object>(), true);
                    }
                }

                //If SecondDataTable Row not in FirstDataTable, Add to ResultDataTable.  
                foreach (DataRow parentrow in ds.Tables[1].Rows)
                {
                    DataRow[] childrows = parentrow.GetChildRows(r2);
                    if (childrows == null || childrows.Length == 0)
                    {
                        //parentrow["Comparison_Result"] = "Found on table 2 but not in 1";
                        List<object> cols = new List<object>();
                        cols.AddRange(parentrow.ItemArray);
                        cols.Add(dataWork2Name);
                        ResultDataTable.LoadDataRow(cols.ToArray<object>(), true);
                        //ResultDataTable.LoadDataRow(parentrow.ItemArray, true);
                    }
                }
                ResultDataTable.EndLoadData();
            }

            DataView view = ResultDataTable.AsDataView();
            StringBuilder sortbuilder = new StringBuilder();
            foreach (var item in orderColumnNames)
            {
                sortbuilder.Append(item);
                sortbuilder.Append(",");
            }
            view.Sort = sortbuilder.ToString().TrimEnd(',');
            return view.ToTable();

            //return ResultDataTable;
        }

        [Obsolete]
        public DataTable GetTableDifferences(DataTable firstTable, DataTable secondTable)
        {

            //Handles table names
            string diffTableName;
            if (firstTable.TableName.ToUpperInvariant().Equals(secondTable.TableName.ToUpperInvariant()))
            {
                diffTableName = String.Format("{0}_Differences", firstTable.TableName);
                firstTable.TableName = firstTable.TableName + "1";
                secondTable.TableName = firstTable.TableName + "2";
            }
            else
            {
                diffTableName = String.Format("{0}_{1}_Differences", firstTable.TableName, secondTable.TableName);
            }

            DataTable ResultDataTable = new DataTable(diffTableName);

            //use a Dataset to make use of a DataRelation object  
            using (DataSet ds = new DataSet())
            {
                //Add tables  
                ds.Tables.AddRange(new DataTable[] { firstTable.Copy(), secondTable.Copy() });

                //Get Columns for DataRelation  
                DataColumn[] firstColumns = new DataColumn[ds.Tables[0].Columns.Count];
                for (int i = 0; i < firstColumns.Length; i++)
                {
                    firstColumns[i] = ds.Tables[0].Columns[i];
                }

                DataColumn[] secondColumns = new DataColumn[ds.Tables[1].Columns.Count];
                for (int i = 0; i < secondColumns.Length; i++)
                {
                    secondColumns[i] = ds.Tables[1].Columns[i];
                }

                //Create DataRelation  
                DataRelation r1 = new DataRelation(string.Empty, firstColumns, secondColumns, false);
                ds.Relations.Add(r1);

                DataRelation r2 = new DataRelation(string.Empty, secondColumns, firstColumns, false);
                ds.Relations.Add(r2);

                //Create columns for return table  
                for (int i = 0; i < firstTable.Columns.Count; i++)
                {
                    ResultDataTable.Columns.Add(firstTable.Columns[i].ColumnName, firstTable.Columns[i].DataType);
                }

                ResultDataTable.Columns.Add("Comparison_Result", typeof(String));


                //If FirstDataTable Row not in SecondDataTable, Add to ResultDataTable.  
                ResultDataTable.BeginLoadData();
                foreach (DataRow parentrow in ds.Tables[0].Rows)
                {
                    DataRow[] childrows = parentrow.GetChildRows(r1);
                    if (childrows == null || childrows.Length == 0)
                    {
                        //parentrow["Comparison_Result"] = "Found on table 1 but not in 2";
                        List<object> cols = new List<object>();
                        cols.AddRange(parentrow.ItemArray);
                        cols.Add("Row from data work 1");
                        ResultDataTable.LoadDataRow(cols.ToArray<object>(), true);
                    }
                }

                //If SecondDataTable Row not in FirstDataTable, Add to ResultDataTable.  
                foreach (DataRow parentrow in ds.Tables[1].Rows)
                {
                    DataRow[] childrows = parentrow.GetChildRows(r2);
                    if (childrows == null || childrows.Length == 0)
                    {
                        //parentrow["Comparison_Result"] = "Found on table 2 but not in 1";
                        List<object> cols = new List<object>();
                        cols.AddRange(parentrow.ItemArray);
                        cols.Add("Row from data work 2");
                        ResultDataTable.LoadDataRow(cols.ToArray<object>(), true);
                        //ResultDataTable.LoadDataRow(parentrow.ItemArray, true);
                    }
                }
                ResultDataTable.EndLoadData();
            }

            return ResultDataTable;
        }

        public string GetDifferencesHtmlTable(DataTable differences, string[] identifierColumns, string[] ignoredColumns)
        {
            StringBuilder sb = new StringBuilder();
            List<int> ignoredColumnIndexList = new List<int>();
            foreach (var item in ignoredColumns)
            {
                ignoredColumnIndexList.Add(differences.Columns.IndexOf(item));
            }
            int[] ignoredColumnIndices = ignoredColumnIndexList.ToArray<int>();


            sb.Append(String.Format("<h4>{0}</h4>", differences.TableName));
            sb.Append("Comparison&nbsp;criteria&#58;<br>");
            sb.AppendLine("Identifier&nbsp;Columns<br>");
            sb.AppendLine("<ul>");
            foreach (var item in identifierColumns)
            {
                sb.Append(String.Format("<li>{0}</li>", item));
            }
            sb.AppendLine("</ul>");
            sb.AppendLine("Ignored&nbsp;Columns<br>");
            sb.AppendLine("<ul>");

            foreach (var item in ignoredColumns)
            {
                sb.Append(String.Format("<li>{0}</li>", item));
            }
            sb.AppendLine("</ul>");

            //generate table header row
            sb.AppendLine("<div class='comparisonresults'>");
            sb.Append("<table><thead>");

            for (int colIndx = 0; colIndx < differences.Columns.Count; colIndx++)
            {
                sb.Append("<th>");
                sb.Append(differences.Columns[colIndx].ColumnName);
                if (ignoredColumnIndices.Contains<int>(colIndx)) { sb.Append("(Ignored)"); }
                sb.Append("</th>");
            }
            sb.Append("</thead>");
            sb.Append("<tbody>");
            //add data rows to html table
            //Limiting the data displayed.
            //TODO: Deal with this
            int rows = (differences.Rows.Count > 500) ? 500 : differences.Rows.Count;
            for (int rowIndx = 0; rowIndx < rows; rowIndx++)
            {
                bool aboveIsSame = (rowIndx > 0 && AreDataRowsTheSameEntity(differences.Rows[rowIndx], differences.Rows[rowIndx - 1], identifierColumns));
                bool belowIsSame = (rowIndx < differences.Rows.Count - 1 && AreDataRowsTheSameEntity(differences.Rows[rowIndx], differences.Rows[rowIndx + 1], identifierColumns));
                sb.Append("<tr>");
                for (int colIndx = 0; colIndx < differences.Columns.Count; colIndx++)
                {
                    if (ignoredColumnIndices.Contains<int>(colIndx) || ((aboveIsSame && differences.Rows[rowIndx][colIndx].ToString() == differences.Rows[rowIndx - 1][colIndx].ToString()) ||
                        (belowIsSame && differences.Rows[rowIndx][colIndx].ToString() == differences.Rows[rowIndx + 1][colIndx].ToString())))
                    {
                        sb.Append("<td>");
                        sb.Append
                      (differences.Rows[rowIndx][colIndx].ToString());
                        sb.Append("</td>");
                    }
                    else
                    {
                        sb.Append("<td class='differentcell'>");
                        sb.Append
                      (differences.Rows[rowIndx][colIndx].ToString());
                        sb.Append("</td>");
                    }

                }
                sb.Append("</tr>");
            }
            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.AppendLine("</div>");
            return sb.ToString();
        }

        private bool AreDataRowsTheSameEntity(DataRow row1, DataRow row2, string[] identifierColumns)
        {

            foreach (var item in identifierColumns)
            {
                if (!row1[item].Equals(row2[item]))
                {
                    return false;
                }
            }
            return true;
        }

    }
}