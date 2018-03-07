////////////Аленький цветочек не получился на основе расчета векторами, даже предварительные замеры показывают
////////////что получается медленнее чем native (возможно еще не до конца понимаю векторы), 
////////////в дальнейшем дебаге не вижу смысла. 
////////////Самый быстрый и простой вариант выводится вначале.

using System;
using System.Linq;


namespace ConsoleApp1
{
    class CalcArr
    {

        static void Main(string[] args)
        {
            int[,] resArr = new int[9, 5];
            int[,] cliArr = new int[9, 5];
            int[,] ordArr = new int[9, 5];

            if (System.IO.File.Exists("clients.txt"))
            {
                System.Diagnostics.Stopwatch swatch = new System.Diagnostics.Stopwatch(); // создаем объект

                try
                {

                    //System.Numerics.Vector<int> lm = new System.Numerics.Vector<int>(0);
                    //int Step = System.Numerics.Vector<int>.Count;

                    ///////////////Читаем
                    cliArr = FileCli("clients.txt");
                    ordArr = FileBi("orders.txt");

                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 5; j++) { Console.Write(cliArr[i, j] + "\t"); }
                        Console.WriteLine();
                    }

                    ///////////Складываем
                    swatch.Start(); // старт

                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 5; j++) { resArr[i, j] = cliArr[i, j] + ordArr[i, j]; }
                    }

