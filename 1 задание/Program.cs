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
        Alphabet<Node> a;
        List<int> obj;
        int k;

        public CompObj(Alphabet<Node> s)
        {
            a = new Alphabet<Node>(s);
            obj = new List<int>();
        }

        public Alphabet<Node> A
        {
            get
            {
                return a;
            }
        }

        public int N
        {
            get
            {
                return a.N;
            }
        }

        public int K
        {
            get
            {
                return k;
            }
            protected set
            {
                k = value;
            }
        }

        public List<Node> GetNode
        {
            get
            {
                List<Node> result = new List<Node>();
                for (int i = 0; i < K; i++)
                    result.Add(a[Obj[i]]);
                return result;
            }
        }

        public List<int> Obj
        {
            get
            {
                return obj;
            }
            protected set
            {
                obj = value;
            }
        }

        abstract public bool Next();

        override public string ToString() {
            String result = "";
            foreach (Node i in GetNode)
                result += i.ToString() + " ";
            return result;
        }
    }

    class PlacementWR<Node> : CompObj<Node> // Размещения с повторениями
    {
        public PlacementWR(Alphabet<Node> s, int k) : base(s)
        {
            K = k;
            for (int i = 0; i < k; i++)
                Obj.Add(0);
        }

        override public bool Next()
        {
            int i = K - 1;

            while (i >= 0 && Obj[i] == N - 1)
                i--;

            if (i >= 0)
            {
                Obj[i]++;
                i++;
                for (; i < K; i++)
                    Obj[i] = 0;
                return true;
            }

            return false;
        }
    }

    class Permut<Node> : CompObj<Node> // Перестановки
    {
        public Permut(Alphabet<Node> Alf) : base(Alf)
        {
            K = N;
            for (int i = 0; i < K; i++)
                Obj.Add(i);
        }

        private void Swap(int ind1, int ind2)
        {
            int temp = Obj[ind1];
            Obj[ind1] = Obj[ind2];
            Obj[ind2] = temp;
        }

        public override bool Next()
        {
            int frst;
            for (frst = K - 2; frst >= 0 && Obj[frst] >= Obj[frst + 1]; frst--) ;

            if (frst == -1)
                return false;

            int scnd;
            for (scnd = K - 1; scnd >= 0 && Obj[frst] >= Obj[scnd]; scnd--) ;

            Swap(frst, scnd);

            for (int begin = frst + 1, end = K - 1; begin < end; begin++, end--)
                Swap(begin, end);

            return true;
        }
    }

    class Placement<Node> : CompObj<Node> // Размещения без повторений
    {
        public Placement(Alphabet<Node> Alf, int k) : base(Alf)
        {
            K = k;
            for (int i = 0; i < N; i++)
                Obj.Add(i);
        }

        private void Swap(int ind1, int ind2)
        {
            int temp = Obj[ind1];
            Obj[ind1] = Obj[ind2];
            Obj[ind2] = temp;
        }
        public override bool Next()
        {
            int frst;
            do
            {
                for (frst = N - 2; frst >= 0 && Obj[frst] >= Obj[frst + 1]; frst--) ;

                if (frst == -1)
                    return false;

                int scnd;
                for (scnd = N - 1; scnd >= 0 && Obj[frst] >= Obj[scnd]; scnd--) ;

                Swap(frst, scnd);

                for (int begin = frst + 1, end = N - 1; begin < end; begin++, end--)
                    Swap(begin, end);

            } while (frst > K - 1);
            return true;
        }
    }

    class Subset<Node> : CompObj<Node> // Подмножества
    {
        int number;

        public Subset(Alphabet<Node> Alf) : base(Alf)
        {
            number = 0;
            K = N;
            for (int i = 0; i < K; i++)
                Obj.Add(-1);
        }
        public override bool Next()
        {
            if (number == Math.Pow(2, K) - 1)
                return false;
            number++;
            for (int i = K - 1; i >= 0; i--)
            {
                if ((int)(number / Math.Pow(2, K - 1 - i)) % 2 == 1)
                    Obj[i] = i;
                else
                    Obj[i] = -1;
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

                for (i = 0; i < K; i++)
                    if (Obj[i] != -1)
                    {
                        result += A[Obj[i]].ToString();
                        break;
                    }

                for (i++; i < K; i++)
                    if (Obj[i] != -1)
                        result += ", " + A[Obj[i]].ToString();

                result += "}";
            }
            return result;
        }
    }

    class Combinations<Node> : CompObj<Node> // Сочетания
    {
        public Combinations(Alphabet<Node> Alf, int k) : base(Alf)
        {
            K = k;
            for (int i = 0; i < K; i++)
                Obj.Add(i);
        }
        public override bool Next()
        {
            int ind = -1;
            for (int i = K - 1, j = N - 1; i >= 0; i--, j--)
                if (Obj[i] != j)
                {
                    ind = i;
                    break;
                }

            if (ind == -1)
                return false;

            Obj[ind]++;
            for (int i = ind + 1; i < K; i++)
                Obj[i] = Obj[i - 1] + 1;


            return true;
        }
    }

    class CombinationsWR<Node> : CompObj<Node> // Сочетания с повторениями
    {
        public CombinationsWR(Alphabet<Node> s, int k) : base(s)
        {
            K = k;
            for (int i = 0; i < K; i++)
                Obj.Add(0);
        }

        override public bool Next()
        {
            int i = K - 1;

            while (i >= 0 && Obj[i] == N - 1)
                i--;

            if (i >= 0)
            {
                Obj[i]++;
                i++;
                for (; i < K; i++)
                    Obj[i] = Obj[i - 1];
                return true;
            }

            return false;
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
