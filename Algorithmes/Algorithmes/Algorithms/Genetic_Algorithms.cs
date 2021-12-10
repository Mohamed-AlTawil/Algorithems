using Algorithmes.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithm
{

    public class Genetic_Algorithms
    {
        #region parameter:

        Random r = new Random();
       // public string solve;
       // public string valueOfSolve;
       //number of generate solutions
        private int numberOfSolve,Crosswise, Mutation;
        private List<String> City; //list of city
        private List<List<double>> value; //list of distance
       // public List<List<int>> Solves = new List<List<int>>();//list for the first generate solutions
        public List<List<int>> gen_Solves = new List<List<int>>();//list for all solutions
        public List<List<double>> val_gen_Solves = new List<List<double>>();//list for value of solution
        public List<int> selecte_solution = new List<int>();//The solutions that have been selected in gen_Solves list
        Solution solutions =new Solution();
        
        #endregion

        
        public Genetic_Algorithms(int numberOfSolve, List<String> City, List<List<double>> Value, int Crosswise, int Mutation )
        {
            this.numberOfSolve = numberOfSolve;
            this.City = City;
            this.value = Value;
            this.Crosswise = Crosswise;
            this.Mutation = Mutation;
        }
        public Solution Start()
        {
            if (gen_Solves.Count == 0)
            {
                generation();
            }
            Crossover();
            mutlation();
            resatrt();
            return solutions;
        }
        /*
         generate solutions
         */
        public void generation()
        {
           
            int Sol; //rondom
            for (int i = 0; i < numberOfSolve; i++)
            {
                gen_Solves.Add(new List<int>());
                Sol = r.Next(0, City.Count);
                gen_Solves[i].Add(Sol);

                for (int j = 0; j < City.Count - 1; j++)//
                {
                    do
                    {
                        Sol = r.Next(0, City.Count);
                    }
                    while (gen_Solves[i].IndexOf(Sol) != -1);//فحص هل العدد مكرر
                    //{
                    //    Sol = r.Next(0, City.Count);
                    //}
                    gen_Solves[i].Add(Sol);
                }
                gen_Solves[i].Add(gen_Solves[i][0]);//add the last 
                val_gen_Solves.Add(new List<double> { cal(gen_Solves[i]), 0 });//Calculate List < sumvalue , sumvalue/sumallvalue >
            }
           // gen_Solves.AddRange(Solves);
            lucky_circle();


            solutions.road_track = new List<int>();
        }

        //Calculate the value of the solution
        private double cal(List<int> solve)
        {
            double calculate = 0;
            for (int i = 0; i < solve.Count - 1; i++)
            {
                calculate += value[solve[i]][solve[i + 1]];
            }
            return calculate;
        }
        private void lucky_circle()
        {
            double sum = 0;
            for (int i = 0; i < val_gen_Solves.Count; i++)
            {
                sum += 1 / val_gen_Solves[i][0];
            }
            for (int i = 0; i < val_gen_Solves.Count; i++)
            {
                val_gen_Solves[i][1] = 360 / (val_gen_Solves[i][0] * sum);
            }
        }


        /// <summary>
        /// 1- Generate two different random numbers and choose the solution through the lucky circle (crossover_solution() function)
        /// 2-Generate a random number in order to determine the location of the cut
        /// 3-Generate two new solutions through the two selected solutions (new_sol() function)
        /// </summary>
        public void Crossover()
        {
            int cros = Convert.ToInt32((Crosswise*City.Count)/200);
            for (int i = 0; i < cros; i++)
            {
                crossover_solution(2);
                int numCro = r.Next(1, gen_Solves[0].Count - 2);//first and last do not have
                new_sol(selecte_solution[0], selecte_solution[1], numCro);
                new_sol(selecte_solution[1], selecte_solution[0], numCro);
            }
            lucky_circle();
        }
        public void crossover_solution(int NumOfRandom)//num = number of random
        {//توليد عددين عشوائية مختلفين واختيار الحل من خلال دائرة الحظ 
           // int x = -1;
            double y = 0;
            selecte_solution.Clear();
            for (int j = 0; j < NumOfRandom; j++)
            {
                double numCro1 = r.Next(0, 360);
                for (int i = 0; i < gen_Solves.Count; i++)
                {

                    y += val_gen_Solves[i][1];
                    if (numCro1 < y)
                    {
                        if (selecte_solution.Count==0) 
                        {
                            selecte_solution.Add(i);
                            break;
                        }
                        else
                        {
                            if (selecte_solution.IndexOf(i)==-1)
                            {
                                selecte_solution.Add(i);
                                break;
                            }
                        }
                        
                        //x = i;
                       
                    }
                }
            }
        }
        private void new_sol(int sol1, int sol2, int number_of_cut)
        {
            bool check = true;
            List<int> new_Slo = new List<int>();
            for (int i = 0; i < gen_Solves[sol1].Count - 1; i++)//مقارنة عناصر الثانية مع العناصر الثابتة في الاولى
            {
                check = true;
                for (int j = number_of_cut + 1; j < gen_Solves[0].Count - 1; j++)
                {
                    if (gen_Solves[sol2][i] == gen_Solves[sol1][j])
                    {
                        check = false;
                        break;
                    }
                }
                if (check)
                {
                    new_Slo.Add(gen_Solves[sol2][i]);
                }
            }
            for (int j = number_of_cut + 1; j < gen_Solves[0].Count - 1; j++)//اضافة العناصر الثابتة الى الحل الجديد
            {
                new_Slo.Add(gen_Solves[sol1][j]);
            }
            new_Slo.Add(new_Slo[0]);//اضافة العنصر الاخير الذي هو نفس عنصر البداية
            gen_Solves.Add(new_Slo);
            val_gen_Solves.Add(new List<double> { cal(new_Slo), 0 });
        }

        /// <summary>   mutlation() function
        /// 1-Determine the solution from which a new solution will be generated using lucky circle.
        /// 2- Generate two different non-consecutive random numbers
        /// 3-Create a new solution by placing the far element next to the near
        /// </summary>
        public void mutlation()
        {
            List<int> new_Slo ;
          
            crossover_solution((Mutation* numberOfSolve) /100);
            for (int i = 0; i < selecte_solution.Count; i++)
            {
                new_Slo = new List<int>();
                new_Slo.AddRange(gen_Solves[selecte_solution[i]]);
                int num2 = -1;
                int num1 = r.Next(0, gen_Solves[selecte_solution[i]].Count - 2);//توليد عدد عشوائي لتحديد عناصر الطفرة


                //توليد العدد بحيث لا يكون العدد الاخير
                do// Generate two different non-consecutive random numbers
                {
                    num2 = r.Next(0, gen_Solves[selecte_solution[i]].Count - 2);
                } while (num1 == num2 || Math.Abs(num1 - num2) == 1);//تحديد الرقم العشوائي الثاني بحيث لايساوي الاول والفرق بينهما اكبر من 1

                //Create a new solution by placing the far element next to the near
                if (num1 > num2)
                {
                    new_Slo.Insert(num2 + 1, new_Slo[num1]);
                    new_Slo.RemoveAt(num1 + 1);
                }
                else
                {
                    new_Slo.Insert(num1 + 1, new_Slo[num2]);
                    new_Slo.RemoveAt(num2 + 1);
                }

                gen_Solves.Add(new_Slo);
                val_gen_Solves.Add(new List<double> { cal(new_Slo), 0 });
            }
        }

        /// <summary>  resatrt()
        /// Determine the best solutions from the list of generated solutions in order to start a new phase
        /// </summary>
        public void resatrt()
        {
            List<List<int>> Solve= new List<List<int>>();
            Solve.AddRange(gen_Solves);
            gen_Solves.Clear();
            List<List<double>> val_gen_Solvesnew = new List<List<double>>();
            for (int i = 0; i < numberOfSolve; i++)//وضع افضل خمس حلول في مصفوفوفة الحلول
            {
                double x = val_gen_Solves[0][0];
                int y = 0;
                int j = 0;
                while (j < val_gen_Solves.Count)
                {
                    if (x > val_gen_Solves[j][0])
                    {
                        x = val_gen_Solves[j][0];
                        y = j;
                    }
                    j++;
                }
                gen_Solves.Add(Solve[y]);
                val_gen_Solvesnew.Add(val_gen_Solves[y]);
                val_gen_Solves.RemoveAt(y);
            }
           
            val_gen_Solves.Clear();
            selecte_solution.Clear();

            val_gen_Solves.AddRange(val_gen_Solvesnew);
           // gen_Solves.AddRange(Solves);
            lucky_circle();

            updateSolution();

            // SolveCh();
        }

        private void updateSolution()
        {
            if (solutions.road_track.Count == 0)
            {
                solutions.road_track.AddRange(gen_Solves[0]);
                solutions.value = val_gen_Solves[0][0];
                solutions.change = true;
            }
            else
            {
                for (int i = 0; i < gen_Solves[0].Count; i++)
                {
                    if (solutions.road_track[i]!= gen_Solves[0][i])
                    {
                        solutions.road_track.Clear();
                        solutions.road_track.AddRange(gen_Solves[0]);
                        solutions.value = val_gen_Solves[0][0];
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
        public void clear()
        {
          //  Solves.Clear();
            gen_Solves.Clear();
            val_gen_Solves.Clear();
            selecte_solution.Clear();

        }


    }
}
