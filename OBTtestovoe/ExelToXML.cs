using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OBTtestovoe
{
    /// <summary>
    /// Класс с единственным методом ConvertToXML
    /// </summary>
    internal static class ExelToXML
    {
        /// <summary>
        /// Принимает путь Exel файла и путь, куда сохранить результат в формате XML
        /// </summary>
        public static void ConvertToXML(string path, string resultpath)
        {
            /// <summary>
            /// Получаем данные из екскля с помощью ExelReader
            /// </summary>         
            ExelReaer exelReaer = new ExelReaer(path);
            //инициализация документа
            XDocument xdoc = new XDocument();
            //корневой элемент
            XElement root = new XElement("RootXml");
            //элемент и его атрибуты
            XElement schema = new XElement("SchemaVersion");
            XAttribute schemaVersion = new XAttribute("Number", 2);

            //элемент и его атрибуты
            XElement date = new XElement("Period");
            XAttribute datevalue = new XAttribute("Date", $"{File.GetLastWriteTime(path).Date.ToString("yyyy-MM-dd")}");

            //добавляем элементы и атрибуты
            date.Add(datevalue);

            schema.Add(schemaVersion);

            root.Add(schema);
            root.Add(date);

            //элемент и его атрибуты
            XElement source = new XElement("Source");
            XAttribute sourceClassCode = new XAttribute("ClassCode", "ДМС");
            XAttribute sorceCode = new XAttribute("Code", "819");
            //добавляем элементы и атрибуты
            source.Add(sourceClassCode);
            source.Add(sorceCode);
            date.Add(source);
            //элемент и его атрибуты
            XElement form = new XElement("Form");
            XAttribute formCode = new XAttribute("Code", "178");
            XAttribute formName = new XAttribute("Name", "Счета в кредитных организациях");

            XAttribute formStatus = new XAttribute("Status", "0");
            //добавляем элементы и атрибуты
            form.Add(formCode);
            form.Add(formName);
            form.Add(formStatus);

            //переменная, что бы задать формат отображения для decimal
            var format = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            format.NumberGroupSeparator = " ";

            //получаем таблицы из ридера
            foreach (System.Data.DataTable table in exelReaer.DataSet.Tables)
            {
                //список с разными кодами счета бюджетного учета
                List<string> codes = new List<string>();
                for (int i = 3; i < table.Rows.Count; i++)
                {
                    //высчитываем коды счета бюджетного учета
                    string code = $"1{table.Rows[i].ItemArray[1]}";
                    code = code.Remove(code.Length - 3) + "000";
                    if (!codes.Contains(code))
                    {
                        codes.Add(code);
                    }
                }
                //выделенно 3 типа кодов, идем по ним
                foreach (var itemCode in codes)
                {
                    //элемент и его атрибуты
                    XElement document = new XElement("Document");

                    XAttribute plsch = new XAttribute("ПлСч11", $"{itemCode}");

                    //массив для хранения контрольных сумм
                    decimal[] string960Values = new decimal[] { 0m, 0m, 0m, 0m };

                    //добавляем атрибут
                    document.Add(plsch);
                    //счетчик для элемента data
                    int dataCounter = 1;
                    //идем по таблице сразу со строки со значениями
                    for (int i = 3; i < table.Rows.Count; i++)
                    {
                        //высчитываем коды счета бюджетного учета
                        string code = $"1{table.Rows[i].ItemArray[1]}";
                        code = code.Remove(code.Length - 3) + "000";
                        if (code == itemCode)
                        {
                            //счетчик элемента Px
                            int columnNumCounter = 1;
                            //вписываем данные для контрольных сумм
                            for (int value = 0; value < string960Values.Length; value++)
                            {
                                string960Values[value] += Convert.ToDecimal(table.Rows[i].ItemArray[value + 2]);
                            }
                            //создаем элемент и его атрибуты
                            XElement data = new XElement("Data");
                            XAttribute dataString = new XAttribute("СТРОКА", dataCounter.ToString("000"));
                            data.Add(dataString);
                            dataCounter++;
                            //идем по столбцам таблицы
                            for (int j = 0; j < table.Columns.Count; j++)
                            {
                                //исключаем столбец с кодами
                                if (j != 1)
                                {
                                    //создаем элемент и его атрибуты
                                    XElement PxElement = new XElement("Px");

                                    XAttribute number = new XAttribute("Num", columnNumCounter);

                                    PxElement.Add(number);
                                    columnNumCounter++;
                                    //преобразования для корректного вывода ячеек в decimal и string
                                    string? input = table.Rows[i].ItemArray[j]?.ToString();
                                    decimal value;

                                    var result = decimal.TryParse(input, out value);
                                    if (result)
                                    {
                                        XAttribute PxValue = new XAttribute("Value", value.ToString("0.00", format));
                                        PxElement.Add(PxValue);
                                    }
                                    else
                                    {
                                        XAttribute PxValue = new XAttribute("Value", $"{table.Rows[i].ItemArray[j]}");
                                        PxElement.Add(PxValue);
                                    }
                                    //добавляем получившийся элемент
                                    data.Add(PxElement);
                                    //получаем значения из шапки
                                    if ((i == 3))
                                    {

                                        XElement column = new XElement("Column");
                                        XAttribute columnNum = new XAttribute("Num", $"{columnNumCounter - 1}");

                                        column.Add(columnNum);

                                        if (string.IsNullOrWhiteSpace(table.Rows[0].ItemArray[j]?.ToString()))
                                        {
                                            XAttribute columnName = new XAttribute("Name", $"{table.Rows[1].ItemArray[j]} {table.Rows[0].ItemArray[j - 1]}");
                                            column.Add(columnName);
                                        }
                                        else
                                        {
                                            XAttribute columnName = new XAttribute("Name", $"{table.Rows[1].ItemArray[j]} {table.Rows[0].ItemArray[j]}");
                                            column.Add(columnName);
                                        }

                                        form.Add(column);

                                    }
                                }

                            }
                            //добовляем элемент
                            document.Add(data);

                        }

                    }
                    //счетчик для контрольных сумм
                    int columnNum960Counter = 2;
                    //элемент и атрибуты контрольных сумм
                    XElement sumdata = new XElement("Data");
                    XAttribute sumDataString = new XAttribute("СТРОКА", "960");
                    sumdata.Add(sumDataString);
                    for (int value = 0; value < string960Values.Length; value++)
                    {
                        XElement sumPxElement = new XElement("Px");
                        
                        XAttribute sumnumber = new XAttribute("Num", columnNum960Counter);
                        
                        XAttribute sumvalue = new XAttribute("Value", $"{string960Values[value].ToString("0.00", format)}");
                        sumPxElement.Add(sumnumber);
                        sumPxElement.Add(sumvalue);
                        sumdata.Add(sumPxElement);
                        columnNum960Counter++;
                    }

                    document.Add(sumdata);

                    form.Add(document);
                }

            }
            //добавляем элементы
            source.Add(form);

            xdoc.Add(root);
            //сохраняем результат во второй параметр
            xdoc.Save(resultpath);
            
        }
    }
}
