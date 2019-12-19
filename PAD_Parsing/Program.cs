using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace PAD_Parsing
{
    class Program
    {
        //static IList<double> t = new List<double>();
        //static IList<double> Pks = new List<double>();
        //static IList<double> Pgc = new List<double>();
        //static IList<double> Tks = new List<double>();
        //static IList<double> Tgc = new List<double>();
        //static IList<double> e = new List<double>();
        //static IList<double> Stt = new List<double>();
        //static IList<double> Mt = new List<double>();
        //static IList<double> Gt = new List<double>();
        //static IList<double> Mist = new List<double>();
        //static IList<double> Gist = new List<double>();
        //static IList<double> xIO = new List<double>();
        //static IList<double> vIO = new List<double>();
        //static IList<double> aIO = new List<double>();

        static void Main(string[] args)
        {
            try
            {
                Console.Write("Введите путь к основному файлу: ");
                var textPath = Console.ReadLine();
                Console.WriteLine("____________________________________________________________________________________________");
                Console.Write("Введите путь к первому доп. файлу: ");
                var textPath1 = Console.ReadLine();
                Console.Write("Введите коэффициент деления силы сопртивления для этого файла: ");
                var fc1 = Console.ReadLine();
                Console.WriteLine("____________________________________________________________________________________________");
                Console.Write("Введите путь ко второму доп. файлу: ");
                var textPath2 = Console.ReadLine();
                Console.Write("Введите коэффициент деления силы сопртивления для этого файла: ");
                var fc2 = Console.ReadLine();
                Console.WriteLine("____________________________________________________________________________________________");
                Console.Write("Введите путь к третьему доп. файлу: ");
                var textPath3 = Console.ReadLine();
                Console.Write("Введите коэффициент деления силы сопртивления для этого файла: ");
                var fc3 = Console.ReadLine();
                Console.WriteLine("____________________________________________________________________________________________");

                string text = File.ReadAllText(textPath);
                string text1 = File.ReadAllText(textPath1);
                string text2 = File.ReadAllText(textPath2);
                string text3 = File.ReadAllText(textPath3);

                var data = CreateNormalizedData();
                var data1 = CreateNormalizedData();
                var data2 = CreateNormalizedData();
                var data3 = CreateNormalizedData();

                GetDataFromREZ(text, data);
                GetDataFromREZ(text1, data1);
                GetDataFromREZ(text2, data2);
                GetDataFromREZ(text3, data3);

                PlotsFromNormalizedData(new string[] { fc1, fc2, fc3 }, data, data1, data2, data3);
            }
            catch (Exception)
            {
                Console.WriteLine("____________________________________________________________________________________________");
                Console.WriteLine("____________________________________________________________________________________________");
                Console.WriteLine("Неверный файл. Программа завершает работу");
                Console.WriteLine("____________________________________________________________________________________________");
                Console.ReadKey();
            }            
        }

        static IList<List<double>> CreateNormalizedData()
        {
            IList<List<double>> data = new List<List<double>>();

            data.Add(new List<double>());
            data.Add(new List<double>());
            data.Add(new List<double>());
            data.Add(new List<double>());
            data.Add(new List<double>());
            data.Add(new List<double>());
            data.Add(new List<double>());
            data.Add(new List<double>());
            data.Add(new List<double>());
            data.Add(new List<double>());
            data.Add(new List<double>());
            data.Add(new List<double>());
            data.Add(new List<double>());
            data.Add(new List<double>());
            data.Add(new List<double>());

            return data;
        }

        static void GetDataFromREZ(string REZ_Data, IList<List<double>> normalizedData)
        {
            string[] lines = REZ_Data.Split('\n');

            string[] lineItems;
            List<string> info = new List<string>();

            for (int i = 0; i < lines.Length - 35; i++)
            {
                lineItems = lines[i + 35].Split(" ");

                if (lineItems[0].Equals("t="))
                {
                    continue;
                }

                for (int j = 0; j < lineItems.Length; j++)
                {
                    if (!lineItems[j].Equals(""))
                    {
                        info.Add(lineItems[j]);
                    }
                }

                if (info.Count == 9)
                {
                    normalizedData[0].Add(Double.Parse(info[0], CultureInfo.InvariantCulture));
                    normalizedData[1].Add(Double.Parse(info[1], CultureInfo.InvariantCulture));
                    normalizedData[3].Add(Double.Parse(info[2], CultureInfo.InvariantCulture));
                    normalizedData[5].Add(Double.Parse(info[3], CultureInfo.InvariantCulture));
                    normalizedData[6].Add(Double.Parse(info[4], CultureInfo.InvariantCulture));
                    normalizedData[7].Add(Double.Parse(info[5], CultureInfo.InvariantCulture));
                    normalizedData[8].Add(Double.Parse(info[6], CultureInfo.InvariantCulture));
                    normalizedData[9].Add(Double.Parse(info[7], CultureInfo.InvariantCulture));
                    normalizedData[10].Add(Double.Parse(info[8], CultureInfo.InvariantCulture));
                }

                if (info.Count == 5)
                {
                    normalizedData[2].Add(Double.Parse(info[0], CultureInfo.InvariantCulture));
                    normalizedData[4].Add(Double.Parse(info[1], CultureInfo.InvariantCulture));
                    normalizedData[11].Add(Double.Parse(info[2], CultureInfo.InvariantCulture));
                    normalizedData[12].Add(Double.Parse(info[3], CultureInfo.InvariantCulture));
                    normalizedData[13].Add(Double.Parse(info[4], CultureInfo.InvariantCulture));
                }

                info.Clear();
            }            
        }

        static void PlotsFromNormalizedData(
            string[] fc,
            IList<List<double>> normalizedData, 
            IList<List<double>> normalizedData1, 
            IList<List<double>> normalizedData2, 
            IList<List<double>> normalizedData3)
        {
            Plot4("Давление в камере сгорания от времени", fc, normalizedData[0], normalizedData[1], normalizedData1[0], normalizedData1[1], normalizedData2[0], normalizedData2[1], normalizedData3[0], normalizedData3[1], "Время, с", "Давление, ат");
            Plot4("Давление в силовом цилиндре от времени", fc, normalizedData[0], normalizedData[2], normalizedData1[0], normalizedData1[2], normalizedData2[0], normalizedData2[2], normalizedData3[0], normalizedData3[2], "Время, с", "Давление, ат");
            Plot4("Температура в камере сгорания от времени", fc, normalizedData[0], normalizedData[3], normalizedData1[0], normalizedData1[3], normalizedData2[0], normalizedData2[3], normalizedData3[0], normalizedData3[3], "Время, с", "Температура, К");
            Plot4("Температура в силовом цилиндре от времени", fc, normalizedData[0], normalizedData[4], normalizedData1[0], normalizedData1[4], normalizedData2[0], normalizedData2[4], normalizedData3[0], normalizedData3[4], "Время, с", "Температура, К");
            Plot4("Толщина сгоревшего свода от времени", fc, normalizedData[0], normalizedData[5], normalizedData1[0], normalizedData1[5], normalizedData2[0], normalizedData2[5], normalizedData3[0], normalizedData3[5], "Время, с", "Толщина, мм");
            Plot4("Величина горящей поверхности от времени", fc, normalizedData[0], normalizedData[6], normalizedData1[0], normalizedData1[6], normalizedData2[0], normalizedData2[6], normalizedData3[0], normalizedData3[6], "Время, с", "Толщина, м2");
            Plot4("Масса сгоревшего топлива от времени", fc, normalizedData[0], normalizedData[7], normalizedData1[0], normalizedData1[7], normalizedData2[0], normalizedData2[7], normalizedData3[0], normalizedData3[7], "Время, с", "Масса, кг");
            Plot4("Величина газоприхода в камеру сгорания от времени", fc, normalizedData[0], normalizedData[8], normalizedData1[0], normalizedData1[8], normalizedData2[0], normalizedData2[8], normalizedData3[0], normalizedData3[8], "Время, с", "Расход, кг/с");
            Plot4("Масса истекших газов от времени", fc, normalizedData[0], normalizedData[9], normalizedData1[0], normalizedData1[9], normalizedData2[0], normalizedData2[9], normalizedData3[0], normalizedData3[9], "Время, с", "Масса, кг");
            Plot4("Величина газорасхода от времени", fc, normalizedData[0], normalizedData[10], normalizedData1[0], normalizedData1[10], normalizedData2[0], normalizedData2[10], normalizedData3[0], normalizedData3[10], "Время, с", "Расход, кг/с");
            Plot4("Координаты исполнительного органа от времени", fc, normalizedData[0], normalizedData[11], normalizedData1[0], normalizedData1[11], normalizedData2[0], normalizedData2[11], normalizedData3[0], normalizedData3[11], "Время, с", "Перемещение, м");
            Plot4("Скорость движения исполнительного органа от времени", fc, normalizedData[0], normalizedData[12], normalizedData1[0], normalizedData1[12], normalizedData2[0], normalizedData2[12], normalizedData3[0], normalizedData3[12], "Время, с", "Скорость, м/с");
            Plot4("Ускорение исполнительного органа от времени", fc, normalizedData[0], normalizedData[13], normalizedData1[0], normalizedData1[13], normalizedData2[0], normalizedData2[13], normalizedData3[0], normalizedData3[13], "Время, с", "Ускорение, м/с2");
        }

        static void Plot4(
            string title,
            string[] fc,
            IList<double> x, IList<double> y,
            IList<double> x1, IList<double> y1,
            IList<double> x2, IList<double> y2,
            IList<double> x3, IList<double> y3,            
            string xL, string yL)
        {
            try
            {
                var plot = new ScottPlot.Plot(800, 600);
                plot.Title(title);
                plot.PlotScatter(x.ToArray(), y.ToArray(), label: "Fc");
                plot.PlotScatter(x1.ToArray(), y1.ToArray(), label: "Fc/" + fc[0]);
                plot.PlotScatter(x2.ToArray(), y2.ToArray(), label: "Fc/" + fc[1]);
                plot.PlotScatter(x3.ToArray(), y3.ToArray(), label: "Fc/" + fc[2]);
                plot.XLabel(xL);
                plot.YLabel(yL);
                plot.Legend();
                var path = Path.GetFullPath(title + ".png");
                plot.SaveFig(path);
                Console.WriteLine("____________________________________________________________________________________________");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine("График \"" + title + "\" построен");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine("Расположение файла: " + path);
                Console.WriteLine("____________________________________________________________________________________________");
                Console.WriteLine("");
            }
            catch (Exception)
            {
                Console.WriteLine("____________________________________________________________________________________________");
                Console.WriteLine("____________________________________________________________________________________________");
                Console.WriteLine("Ошибка при построении графика " + title);
                Console.WriteLine("____________________________________________________________________________________________");
            }
        }        
    }
}
