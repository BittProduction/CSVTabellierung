using System;
using Xunit;
using EnumerableTabellierung;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

namespace CSVTabellierungTester
{
    public class Tester
    {
        [Fact]
        public void TestSplitting()
        {
            var service = new EnumerableTabellierung.EnumerableTabellierung();

            var result = service.Split(new List<string>() { "Name;Strasse;Ort;Alter" }).ToList();

            Assert.True(result.Count() != 0);

        }

        [Fact]
        public void TestSplitResult()
        {
            List<List<string>> expected = new List<List<string>>();
            expected.Add(new List<string>() { "Name", "Strasse", "Ort", "Alter" });

            var service = new EnumerableTabellierung.EnumerableTabellierung();

            var result = service.Split(new List<string>() { "Name;Strasse;Ort;Alter" }).ToList();

            Assert.True(Enumerable.SequenceEqual(expected[0],result[0]));
        }

        [Fact]
        public void TestCoulmnLength()
        {
            List<List<int>> expected = new List<List<int>>();
            expected.Add(new List<int> (){ 4, 7, 3, 5 });

            var service = new EnumerableTabellierung.EnumerableTabellierung();

            var splitted = service.Split(new List<string>() { "Name;Strasse;Ort;Alter" }).ToList();

            var result = service.GetColumnLengths(splitted).ToList();

            Assert.True(Enumerable.SequenceEqual(expected[0], result[0]));
        }

        [Fact]
        public void TestGetMaxColumnLengthPerRow()
        {
            List<string> source = new List<string>();
            source.Add("Name;Strasse;Ort;Alter");                       //4 , 7, 3,5
            source.Add("Peter Pan;Am Hang 5;12345 Einsam;42");          //9 , 9,12,2
            source.Add("Maria Schmitz;Kölner Straße 45;50123 Köln;43"); //13,16,10,2

            List<int> expected = new List<int>() {13,16, 12,5};

            var service = new EnumerableTabellierung.EnumerableTabellierung();

            var splitted = service.Split(source.ToList());

            var ColumnLengths = service.GetColumnLengths(splitted).ToList();

            var result = service.GetMaxColumnLenghtsPerRow(ColumnLengths);

            Assert.True(Enumerable.SequenceEqual(expected, result));
        }

        [Fact]
        public void TestConsoleOutputWithHeadline()
        {
            List<string> source = new List<string>();
            source.Add("Name;Strasse;Ort;Alter");                       //4 , 7, 3,5
            source.Add("Peter Pan;Am Hang 5;12345 Einsam;42");          //9 , 9,12,2
            source.Add("Maria Schmitz;Kölner Straße 45;50123 Köln;43"); //13,16,10,2

            List<string> expected = new List<string>();
            expected.Add("|Name         |Strasse         |Ort         |Alter");
            expected.Add("|-------------|----------------|------------|-----");
            expected.Add("|Peter Pan    |Am Hang 5       |12345 Einsam|42   ");
            expected.Add("|Maria Schmitz|Kölner Straße 45|50123 Köln  |43   ");

            var service = new EnumerableTabellierung.EnumerableTabellierung();

            var result = service.Convert(source);

            Assert.True(Enumerable.SequenceEqual(expected, result));
        }

        [Fact]
        public void TestConsoleOutputWithoutHeadline()
        {
            List<string> source = new List<string>();
            source.Add("Name;Strasse;Ort;Alter");                       //4 , 7, 3,5
            source.Add("Peter Pan;Am Hang 5;12345 Einsam;42");          //9 , 9,12,2
            source.Add("Maria Schmitz;Kölner Straße 45;50123 Köln;43"); //13,16,10,2

            List<string> expected = new List<string>();
            expected.Add("|Name         |Strasse         |Ort         |Alter");
            expected.Add("|Peter Pan    |Am Hang 5       |12345 Einsam|42   ");
            expected.Add("|Maria Schmitz|Kölner Straße 45|50123 Köln  |43   ");

            var service = new EnumerableTabellierung.EnumerableTabellierung(IsFirstLineHeadline: false);

            var result = service.Convert(source);

            Assert.True(Enumerable.SequenceEqual(expected, result));
        }

        [Fact]
        public void TestFileOutput()
        {
            List<string> source = new List<string>();
            source.Add("Name;Strasse;Ort;Alter");                       //4 , 7, 3,5
            source.Add("Peter Pan;Am Hang 5;12345 Einsam;42");          //9 , 9,12,2
            source.Add("Maria Schmitz;Kölner Straße 45;50123 Köln;43"); //13,16,10,2
            source.Add("Markus Bittner;Hindenburgstrasse 3;79183 Waldkirch;35"); //13,16,10,2
            source.Add("Anastasia Bittner;Vogesenstrasse 5;xxxxx Weisweil;31"); //13,16,10,2

            var service = new EnumerableTabellierung.EnumerableTabellierung();

            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string DirectoryPath = Path.GetDirectoryName(path);
            string FilePath = Path.Combine(DirectoryPath, "MyTestFile.txt");
            service.ConvertToFile(FilePath, source);
        }
    }
}
