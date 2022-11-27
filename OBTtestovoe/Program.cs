using OBTtestovoe;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml;
//необходима для чтения файлов с вин-1251
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


///<summary>
/// создаем матрицу
/// </summary>
Matrix matrix = new Matrix();
///<summary>
/// выводим матрицу
/// </summary>
matrix.Print();
Console.WriteLine();
///<summary>
/// выводим максимальный элемент
/// </summary>
Console.WriteLine(matrix.Max());
Console.WriteLine();
///<summary>
/// выводим минимальный элемент
/// </summary>
Console.WriteLine(matrix.Min());
Console.WriteLine();
///<summary>
/// выводим сумму по главной диагонали
/// </summary>
Console.WriteLine(matrix.MainDiagonal());
Console.WriteLine();
///<summary>
/// выводим сумму чисел по побочной диагонали
/// </summary>
Console.WriteLine(matrix.SecondDiagonal());
Console.WriteLine();

///<summary>
/// создаем fileModifier
/// </summary>
FileModifier fileModifier = new FileModifier("Тестовое Задание\\Задание3.txt");
///<summary>
/// сортируем 
/// </summary>
fileModifier.Sort();
///<summary>
/// добавляем типы
/// </summary>
fileModifier.AddType();

///<summary>
/// сохраняем изменения
/// </summary>
fileModifier.SaveChanges("Тестовое Задание\\Задание3result.txt");


///<summary>
/// вызываем метод конвертации у статического класса ExelToXML
/// первый параметр путь к ексель файлу
/// второй параметр путь к xml файлу(содастся автоматически)
/// </summary>
ExelToXML.ConvertToXML("Тестовое Задание\\ТестовоеЗадание\\ФайлСИсходнымиДанными.xls", "Тестовое Задание\\ТестовоеЗадание\\result.xml"); 