                    swatch.Stop();
                    Console.WriteLine(swatch.Elapsed);

                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 5; j++) { Console.Write(resArr[i, j] + "\t");  }
                        Console.WriteLine();
                    }

                    //swatch.Start(); // старт
                        resArr = FileVect();
                    //swatch.Stop();

                    //Console.WriteLine(swatch.Elapsed);

                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 5; j++) { Console.Write(resArr[i, j] + "\t"); }
                        Console.WriteLine();
                    }

                    Console.ReadLine();

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error file: " + e.Message);
                    Console.ReadLine();
                }

            }
            else { Console.WriteLine("Не удалось найти файл"); }

        }
        ///////////////Чтение/Разбор файла
        public static int[,] FileCli(String Filename)
        {
            string[] stchar;
            int i = 0, j = 0;
            int[,] cliArr = new int[9, 5];
            int stout;

            var FileX = System.IO.File.ReadAllLines(Filename);
            //cliArr[FileX.,FileX]

            foreach (var sttr in FileX)
            {
                stchar = sttr.Split("\t");
                j = 0;

                foreach (var stch in stchar)
                {
                    if (int.TryParse(stch, out stout) == false) {
                        //Console.WriteLine("строка: " + i);
                        continue; }
                    cliArr[i, j] = stout;
                    //Console.WriteLine(stout);
                    j++;
                }
                i++;
            }
            ////////Возвращаем массив чисел
            return cliArr;
        }

        ///////////////Чтение/Разбор файла
        public static int[,] FileBi(String Filename)
        {
            string[] stchar;
            int i = 0;
            int[,] ordArr = new int[9, 5];


            ///////Тут мы считываем ордера и преобразуем строковые значения в числовые
            ///////первый и третий параметр - номер строки и номер столбца, второй знак числа
            ///////последние два числа

            var FileX = System.IO.File.ReadAllLines(Filename);


            //cliArr[FileX.,FileX]

            foreach (var sttr in FileX)
            {
                stchar = sttr.Split("\t");
                try
                {
                    ////////Здесь заполняю двухмерный массив, но этот двухмерный массив можно переписать
                    ////////на векторах.
                    int IndStr = int.Parse(stchar[0].Substring(1)) - 1;
                    Char IndCh = Char.Parse(stchar[2]);
                    int IndCol = (int)IndCh - 65;
                    int Znak = 0;
                    if (stchar[1] == "b") { Znak = 1; } else { Znak = -1; }
                    int Dollars = int.Parse(stchar[4]) * Znak;
                    int Papers = int.Parse(stchar[3]) * Znak;

                    if (Papers != 0) { ordArr[IndStr, IndCol] = ordArr[IndStr, IndCol] + Papers; }
                    if (Dollars != 0) { ordArr[IndStr, 4] = ordArr[IndStr, 4] + Dollars; }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка формата файла ордеров " + e.Message);
                    Console.ReadLine();
                }

                i++;
            }
            ////////Возвращаем массив чисел
            return ordArr;
        }

        ///////////////////////////////////
        /////////Расчет на основе векторов
        public static int[,] FileVect()
        {
            string[] stchar;
            int i = 0, j = 0;
            int[,] cliArr = new int[9, 5];
            int[,] ordArr = new int[9, 5];
            int stout;

            ///////Читаем файлы
            var FileCli = System.IO.File.ReadAllLines("clients.txt");
            var FileOrd = System.IO.File.ReadAllLines("orders.txt");

            foreach (var sttr in FileCli)
            {
                stchar = sttr.Split("\t");
                j = 0;

                foreach (var stch in stchar)
                {
                    if (int.TryParse(stch, out stout) == false)
                    {
                        continue;
                    }
                    cliArr[i, j] = stout;
                    j++;
                }
                i++;
            }

            //    ///////Заполняем 4 вектора
            //    //string[] ColA = FileOrd.Where(entry =>
            //    //(entry?.Split("\t")[2] == "A"))?.ToArray();

            //    //string[] ColB = FileOrd.Where(entry =>
            //    //(entry?.Split("\t")[2] == "B"))?.ToArray();

            //    //string[] ColC = FileOrd.Where(entry =>
            //    //(entry?.Split("\t")[2] == "C"))?.ToArray();

            //    //string[] ColD = FileOrd.Where(entry =>
            //    //(entry?.Split("\t")[2] == "D"))?.ToArray();

            //    ////////Переписываю с форами и молотилкой привидения к виду
                Array.Sort(FileOrd);

            //    ////////Все ABCD для ячейки C1, C1 b A, C1 s A, C1 b B, C1 b
            //    //////за один такт нужно посчитать минимум 4 ячейки

            //    ////Получаем значения для одномерки
            //    ////Будет форыч основанный на счетчиках, будем шагать при смене буквы и ячейки, при этом сразу считать.
            //    ////координаты это счетчики
            int M = 0; int mstep = 0;
            bool flagS = false;
            int strNum = 0; int oldstrNum = 0;
            int colNum = 0; int oldcolNum = 0;
            int[] CalcMassive = new int[800];
            int[] resArr = new int[4];
            int SUMM = 0;
            int Dollars = 0;
            //float[] resArr2 = new float[2];

            System.Diagnostics.Stopwatch swatch = new System.Diagnostics.Stopwatch();
            swatch.Start();

            foreach (var sttr in FileOrd)            
            //for (i=0; i <= FileOrd.Length; i++)
            {
                //String sttr = FileOrd[i];
                stchar = sttr.Split("\t");
                //flagS = false;
                ////////Запоминаем текущую строку, как только меняется + и обновляем
                if (strNum != int.Parse(stchar[0].Substring(1)) - 1 ||
                    colNum != (int)Char.Parse(stchar[2]) - 65)
                {
                    strNum = int.Parse(stchar[0].Substring(1)) - 1;
                    colNum = (int)Char.Parse(stchar[2]) - 65;

                    int Znak = 0;
                    if (stchar[1] == "b") { Znak = 1; } else { Znak = -1; }
                    Dollars = Dollars + int.Parse(stchar[4]) * Znak;

                    //Console.WriteLine(CalcMassive.Sum());
                    //////Вычисляем кратность 8 количества элементов, будет происходить смещение на 4 индекса, 
                    //////разложил для удобства
                    if (mstep%8!=0) { M = ((((mstep/8)+1)*8)+4); } else { M = mstep+4; }
                    //////Начиная с четырех элементов с шагом в 8 будет 4-8, 0-4; 12-16, 8-12
                    for (i = 4; i <= M; i += System.Numerics.Vector<int>.Count + 4)
                    {
                        //////Заполняем первые 8
                                                
                        System.Numerics.Vector<int> aSimd = new System.Numerics.Vector<int>(CalcMassive, i);
                        System.Numerics.Vector<int> bSimd = new System.Numerics.Vector<int>(CalcMassive, i - 4);

                        System.Numerics.Vector<int> cSimd = aSimd + bSimd;
                        
                        cSimd.CopyTo(resArr);
                        
                        int arrsum = resArr.Sum();
                        
                        SUMM = SUMM + arrsum;
                    }
                    
                    Array.Clear(CalcMassive, 0, mstep);
                    Array.Clear(resArr, 0, resArr.Length);
                    
                    //////////Сумму ячейки считаем мы
                    cliArr[oldstrNum, oldcolNum] = cliArr[oldstrNum, oldcolNum] + SUMM;
                    cliArr[oldstrNum, 4] = cliArr[oldstrNum, 4] + Dollars;
                    oldstrNum = strNum;
                    oldcolNum = colNum;
                    Dollars = 0;
                    SUMM = 0;
                    mstep = 0;


                }
                else
                {
                    int Znak = 0;
                    if (stchar[1] == "b") { Znak = 1; } else { Znak = -1; }
                    CalcMassive[mstep] = int.Parse(stchar[3]) * Znak;
                    Dollars = Dollars + int.Parse(stchar[4]) * Znak;
                    mstep++;
                }

            }

            swatch.Stop();
            Console.WriteLine(swatch.Elapsed);
            return cliArr;
        }

    }
}
