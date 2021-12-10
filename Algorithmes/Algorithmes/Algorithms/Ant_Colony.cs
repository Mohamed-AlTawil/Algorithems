using Algorithmes.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithm
{
    class Ant_Colony
    {
        private List<String> City;
        private List<List<double>> value; double pheromone;
        private int Alfa;
        private int Beta;
        private double P;
        private int NumberOfAnt;
        Random r = new Random();
        public List<List<double>> PheromoneList = new List<List<double>>();
        List<List<double>> inverse_distance = new List<List<double>>();

        public List<List<int>> Ant = new List<List<int>>();
       
        List<List<int>> Remaining_Cities = new List<List<int>>();
        public List<double> sumOfSlo = new List<double>();
        public double max=1000000;
        public int solve;
        List<int> last_sol = new List<int>();

        Solution solutions = new Solution();
        public Ant_Colony(List<String> City, List<List<double>> Value, int NumberOfAnt, double pheromone,int Alfa ,int Beta,double P)
        {
            this.City = City;
            this.value = Value;
            this.NumberOfAnt = NumberOfAnt;
            this.pheromone = pheromone;
            this.Alfa = Alfa;
            this.Beta = Beta;
            this.P = P;
            
        }

        /// <summary>  create()
        /// Generate the main Lists (PheromoneList,inverse_distance)
        /// </summary>
        private void create()
        {
            solutions.road_track = new List<int>();
            for (int i = 0; i < City.Count; i++)
            {
                PheromoneList.Add(new List<double>());
                inverse_distance.Add(new List<double>());

                for (int j = 0; j < City.Count; j++)
                {
                    if (i != j)
                    {
                        double x = 1.0 / value[i][j];
                        PheromoneList[i].Add(pheromone);
                        inverse_distance[i].Add(x);
                    }
                    else
                    {
                        PheromoneList[i].Add(0);
                        inverse_distance[i].Add(0);
                    }
                }
            }
        }
        private void setRC()//RC:Remaining_Cities
        {
            for (int i = 0; i < NumberOfAnt; i++)
            {
                Remaining_Cities.Add(new List<int>());
               
                for (int j = 0; j < City.Count; j++)
                {
                    Remaining_Cities[i].Add(j);
                }
            }
        }
       /* public void building()
        {
            create();
            nextBuil();
        }
        public void next_stage()
        {
           // last_sol.Clear();
            
            Ant.Clear();
            sumOfSlo.Clear();
            Remaining_Cities.Clear();
            max = 1000000;
            nextBuil();
        }*/
        public Solution Start()
        {
            Ant.Clear();
            sumOfSlo.Clear();
            Remaining_Cities.Clear();
            max = 1000000;
            if (PheromoneList.Count == 0 || inverse_distance.Count==0)
            {
                create();
            }
            setRC();
            nextBuil();
            Pherom();
            return solutions;
        }


        /// <summary>    nextBuil()
        /// 1- Generate solutions by the number of ants
        /// 2- add the last solution if it exists
        /// 3- Calculate the value of the solution and select the mın of value
        /// 
        /// </summary>
        private void nextBuil()
        {
            for (int i = 0; i < NumberOfAnt; i++)
            {
                Ant.Add(new List<int>());
                int City = r.Next(0, this.City.Count - 1);

                Ant[i].Add(City);
                Remaining_Cities[i].Remove(City);

                for (int j = 0; j < this.City.Count - 1; j++)//next city
                {
                    cal(i);
                }
                Ant[i].Add(Ant[i][0]);
            }
            if (last_sol.Count > 0)//اضافة الحل السابق
            {
                Ant.Add(last_sol);
            }

            CalBestSolution();
            updateSolution();
            //SolveCh();
        }
       /* private void nextBuil()
        {
            //setRC();
            for (int i = 0; i < NumberOfAnt; i++)
            {
                Ant.Add(new List<int>());
                int City = r.Next(0,name.Count-1);
           
                Ant[i].Add(City);
                Remaining_Cities[i].Remove(City);

               
            }

         for (int i = 0; i < NumberOfAnt; i++)   
            {
                for (int j = 0; j < name.Count-1; j++)
                {
                      cal(i);
                }
                Ant[i].Add(Ant[i][0]);
            }
            if (last_sol.Count > 0)//اضافة الحل السابق
            {
                Ant.Add(last_sol);
            }

           
            for (int i = 0; i < Ant.Count; i++)//حساب المسافة
            {
                int sum = 0;
                for (int j = 1; j < Ant[i].Count ; j++)
                {
                    int antbefor = Ant[i][j - 1];
                    int antnow = Ant[i][j];
                    sum +=value[antbefor][antnow];
                }
                sumOfSlo.Add(sum);
                if (sum < max)
                {
                    solve = i;
                    max = sum;
                }
            }
            last_sol = Ant[solve];
            SolveCh();
        }
        */
        private void CalBestSolution()//Calculate the value of the solution and select the min of value
        {
            for (int i = 0; i < Ant.Count; i++)//حساب المسافة
            {
                double sum = 0;
                for (int j = 1; j < Ant[i].Count; j++)
                {
                    int antbefor = Ant[i][j - 1];
                    int antnow = Ant[i][j];
                    sum += value[antbefor][antnow];
                }
                sumOfSlo.Add(sum);
                if (sum < max)
                {
                    solve = i;
                    max = sum;
                }
            }
            last_sol = Ant[solve];
        }
        private void cal(int ant) //ant: number of ant and Remaining Cities
        {
            List<double> segma = new List<double>();
            double sum = 0.0;
            double x = 0.0;
            double y = 0.0;
            int city = 0;
            for (int i = 0; i < Remaining_Cities[ant].Count; i++) //Calculate the distance from the local city and the remaining cities
            {
                int antinplace = Ant[ant][Ant[ant].Count - 1];
                int antinnext = Remaining_Cities[ant][i];
                segma.Add(Math.Pow(PheromoneList[antinplace][antinnext], Alfa) * Math.Pow(inverse_distance[antinplace][antinnext], Beta));
                sum += segma[i];
            }
            for (int i = 0; i < Remaining_Cities[ant].Count; i++)//Calculate the probability of each city and choose the best
            {
                x = segma[i] / sum;
                
                if (x > y)
                {
                    y = x;
                    city = i;//number of next city
                }
            }
            
            Ant[ant].Add(Remaining_Cities[ant][city]);
            Remaining_Cities[ant].RemoveAt(city);
        }
        /// <summary>
        /// Calculate the pherom of solutıons and update the PheromoneList for new value of best Solution 
        /// </summary>
        public void Pherom()
        {
            //t=(1-P)t' + delta(t)
            double rate = 1.0 - P;
            double Ph = 1.0 / max;

            for (int i = 0; i < PheromoneList.Count; i++)
            {
                for (int j = 0; j < PheromoneList[i].Count; j++)
                {
                    PheromoneList[i][j] *= rate;//evaporation rate
                }
            }
            for (int i = 1; i < Ant[solve].Count; i++)
            {
                PheromoneList[Ant[solve][i-1]][Ant[solve][i]] += Ph;//rate of change
                PheromoneList[Ant[solve][i]][Ant[solve][i-1]] += Ph;

            }

        }
        /*public void SolveCh()
        {
            List<int> n = last_sol;
            Solve = "Solve: ";
            for (int i=0;i< n.Count; i++)
            {
                Solve += name[n[i]]+",";
            }
            valueOfSolve= " value: " +max;
        }*/

        private void updateSolution()
        {
            if (solutions.road_track.Count == 0)
            {
                solutions.road_track.AddRange(last_sol);
                solutions.value = max;
                solutions.change = true;
            }
            else
            {
                for (int i = 0; i < last_sol.Count; i++)
                {
                    if (solutions.road_track[i] != last_sol[i])
                    {
                        solutions.road_track.Clear();
                        solutions.road_track.AddRange(last_sol);
                        solutions.value = max;
                        solutions.change = true;
                        break;
                    }
                    else
                    {
                        solutions.change = false;
                    }
                }


            }

        }
        public void ClearAnt()
        {
            PheromoneList.Clear();
            inverse_distance.Clear();
            Ant.Clear();
            Remaining_Cities.Clear();
            sumOfSlo.Clear();
            max = 1000000;
            last_sol.Clear();
    }
    }
}
