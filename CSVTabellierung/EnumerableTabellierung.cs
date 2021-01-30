using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableTabellierung
{
    public class EnumerableTabellierung
    {
        private Char CSVSeperator { get; set; }
        private bool IsFirstLineHeadline { get; set; }

        public EnumerableTabellierung(Char Seperator = ';', bool IsFirstLineHeadline = true)
        {
            this.CSVSeperator = Seperator;
            this.IsFirstLineHeadline = IsFirstLineHeadline;
        }

        public IEnumerable<IEnumerable<string>> Split(List<string> lists)
        {
            List<List<string>> result = new List<List<string>>();

            foreach (var line in lists)
            {
                var splitted = new List<string>();
                line.Split(this.CSVSeperator).ToList().ForEach(item =>
                {
                    splitted.Add(item);
                });
                result.Add(splitted);
            }

            return result;
        }

        public IEnumerable<IEnumerable<int>> GetColumnLengths(IEnumerable<IEnumerable<string>> List)
        {
            List<List<int>> result = new List<List<int>>();

            foreach (var column in List)
            {
                List<int> ColumLength = new List<int>();
                foreach (var columnItem in column)
                {
                    ColumLength.Add(columnItem.Length);
                }
                result.Add(ColumLength);
            }

            return result;
        }

        public IEnumerable<int> GetMaxColumnLenghtsPerRow(IEnumerable<IEnumerable<int>> source)
        {
            List<int> result = new List<int>();

            for (int row = 0; row < source.Count(); row++)
            {
                for (int i = 0; i < source.ElementAt(row).Count(); i++)
                {
                    if (row == 0)
                    {
                        result.Add(source.ElementAt(row).ElementAt(i)); //First Row. Just take this Value for MaxLength
                    }
                    else
                    {
                        if (result.ElementAt(i) < source.ElementAt(row).ElementAt(i)) //Check if the maxLength we already added at Columnindex is smaller tahn the actual value
                        {
                            //Replace it with the current one
                            result[i] = source.ElementAt(row).ElementAt(i);
                        }
                    }
                }
            }

            return result;
        }

        public IEnumerable<string> Convert(IEnumerable<string> Data)
        {
            List<string> result = new List<string>();

            var splitted = Split(Data.ToList());

            var MaxColumLengths = GetMaxColumnLenghtsPerRow(GetColumnLengths(splitted));

            for (var row = 0; row < splitted.Count(); row++)
            {
                string output = string.Empty;
                for (int column = 0; column < splitted.ElementAt(row).Count(); column++)
                {
                    output = String.Join("|", output, splitted.ElementAt(row).ElementAt(column).PadRight(MaxColumLengths.ElementAt(column)));
                }
                result.Add(output);
                if (row == 0)
                {
                    if (IsFirstLineHeadline == true)
                    {
                        string seperator = string.Empty;
                        //Add Header seperator
                        for (int col = 0; col < MaxColumLengths.Count(); col++)
                        {
                            seperator = String.Join("|", seperator, new String('-', MaxColumLengths.ElementAt(col)));
                        }
                        result.Add(seperator);
                    }
                }
            }

            return result;
        }
    }
}
