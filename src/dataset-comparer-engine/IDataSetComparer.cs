using System.Data;

namespace GrumpyDev.Net.DataTools.DataSetComparer
{
    public interface IDataSetComparer
    {
        System.Data.DataSet GetDataSetDifferences(System.Data.DataSet firstDataSet, System.Data.DataSet secondDataSet);
        System.Data.DataTable GetTableDifferences(System.Data.DataTable firstTable, System.Data.DataTable secondTable);

        System.Data.DataTable GetTableDifferences(DataTable firstTable, DataTable secondTable, string[] ignoredColummsNames, string[] orderColumnNames, string dataWorkName1, string dataWorkName2);
    }
}