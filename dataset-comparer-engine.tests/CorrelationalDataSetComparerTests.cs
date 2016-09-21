using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GrumpyDev.Net.DataTools.DataSetComparer;
using System.IO;
using GrumpyDev.Net.DataTools.Converters;

namespace dataset_comparer_engine.tests
{
    [TestClass]
    public class CorrelationalDataSetComparerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var fileText1 = File.ReadAllText(@"");
            var fileText2 = File.ReadAllText(@"");
            var dataTable1 = CsvToDataSetConverter.Convert(fileText1, true);
            var dataTable2 = CsvToDataSetConverter.Convert(fileText2, true);

            var comparer = new CorrelationalDataSetComparer();

            var dataTable = comparer.GetTableDifferences(dataTable1, dataTable2);
            var html = comparer.GetDifferencesHtmlTable(dataTable, new string[0], new string[0]);
            File.WriteAllText("output.html", html);

        }
    }
}
