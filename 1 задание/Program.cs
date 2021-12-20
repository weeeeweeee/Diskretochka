using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace program
{
    class Alphabet<Node> // Класс алфавита
    {
        int n;
        Node[] anum; // Нумеруем символы. Всё-таки, с числами работать проще, чем с чем-то абстрактным.

        public Alphabet(List<Node> s)
        {
            HashSet<Node> alph = new HashSet<Node>(s);
            anum = alph.ToArray();
            n = alph.Count;
        }

        public Alphabet(Node[] s)
        {
            HashSet<Node> alph = new HashSet<Node>(s);
            n = alph.Count;
            anum = alph.ToArray();
        }

        public Alphabet(Alphabet<Node> Another)
        {
            n = Another.N;
            anum = Another.anum;
        }

        public int N
        {
            get
            {
                return n;
            }
        }

        public Node this[int i]
        {
            get
            {
                return anum[i];
            }
        }
    }

    abstract class CompObj<Node> // Базовый класс - комбинаторный объект
    {
        abstract public bool Next();
        abstract override public string ToString();
    }

    class PlacementWR<Node> : CompObj<Node> // Размещения с повторениями
    {
        Alphabet<Node> a;
        int k;
        List<int> obj;
        public List<int> Obj
        {
            get
            {
                return obj;
            }
        }
        public PlacementWR(Alphabet<Node> s, int k)
        {
            this.k = k;
            a = new Alphabet<Node>(s);
            obj = new List<int>();
            for (int i = 0; i < k; i++)
                obj.Add(0);
        }

        override public bool Next()
        {
            int i = k - 1;

            while (i >= 0 && obj[i] == a.N - 1)
                i--;

            if (i >= 0)
            {
                obj[i]++;
                i++;
                for (; i < k; i++)
                    obj[i] = 0;
                return true;
            }

            return false;
        }

        override public String ToString()
        {
            String result = "";
            for (int i = 0; i < k; i++)
                result += a[obj[i]].ToString() + " ";

            return result;
        }
    }

    class Permut<Node> : CompObj<Node> // Перестановки
    {
        Alphabet<Node> a;
        int k;
        List<int> obj;

        public Permut(Alphabet<Node> Alf)
        {
            a = new Alphabet<Node>(Alf);
            k = a.N;
            obj = new List<int>();
            for (int i = 0; i < k; i++)
                obj.Add(i);
        }

        private void Swap(int ind1, int ind2)
        {
            int temp = obj[ind1];
            obj[ind1] = obj[ind2];
            obj[ind2] = temp;
        }

        public override bool Next()
        {
            int frst;
            for (frst = k - 2; frst >= 0 && obj[frst] >= obj[frst + 1]; frst--) ;

            if (frst == -1)
                return false;

            int scnd;
            for (scnd = k - 1; scnd >= 0 && obj[frst] >= obj[scnd]; scnd--) ;

            Swap(frst, scnd);

            for (int begin = frst + 1, end = k - 1; begin < end; begin++, end--)
                Swap(begin, end);

            return true;
        }

        public override string ToString()
        {
            String result = "";
            for (int i = 0; i < k; i++)
                result += a[obj[i]].ToString() + " ";

            return result;
        }
    }

    class Placement<Node> : CompObj<Node> // Размещения без повторений
    {
        Alphabet<Node> a;
        int k, n;
        List<int> obj;

        public Placement(Alphabet<Node> Alf, int k)
        {

            a = new Alphabet<Node>(Alf);
            obj = new List<int>();
            this.k = k;
            this.n = a.N;
            for (int i = 0; i < a.N; i++)
                obj.Add(i);
        }

        private void Swap(int ind1, int ind2)
        {
            int temp = obj[ind1];
            obj[ind1] = obj[ind2];
            obj[ind2] = temp;
        }
        public override bool Next()
        {
            int frst;
            do
            {
                for (frst = n - 2; frst >= 0 && obj[frst] >= obj[frst + 1]; frst--) ;

                if (frst == -1)
                    return false;

                int scnd;
                for (scnd = n - 1; scnd >= 0 && obj[frst] >= obj[scnd]; scnd--) ;

                Swap(frst, scnd);

                for (int begin = frst + 1, end = n - 1; begin < end; begin++, end--)
                    Swap(begin, end);

            } while (frst > k - 1);
            return true;
        }

        public override string ToString()
        {
            String result = "";
            for (int i = 0; i < k; i++)
                result += a[obj[i]].ToString() + " ";

            return result;
        }
    }

    class Subset<Node> : CompObj<Node> // Подмножества
    {
        Alphabet<Node> a;
        int k, number;
        List<int> obj;
        public Subset(Alphabet<Node> Alf)
        {
            a = new Alphabet<Node>(Alf);
            number = 0;
            k = a.N;
            obj = new List<int>();
            for (int i = 0; i < k; i++)
                obj.Add(-1);
        }
        public override bool Next()
        {
            if (number == Math.Pow(2, k) - 1)
                return false;
            number++;
            for (int i = k - 1; i >= 0; i--)
            {
                if ((int)(number / Math.Pow(2, k - 1 - i)) % 2 == 1)
                    obj[i] = i;
                else
                    obj[i] = -1;
            }
            return true;
        }

        public override string ToString()
        {
            String result = "";
            if (number == 0)
                result = "Пустое множество";
            else
            {
                result = "{";
                int i;
                for (i = 0; i < k; i++)
                    if (obj[i] != -1)
                    {
                        result += a[obj[i]].ToString();
                        break;
                    }

                for (i++; i < k; i++)
                    if (obj[i] != -1)
                        result += ", " + a[obj[i]].ToString();

                result += "}";
            }
            return result;
        }
    }

    class Combinations<Node> : CompObj<Node> // Сочетания
    {
        Alphabet<Node> a;
        int k, n;
        List<int> obj;
        public Combinations(Alphabet<Node> Alf, int k)
        {
            this.k = k;
            this.n = Alf.N;
            a = new Alphabet<Node>(Alf);
            obj = new List<int>();
            for (int i = 0; i < k; i++)
                obj.Add(i);
        }
        public override bool Next()
        {
            int ind = -1;
            for (int i = k - 1, j = n - 1; i >= 0; i--, j--)
                if (obj[i] != j)
                {
                    ind = i;
                    break;
                }

            if (ind == -1)
                return false;

            obj[ind]++;
            for (int i = ind + 1; i < k; i++)
                obj[i] = obj[i - 1] + 1;


            return true;
        }

        public override string ToString()
        {
            String result = "";
            for (int i = 0; i < k; i++)
                result += a[obj[i]].ToString() + " ";

            return result;
        }
    }

    class CombinationsWR<Node> : CompObj<Node> // Сочетания с повторениями
    {
        Alphabet<Node> a;
        int k;
        List<int> obj;

        public List<int> Obj
        {
            get
            {
                return obj;
            }
        }

        public CombinationsWR(Alphabet<Node> s, int k)
        {
            this.k = k;
            a = new Alphabet<Node>(s);
            obj = new List<int>();
            for (int i = 0; i < k; i++)
                obj.Add(0);
        }

        override public bool Next()
        {
            int i = k - 1;

            while (i >= 0 && obj[i] == a.N - 1)
                i--;

            if (i >= 0)
            {
                obj[i]++;
                i++;
                for (; i < k; i++)
                    obj[i] = obj[i - 1];
                return true;
            }

            return false;
        }

        override public String ToString()
        {
            String result = "";
            for (int i = 0; i < k; i++)
                result += a[obj[i]].ToString() + " ";

            return result;
        }
    }

    class ImpossibleException : Exception
    {

    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите алфавит: ");
            List<char> s = Console.ReadLine().ToList();
            s.RemoveAll(x => x == ' ');
            Alphabet<char> Alf = new Alphabet<char>(s);
            CompObj<char> comb = null;

            bool running = true;
            do
            {
                try
                {
                    Console.WriteLine("Выберите необходимый комбинаторный объект: ");
                    Console.WriteLine("1. Размещения с повторениями по k элементов.");
                    Console.WriteLine("2. Перестановки.");
                    Console.WriteLine("3. Размещения без повторений по k элементов.");
                    Console.WriteLine("4. Все подмножества.");
                    Console.WriteLine("5. Сочетания без повторений по k элементов.");
                    Console.WriteLine("6. Сочетания с повторениями по k элементов,");
                    int choice = Convert.ToInt32(Console.ReadLine());
                    int k;
                    switch (choice)
                    {
                        case 1:
                            Console.Write("Введите k: ");
                            k = Convert.ToInt32(Console.ReadLine());
                            if (k > Alf.N || k < 0)
                                throw new ImpossibleException();
                            comb = new PlacementWR<char>(Alf, k);
                            break;
                        case 2:
                            comb = new Permut<char>(Alf);
                            break;
                        case 3:
                            Console.Write("Введите k: ");
                            k = Convert.ToInt32(Console.ReadLine());
                            if (k > Alf.N || k < 0)
                                throw new ImpossibleException();
                            comb = new Placement<char>(Alf, k);
                            break;
                        case 4:
                            comb = new Subset<char>(Alf);
                            break;
                        case 5:
                            Console.Write("Введите k: ");
                            k = Convert.ToInt32(Console.ReadLine());
                            if (k > Alf.N || k < 0)
                                throw new ImpossibleException();
                            comb = new Combinations<char>(Alf, k);
                            break;
                        case 6:
                            Console.Write("Введите k: ");
                            k = Convert.ToInt32(Console.ReadLine());
                            if (k > Alf.N || k < 0)
                                throw new ImpossibleException();
                            comb = new CombinationsWR<char>(Alf, k);
                            break;
                    }
                }
                catch (ImpossibleException)
                {
                    Console.WriteLine("k должно быть меньше, либо равно n, и больше, либо равно нулю, попробуйте ещё раз...");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
                catch (Exception)
                {
                    Console.WriteLine("Чёт пошло не так, попробуйте ещё раз...");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
                running = false;
            } while (running);

            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter("file.txt");
                do
                {
                    sw.WriteLine(comb.ToString());
                } while (comb.Next());
            }
            finally
            {
                sw.Close();
            }
        }
    }
}
