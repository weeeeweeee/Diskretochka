using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Program
{
    class Program
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

            public List<Node> GetNode
            {
                get
                {
                    List<Node> result = new List<Node>();
                    for (int i = 0; i < k; i++)
                        result.Add(a[obj[i]]);
                    return result;
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

            public List<Node> GetNode
            {
                get
                {
                    List<Node> result = new List<Node>();
                    for (int i = 0; i < k; i++)
                        result.Add(a[obj[i]]);
                    return result;
                }
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
            public List<int> Obj
            {
                get
                {
                    return obj;
                }
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
        static List<int> FindSubtract(List<int> decr, List<int> sub)
        {
            IEnumerable<int> result = decr.Except(sub);
            return result.ToList();
        }
        static void Main(string[] args)
        {
            StreamWriter sw = new StreamWriter("file.txt");
            List<int> param = new List<int>{ 0, 1, 2, 3, 4 }; // Алфавит из индексов массива 
            Alphabet<int> alf = new Alphabet<int>(param);
            Combinations<int> C = new Combinations<int>(alf, 2); // Выбираем 2 индекса, на которые будет поставлена буква "а"
            do
            {
                List<int> ForA = C.Obj; // Получаем лист индексов, на которые будет поставлены буква "а"
                List<int> ForB = FindSubtract(param, C.Obj); // Находим индексы, на которые НЕ будет поставлена буква "а"
                Alphabet<char> bcdef = new Alphabet<char> (new List<char>{'b', 'c', 'd', 'e', 'f' }); 
                Placement<char> P = new Placement<char>(bcdef, 3); // Выбираем 3 символа из алфавита
                // Достаточно изменить эту строку на следующую, если нужно, чтобы символы могли повторяться
                //PlacementWR<char> P = new PlacementWR<char>(bcdef, 3);

                do
                {
                    char[] result = new char[5]; // Массив для хранения текущего комбинаторного объекта
                    List<char> pl = P.GetNode; // Получаем символы 
                    List<char>.Enumerator iter = pl.GetEnumerator();

                    foreach (int i in ForA) // Расставляем буквы "а" на выбранные позиции
                        result[i] = 'a';

                    foreach (int i in ForB) // Расставляем 3 из множества {b, c, d, e, f} выбранные буквы 
                    {
                        iter.MoveNext();
                        result[i] = iter.Current;
                    }

                    foreach(char c in result) // Выводим результат
                    {
                        sw.Write("{0} ", c);
                    }
                    sw.WriteLine();
                } while (P.Next());
            } while (C.Next());

            sw.Close();
        }
    }
}
