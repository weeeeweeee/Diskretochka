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

        override public string ToString()
        {
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

        static class Sets<T>
        {
            public static List<T> FindSubtract(List<T> decr, List<T> sub)
            {
                IEnumerable<T> result = decr.Except(sub);
                return result.ToList();
            }
            public static List<T> FindUnion(List<T> F, List<T> S)
            {
                IEnumerable<T> result = F.Union(S);
                return result.ToList();
            }
        }


        static void Main(string[] args)
        {
            StreamWriter sw = new StreamWriter("file.txt");
            List<int> param = new List<int> { 0, 1, 2, 3, 4, 5 }; // Алфавит из индексов массива 
            List<char> letters = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f' };
            Alphabet<int> alf_ind = new Alphabet<int>(param);
            Alphabet<char> alf_char = new Alphabet<char>(letters);

            Combinations<char> letter = new Combinations<char>(alf_char, 2); // Выбираем 2 буквы

            do {
                Combinations<int> for_first = new Combinations<int>(alf_ind, 2); // Выбираем 2 индекса, на которые будет поставлена первая выбранная буква
                do {
                    Alphabet<int> wo_first = new Alphabet<int>(Sets<int>.FindSubtract(param, for_first.GetNode));
                    Combinations<int> for_second = new Combinations<int>(wo_first, 2); // Выбираем 2 индекса, на которые будет поставлена первая выбранная буква
                    do
                    {
                        List<int> ForF = for_first.GetNode; // Получаем лист индексов, на которые будет поставлена первая выбранная буква
                        List<int> ForS = for_second.GetNode; // Получаем лист индексов, на которые будут поставлена вторая выбранная буква
                        List<int> ForO = Sets<int>.FindSubtract(param, Sets<int>.FindUnion(for_first.GetNode, for_second.GetNode)); // Находим индексы, на которые НЕ будут поставлены выбранные буквы

                        Alphabet<char> not_letter = new Alphabet<char>(Sets<char>.FindSubtract(letters, letter.GetNode)); // Находим все буквы, кроме выбранных
                        Placement<char> P = new Placement<char>(not_letter, 2); // Выбираем 2 символа из алфавита
                        do
                        {
                            char[] result = new char[6]; // Массив для хранения текущего комбинаторного объекта
                            List<char> pl = P.GetNode; // Получаем символы 
                            List<char>.Enumerator iter_pl = pl.GetEnumerator();

                            foreach (int i in ForF) // Расставляем первую выбранную букву на выбранные позиции
                            {
                                result[i] = letter.GetNode[0];
                            }

                            foreach (int i in ForS) // Расставляем вторую выбранную букву на выбранные позиции
                            {
                                result[i] = letter.GetNode[1];
                            }

                            foreach (int i in ForO) // Расставляем 2 оставшиеся буквы
                            {
                                iter_pl.MoveNext();
                                result[i] = iter_pl.Current;
                            }

                            foreach (char c in result) // Выводим результат
                            {
                                sw.Write("{0} ", c);
                            }
                            sw.WriteLine();
                        } while (P.Next());
                    } while (for_second.Next());
                } while (for_first.Next());
            } while (letter.Next());

            sw.Close();
        }
    }
}