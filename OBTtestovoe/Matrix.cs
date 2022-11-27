using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBTtestovoe
{
    internal class Matrix
    {
        /// <summary>
        /// Двумерный массив
        /// </summary>
        private int[,] matrix;
        /// <summary>
        /// Конструктор заполняет матрицу случайными элементами
        /// </summary>
        public Matrix(int a = 4, int b = 5)
        {
            matrix = new int[a, b];
            Random rnd = new Random();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    //для облегчения проверки числа от 1 до 20
                    matrix[i, j] = rnd.Next(1,20);
                }
            }
        }
        /// <summary>
        /// Вывод матрицы
        /// </summary>
        public void Print()
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write("{0} ", matrix[i, j]);

                }
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Сумма чисел по главной диагонали
        /// </summary>
        public int MainDiagonal()
        {
            int main = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                main += matrix[i, i];               
            }
            return main;
            
        }
        /// <summary>
        /// Сумма чисел по побочной диагонали
        /// </summary>
        public int SecondDiagonal()
        {
            int second = 0;
            int j = matrix.GetLength(1) - 1;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                second += matrix[i, j];
                j--;              
            }
            return second;
            
        }

        /// <summary>
        /// Поиск максимального значения в матрице
        /// </summary>
        public int Max()
        {
            //за максимальное число берем 0
            int max = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] > max)
                    {
                        max = matrix[i, j];
                    }
                }
            }
            return max;
        }
        /// <summary>
        /// Поиск минимального значения в матрице
        /// </summary>
        public int Min()
        {
            //за минимальное число берем первый элемент матрицы
            int min = matrix[0, 0];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] < min)
                    {
                        min = matrix[i, j];
                    }
                }
            }
            return min;
        }
       
    }
}
