using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Pract21
{
    class Garden
    {
        private readonly object lockObj = new object();
        private char[,] plot;

        public Garden(int size)
        {
            plot = new char[size, size];
        }

        public void PlantGarden()
        {
            Thread gardener1 = new Thread(Gardener1Work);
            Thread gardener2 = new Thread(Gardener2Work);

            gardener1.Start();
            gardener2.Start();

            gardener1.Join();
            gardener2.Join();
        }

        private void Gardener1Work()
        {
            lock (lockObj)
            {
                int row = 0;
                int col = 0;

                while (row < plot.GetLength(0))
                {
                    plot[row, col] = '1'; // Первый садовник отмечает свой путь '1'
                    Console.WriteLine($"Первый садовник закончил ряд {row}");

                    col++;

                    if (col == plot.GetLength(1))
                    {
                        col--;
                        row++;
                    }

                    Thread.Sleep(100); // Задержка для наглядности
                }
            }
        }

        private void Gardener2Work()
        {
            lock (lockObj)
            {
                int row = plot.GetLength(0) - 1;
                int col = plot.GetLength(1) - 1;

                while (row >= 0)
                {
                    if (plot[row, col] != '1') // Если другой садовник уже отметил этот участок, идем дальше
                    {
                        plot[row, col] = '2'; // Второй садовник отмечает свой путь '2'
                        Console.WriteLine($"Второй садовник закончил ряд {row}");
                    }

                    col--;

                    if (col < 0)
                    {
                        col++;
                        row--;
                    }

                    Thread.Sleep(100); // Задержка для наглядности
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int size = 5;
            Garden garden = new Garden(size);
            garden.PlantGarden();
        }
    }
}