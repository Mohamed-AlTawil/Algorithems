using Algorithm;
using Algorithmes.Classes;
using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Algorithmes
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Series series1;
        Genetic_Algorithms genetic;
        Ant_Colony Ant;
        Operations operation;
        List<string> City = new List<string>();
        List<List<double>> value = new List<List<double>>();
        //  Model model = new Model();
        List<Point> coordinates = new List<Point>();
        Point max;

        public Form1()
        {
            InitializeComponent();
        }

        private void Open_File_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
          
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "E:\\master karabuk\\Metasezgisel Yontemler";
            openFileDialog1.Filter = "Database files (*.CSV, *.csv)|*.CSV;*.csv";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;
            var csvTable = new DataTable();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;
                operation = new Operations();
                TextPath.Caption= selectedFileName;
                this.Text= "Algorithmes - "+Path.GetFileName(selectedFileName);
                operation.open(selectedFileName);
                City = operation.GetCity();
                value = operation.GetValue();
                coordinates = operation.Getcoordinate();
               TextElements.Caption = City.Count.ToString();

                draw();
                bool butEnable = false;
                if (City.Count > 0) { butEnable = true; }
                else { butEnable = false; }
               StartBut.Enabled = butEnable;
               ClearBut.Enabled = butEnable;
                Data.Enabled = butEnable;
            }
        }

        private void ClearBut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            clear();
        }

        private void StartBut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            series1 = new Series("Distance", ViewType.Line);

            int numberOFRepet = Int32.Parse(TextNumberOfIteration.Text);

            progressBar_OfAnt.Properties.Maximum = numberOFRepet;
           progressBar_OfAnt.Position = 0;

           
            if (AlgorithmType.SelectedIndex == 0)
            {
                Genetic_Algorithms(numberOFRepet);

            }
            else
            {

                ACO_Algorithms(numberOFRepet);

            }
            chartControlShow();
        }
        private void Genetic_Algorithms(int iter)
        {
            Solution solution = new Solution();
            int Number_Of_Solves = Int32.Parse(TextNumberOfAnt.Text);
            int Crosswise = Int32.Parse(TextOfAlfa.Text);
            int Mutation = Int32.Parse(TextOfBeta.Text);
            genetic = new Genetic_Algorithms(Number_Of_Solves, City, value, Crosswise, Mutation);
            clear();
            for (int i = 0; i < iter; i++)
            {
                solution = genetic.Start();
                Thread.Sleep(150);
                progressBar_OfAnt.Position = i + 1;
                numberOfrepet.Text = (i + 1).ToString();
                if (solution.change)
                {
                    draw_coordinate(solution.road_track);
                    ShowSolve.Text = operation.ToString(solution.road_track);
                    ShowValue.Text =  Math.Round(solution.value, 2).ToString();
                }

                series1.Points.Add(new SeriesPoint(i, solution.value));
                Application.DoEvents();
            }
        }
        private void ACO_Algorithms(int iter)
        {
            Solution solution = new Solution();

            int Number_Of_Ant = Int32.Parse(TextNumberOfAnt.Text);
            double pheromone = double.Parse(TextPheromone.Text);
            int Alfa = Int32.Parse(TextOfAlfa.Text);
            int Beta = Int32.Parse(TextOfBeta.Text);
            double P = double.Parse(TextOfP.Text);

            Ant = new Ant_Colony(City, value, Number_Of_Ant, pheromone, Alfa, Beta, P);
            clear();
            //Ant.building();
            for (int i = 0; i < iter; i++)
            {
                solution = Ant.Start();
                Thread.Sleep(150);

                progressBar_OfAnt.Position = i + 1;
                numberOfrepet.Text = (i + 1).ToString();
                if (solution.change)
                {
                    draw_coordinate(solution.road_track);
                    ShowSolve.Text = operation.ToString(solution.road_track);
                    ShowValue.Text = Math.Round(solution.value, 2).ToString();
                }
                series1.Points.Add(new SeriesPoint(i, solution.value));
                Application.DoEvents();
            }
        }



        private void clear()
        {
            try {

                if (AlgorithmType.SelectedIndex == 0)
                {
                    genetic.clear();
                }
                else
                {
                    Ant.ClearAnt();
                }
                ShowSolve.Text = "";
                ShowValue.Text = "";
                progressBar_OfAnt.Position = 0;
                numberOfrepet.Text = "";
                DrawPath.Refresh();
                draw();

            } catch { }
            
        }


        private void draw()
        {
            max = operation.max_X_Y();
            Graphics g = DrawPath.CreateGraphics();
            for (int i = 0; i < coordinates.Count; i++)
            {
                Brush pen1 = Brushes.Green;
                Brush pen2 = Brushes.Red;
                Font myFont = new Font("Arial", 10);
                int x = Convert.ToInt32((coordinates[i].X * (DrawPath.Width - 40)) / max.X) + 20;
                int y = Convert.ToInt32((coordinates[i].Y * (DrawPath.Height - 40)) / max.Y) + 20;
                g.FillEllipse(pen2, x - 3, y - 3, 6, 6);
                g.DrawString(City[i], myFont, pen1, x - 15, y - 15);
            }


        }
        void draw_coordinate(List<int> sol)
        {
            //Graphics g = pictureBox1.CreateGraphics();
            int wid = DrawPath.Width;
            int high = DrawPath.Height;
            int x1, y1, x2, y2;
            DrawPath.Image = null;
            DrawPath.Update();
            DrawPath.Refresh();
            //Task thread1 = Task.Factory.StartNew(() =>
            //{
            Graphics g = DrawPath.CreateGraphics();
            for (int i = 0; i < coordinates.Count; i++)
            {
                Brush pen1 = Brushes.Green;
                Brush pen2 = Brushes.Red;
                Font myFont = new Font("Arial", 10);
                int x = Convert.ToInt32((coordinates[sol[i]].X * (wid - 40)) / max.X) + 20;
                int y = Convert.ToInt32((coordinates[sol[i]].Y * (high - 40)) / max.Y) + 20;
                g.FillEllipse(pen2, x - 3, y - 3, 7, 7);
                g.DrawString(City[sol[i]], myFont, pen1, x - 15, y - 15);
            }
            // });

            //Task thread2 = Task.Factory.StartNew(() =>
            //{
            Graphics g1 = DrawPath.CreateGraphics();
            AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5);
            for (int i = 1; i < coordinates.Count + 1; i++)
            {
                Pen pen = new Pen(Color.Blue, 2);
                pen.CustomEndCap = bigArrow;
                x1 = Convert.ToInt32((coordinates[sol[i - 1]].X * (wid - 40)) / max.X) + 20;
                y1 = Convert.ToInt32((coordinates[sol[i - 1]].Y * (high - 40)) / max.Y) + 20;
                x2 = Convert.ToInt32((coordinates[sol[i]].X * (wid - 40)) / max.X) + 20;
                y2 = Convert.ToInt32((coordinates[sol[i]].Y * (high - 40)) / max.Y) + 20;
                g1.DrawLine(pen, x1, y1, x2, y2);
            }
            //});
            // Task.WaitAll(thread1, thread2);

            

        }

        private void AlgorithmType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool antEnable = false;
            if (AlgorithmType.SelectedIndex == 0)
            {
                antEnable = false;
                labelControl3.Text = "Number of Solves:";
                labelControl4.Text = "Crosswise:";
                labelControl5.Text = "Mutation:";
                TextOfAlfa.Text = "80";
                TextOfBeta.Text = "20";
            }
            else
            {
                antEnable = true;
                labelControl3.Text = "Number Of Ant:";
                labelControl4.Text = "Alfa:";
                labelControl5.Text = "Beta:";
                TextOfAlfa.Text = "3";
                TextOfBeta.Text = "3";
            }

            TextOfP.Visible = antEnable;
            TextPheromone.Visible = antEnable;
            labelControl6.Visible = antEnable;
            labelControl7.Visible = antEnable;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            series1 = new Series("Distance", ViewType.Line);
            TextNumberOfIteration.Select();
            AlgorithmType.SelectedIndex = 0;
            series1.Points.Add(new SeriesPoint(1, 2));
            series1.Points.Add(new SeriesPoint(2, 12));
            series1.Points.Add(new SeriesPoint(3, 14));
            series1.Points.Add(new SeriesPoint(4, 17));
            chartControlShow();
        }

        private void chartControlShow()
        {
            


            chartControl1.Series.Clear();
            chartControl1.Series.Add(series1);
            series1.ArgumentScaleType = ScaleType.Numerical;
            ((LineSeriesView)series1.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            ((LineSeriesView)series1.View).LineMarkerOptions.Size = 3;
            ((LineSeriesView)series1.View).LineStyle.Thickness = 1;
            ((LineSeriesView)series1.View).LineMarkerOptions.Kind = MarkerKind.Circle;
            ((LineSeriesView)series1.View).LineStyle.DashStyle =DevExpress.XtraCharts.DashStyle.Dash;
        }

        private void Data_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataViewForm dataview = new DataViewForm();
            dataview.data = value;
            dataview.City = City;
            dataview.ShowDialog();
        }
    }
}
