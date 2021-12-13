using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace ConsoleApp1
{
    internal class Program
    {
        static int min = 999;
        static void foo(int[][] graph, int[] color, int n, int[] answer)
        {
            if (!color.Contains(0))
            {
                if (min > color.Max())
                {
                    min = color.Max();
                    Array.Copy(color, answer, color.Length);
                }
            }
            else
                for (int i = 0; i < n; i++) //C1*n
                {
                    if (color[i] == 0)
                    {
                        for (int z = 1; color[i] == 0; z++) //n^2
                        {
                            color[i] = z;
                            for (int j = 0; j < n && color[i] != 0; j++)
                            {
                                if (color[i] == color[j] && graph[i][j] == 1)
                                    color[i] = 0;
                            }
                        }
                        foo(graph, color, n, answer); //n!
                        color[i] = 0;
                    }
                }
        }
        static int index(int[][] graph, int[] color, int n)
        {
            int max = -1;
            int max_index = -1;
            for (int i = 0; i < n; i++)
            {
                int count = 0;
                if (color[i] == 0)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (graph[i][j] == 1)
                        {
                            count++;
                        }
                    }
                    if (count > max)
                    {
                        max = count;
                        max_index = i;
                    }
                }
            }
            return max_index;
        }

        static void GreedyAlgorithm1(int[][] graph, int[] color, int n)
        {
            for (int i = 0; i < n; i++)
            {
                int c = 1;
                for (int j = 0; j < n; j++)
                {
                    if (color[j] == c && graph[i][j] == 1)
                    {
                        c++;
                        j = 0;
                    }
                }
                color[i] = c;
            }
        }

        static void GreedyAlgorithm2(int[][] graph, int[] color, int n)
        {
            int k;
            for (int z = 0; z < n; z++)
            {
                int i = 1;
                k = index(graph, color, n);
                for (int j = 0; j < n; j++)
                {
                    if (color[j] == i && graph[k][j] == 1)
                    {
                        i++;
                        j = 0;
                    }
                }
                color[k] = i;
            }
        }
        static void PrintGraph(int[][] graph, int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(graph[i][j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        static void PrintColors(int[] color, int n)
        {
            Console.WriteLine($"{color.Max()}");
            for (int i = 0; i < n; i++)
            {
                Console.Write(i + 1 + "-" + color[i].ToString().PadRight(3));
            }
        }

        static int[][] Generation(int n)
        {
            Random rnd = new Random();
            int[][] graph = new int[n][];
            for (int i = 0; i < n; i++)
            {
                graph[i] = new int[n];
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    graph[i][j] = graph[j][i] = rnd.Next(0, 2);
                }
            }
            return graph;
        }

        static int[][] BestGeneration(int n)
        {
            int[][] graph = new int[n][];
            for (int i = 0; i < n; i++)
            {
                graph[i] = new int[n];
            }
            return graph;
        }

        static int[][] BadGeneration(int n)
        {
            int[][] graph = new int[n][];
            for (int i = 0; i < n; i++)
            {
                graph[i] = new int[n];
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    graph[i][j] = graph[j][i] = 1;
                }
            }
            return graph;
        }
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            int n = 0;
            int[][] graph = new int[n][];
            Console.WriteLine("1 - Ручной ввод,\n2 - Генерация случайной матрицы\n3 - Загрузить из файла \"file.txt\"");
            switch (Console.ReadLine().Trim())
            {
                case "1":
                    {
                        Console.Write("Введите размерность матрицы N = ");
                        n = int.Parse(Console.ReadLine());
                        Console.WriteLine("Вводите полностью строки матрицы:");
                        graph = new int[n][];
                        for (int i = 0; i < n; i++)
                        {
                            graph[i] = new int[n];
                            graph[i] = Console.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();
                        }
                        Console.WriteLine();
                    }
                    break;
                case "2":
                    {
                        Console.Write("Введите размерность матрицы N = ");
                        n = int.Parse(Console.ReadLine());
                        Console.WriteLine("Введите способ генерации:\n1 - случайная генерация\n2 - лучший случай (заполнения нулями)\n3 - худший случай (заполнение единицами)");
                        switch (Console.ReadLine().Trim())
                        {
                            case "1":
                                graph = Generation(n);
                                break;
                            case "2":
                                graph = BestGeneration(n);
                                break;
                            case "3":
                                graph = BadGeneration(n);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case "3":
                    {
                        StreamReader read = new StreamReader("file.txt");
                        n = int.Parse(read.ReadLine());
                        graph = new int[n][];
                        for (int i = 0; i < n; i++)
                        {
                            graph[i] = new int[n];
                            graph[i] = read.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();
                        }
                        read.Close();
                        Console.WriteLine("Чтение завершено");
                    }
                    break;
                default:
                    break;
            }

            int[] color = new int[n];
            Console.WriteLine("1 - Метод перебора,\n2 - \"Жадный\" тривиальный\n3 - \"Жадный\" оптимальный");
            switch (Console.ReadLine().Trim())
            {
                case "1":
                    {
                        stopWatch.Restart();
                        int[] answer = new int[n];
                        min = 999;
                        foo(graph, color, n, answer);
                        color = answer;
                        stopWatch.Stop();
                        Console.WriteLine($"N = {n}\nВремя выполнения: {stopWatch.ElapsedMilliseconds} мс");
                    }
                    break;
                case "2":
                    {
                        stopWatch.Restart();
                        GreedyAlgorithm1(graph, color, n);
                        stopWatch.Stop();
                        Console.WriteLine($"N = {n}\nВремя выполнения: {stopWatch.ElapsedMilliseconds} мс");
                    }
                    break;
                case "3":
                    {
                        stopWatch.Restart();
                        GreedyAlgorithm2(graph, color, n);
                        stopWatch.Stop();
                        Console.WriteLine($"N = {n}\nВремя выполнения: {stopWatch.ElapsedMilliseconds} мс");
                    }
                    break;
                default:
                    break;
            }

            //Вывод матрицы на экран
            if (n <= 15)
                PrintGraph(graph, n);

            Console.WriteLine($"Хроматическое число = {color.Max()}\nРаскраска графа (вершина - цвет):");
            for (int i = 0; i < n; i++)
            {
                Console.Write($"{i + 1} - {color[i]}".PadRight(10));
            }

            Console.ReadKey();
        }
    }
}
