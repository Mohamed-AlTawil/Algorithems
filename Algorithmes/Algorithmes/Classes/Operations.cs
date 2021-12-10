using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithmes.Classes
{
    public struct Solution
    {
        public List<int> road_track;
        public double value;
        public bool change;
    } 
    class Operations
    {
        #region Paramters:

        private Point XY;
        //private string Path;
        private List<Point> coordinates = new List<Point>();
        private List<string> City;
        private List<List<double>> value;

        #endregion
        public Operations()
        {
            this.City = new List<string>();
            this.value = new List<List<double>>();
        }

        public void open(string Path)
        {
            ReadCSV(Path);
            CoordinatesToDistance();
        }
        public void ReadCSV(string Path)
        {
            var csvTable = new DataTable();
            var csvReader = new CsvReader(new StreamReader(File.OpenRead(Path)), true);
            csvTable.Load(csvReader);
            FillData(csvTable);
        }

        private void FillData(DataTable data)
        {
            City = new List<string>();
            value = new List<List<double>>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string Column1 = data.Rows[i][0].ToString();
                City.Add(Column1);
                XY.X = Int32.Parse(data.Rows[i][1].ToString());
                XY.Y = Int32.Parse(data.Rows[i][2].ToString());
                coordinates.Add(XY);
            }
        }
        private void CoordinatesToDistance()
        {

            for (int i = 0; i < coordinates.Count; i++)
            {
                value.Add(new List<double>());
                for (int j = 0; j < coordinates.Count; j++)
                {
                    if (i == j)
                    {
                        value[i].Add(0.0);
                    }
                    else
                    {
                        value[i].Add(distance(coordinates[i], coordinates[j]));
                    }
                }
            }
        }

        private double distance(Point first, Point second)
        {
            double x = first.X - second.X;
            double y = first.Y - second.Y;
            double des;


            x = Math.Pow(x, 2);
            y = Math.Pow(y, 2);

            des = Math.Sqrt(x + y);

            return Math.Round(des, 2);
        }
        public string ToString(List<int> sol)
        {
            string solve = "";
            for (int i = 0; i < sol.Count; i++)
            {
                solve += City[sol[i]] + ",";
            }
            return solve;
        }
        public Point max_X_Y()
        {
            int x = 0;
            int y = 0;
            for (int i = 0; i < coordinates.Count; i++)
            {
                if (x < coordinates[i].X)
                {
                    x = coordinates[i].X;
                }
                if (y < coordinates[i].Y)
                {
                    y = coordinates[i].Y;
                }
            }
            return new Point(x, y);
        }
        public List<string> GetCity()
        {
            return City;
        }
        public List<List<double>> GetValue()
        {
            return value;
        }
        public List<Point> Getcoordinate()
        {
            return coordinates;
        }

        public DataTable ToDataTable(List<string> City, List<List<double>> data)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("City");
            for (int i = 0; i < data[0].Count; i++)
            {
                dataTable.Columns.Add(City[i]);
            }

            for (int i = 0; i < data.Count; i++)
            {
                DataRow _ravi = dataTable.NewRow();
                _ravi[0] = City[i];
                for (int j = 0; j < data[i].Count; j++)
                {
                    _ravi[j+1] = data[i][j];
                }
                dataTable.Rows.Add(_ravi);
            }
            return dataTable;
        }
    }
}
