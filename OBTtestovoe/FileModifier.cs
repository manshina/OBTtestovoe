using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBTtestovoe
{

    /// <summary>
    /// Класс для изменения файлов из 3 задания
    /// Использует System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance) для отображения текста 
    /// </summary>
    internal class FileModifier
    {
        /// <summary>
        /// Текст из файла
        /// </summary>
        public string Text {get; set; }
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// Закрытое свойство табличного предсавления из текста в виде двумерного массива
        /// </summary>
        private readonly string[,] _matrix;
        /// <summary>
        /// Принимает в качестве параметра путь к файлу и создает представление его содержимого в виде двумерного массива
        /// </summary>
        public FileModifier(string path)
        {
            //кодовая страница 1251 (кириллица windows)
            using (StreamReader reader = new StreamReader(path, Encoding.GetEncoding(1251)))
            {

                string filetext = reader.ReadToEnd();
                Text = filetext;
                FilePath = path;
            }
            //узнаем количество строк в таблице
            string[] columns = Text.Trim().Split("\r\n\r\n\t\t");
            //массив всех элеменов в файле
            string[] items = Text.Trim().Replace("\t", " ").Replace("\n", " ").Replace("\r", " ").Split(" ", StringSplitOptions.RemoveEmptyEntries);


            

            //создаем и заполняем матрицу
            _matrix = new string[columns.Count(), items.Count() / columns.Count()];
            //счетчик для items
            int counter = 0;
            for (int i = 0; i < _matrix.GetLength(0); i++)
            {

                for (int j = 0; j < _matrix.GetLength(1); j++)
                {
                    _matrix[i, j] = items[counter].Replace("_", " ");
                    counter++;
                }
            }
        }
        /// <summary>
        /// Сортирует таблицу по номеру(пузырьком)
        /// </summary>
        public void Sort()
        {
            //номер на ходится в 0 стобце
            int pos = 0;
            //temp
            long temp;
            for (int i = 1; i < _matrix.GetLength(0); i++)
            {
                for (int j = i + 1; j < _matrix.GetLength(1); j++)
                {
                    //приводим стринг к лонгу
                    if (long.Parse(_matrix[i, pos]) > long.Parse(_matrix[j, pos]))
                    {
                        temp = long.Parse(_matrix[i, pos]);
                        _matrix[i, pos] = _matrix[j, pos];
                        _matrix[j, pos] = temp.ToString();
                    }
                }

            }
        }
        /// <summary>
        /// Вывод таблицы в консоль
        /// Использует System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance)
        /// </summary>
        public void Print()
        {
            //устанавливаем вывод в консоль по кодово1 странице 1251 (кириллица windows)
            Console.OutputEncoding = Encoding.GetEncoding(1251);
            for (int i = 0; i < _matrix.GetLength(0); i++)
            {
                for (int j = 0; j < _matrix.GetLength(1); j++)
                {
                    //делаем отступы
                    Console.Write("{0,-18}", _matrix[i, j]);

                }
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Приватный метод для определения типа данных из строки
        /// Поочередно пробуем привести строку в возможный тип
        /// </summary>
        private object ParseString(string str)
        {

            bool boolValue;
            byte byteValue;
            sbyte sbyteValue;
            short shortValue;
            ushort ushortValue;
            int intValue;
            uint uintValue;
            long longValue;
            ulong ulongValue;
            float floatValue;
            double doubleValue;
            decimal decimalValue;
            char charValue;


            if (bool.TryParse(str, out boolValue))
                return boolValue;
            else if (byte.TryParse(str, out byteValue))
                return byteValue;
            else if (sbyte.TryParse(str, out sbyteValue))
                return sbyteValue;
            else if (short.TryParse(str, out shortValue))
                return shortValue;
            else if (ushort.TryParse(str, out ushortValue))
                return ushortValue;
            else if (int.TryParse(str, out intValue))
                return intValue;
            else if (uint.TryParse(str, out uintValue))
                return uintValue;
            else if (long.TryParse(str, out longValue))
                return longValue;
            else if (ulong.TryParse(str, out ulongValue))
                return ulongValue;
            else if (float.TryParse(str, out floatValue))
                return floatValue;
            else if (double.TryParse(str, out doubleValue))
                return doubleValue;
            else if (decimal.TryParse(str, out decimalValue))
                return decimalValue;
            else if (char.TryParse(str, out charValue))
                return charValue;


            return str;
        }
        /// <summary>
        /// Добавляет к "ячкйкам" тип данных
        /// </summary>
        public void AddType()
        {
            for (int i = 1; i < _matrix.GetLength(0); i++)
            {
                for (int j = 1; j < _matrix.GetLength(1); j++)
                {
                    var value = _matrix[i, j];
                    //получаем тип из ParseString
                    _matrix[i, j] += $" '{ParseString(_matrix[i, j]).GetType().Name}'";
                }

            }
        }
        /// <summary>
        /// Сохраняет изменения, проделанные с файлом
        /// </summary>
        public void SaveChanges()
        {
            using (StreamWriter sw = new StreamWriter($"{FilePath}Test.txt"))
            {
                //переносы для сохранения положения как в исходном файле
                Console.WriteLine("\n");
                sw.WriteLine("\n");

                for (int i = 0; i < _matrix.GetLength(0); i++)
                {
                    sw.Write("\t\t");
                    for (int j = 0; j < _matrix.GetLength(1); j++)
                    {

                        sw.Write("{0,-19}", _matrix[i, j]);

                    }
                    sw.WriteLine("\n");
                }
            }
        }
        /// <summary>
        /// Перегрузка сохранения изменений
        /// Принимает путь с названием файла, где сохранить изменения
        /// </summary>
        public void SaveChanges(string path)
        {
            using (StreamWriter sw = new StreamWriter($"{path}"))
            {
                
                Console.WriteLine("\n");
                sw.WriteLine("\n");

                for (int i = 0; i < _matrix.GetLength(0); i++)
                {
                    sw.Write("\t\t");
                    for (int j = 0; j < _matrix.GetLength(1); j++)
                    {

                        sw.Write("{0,-19}", _matrix[i, j]);

                    }
                    sw.WriteLine("\n");
                }
            }
        }
        
    }
}
