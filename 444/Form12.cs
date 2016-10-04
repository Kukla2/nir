using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Threading;
using _444;

namespace _444
{
    public partial class Form1 : Form
    {
        public List<Category> Stek_links = new List<Category>();//категории по idшнику
        public Category link_of_id = new Category();
        public List<Category> Name_links = new List<Category>();//категории по имени
        public Category link_of_name = new Category();
        public List<Category> Storage_articles = new List<Category>();// статьи по имени
        public List<Category> Stek_articles = new List<Category>();// статьи по idшнику
        public List<Category> Slova = new List<Category>();// ссылки в статье
        public Category slovo = new Category();
        public List<Connect> idCon = new List<Connect>();// связи между статьями
        public Connect con = new Connect();
        public List<Weight> Ratio = new List<Weight>();// отношение связей
        public Weight rat = new Weight();
        public List<Fracion> Fract = new List<Fracion>();
        public List<int> DataDomain = new List<int>(); //предметная область
        // public List<Weight> graf = new Weight();
        public int r = 1, u = 1, e = 1, ut = 0, mesto = 0, ove = 0, rove = 0, j = 0, ooover = 0;
        public bool Rip = false;
        public bool nis = false;
        public Form1()
        {
            InitializeComponent();
        }
        public struct Category
        {
            public int IdCat;// сколько раз ссылка повторялась
            public string NameCat;
            public string LinkCat;
        }

        public struct Connect
        {
            public int node;// узел
            public string nodeName;
            public int leaf; //лист
            public string leafName;
        }

        public struct Weight //Список связей вершины
        {
            public int ver;// вершина
            public int vesI;//исходящие
            public int vesV;//входящие
            public double otn;//отношение
        }

        public struct Fracion //Коэфициент принадлежности вершины предметной области
        {
            public int root; //вершина
            public double ratio; //отношение
        }

        public string get_http(string url) //get запрос 
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:17.0) Gecko/20100101 Firefox/17.0";
                req.Headers.Add("DNT", "1");
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                string text = sr.ReadToEnd();
                resp.Close();
                sr.Close();
                return text;
            }
            catch (WebException)
            {
                //richTextBox1.Text += Convert.ToString(Stek_articles[r - 2].NameCat) + /*"\n" + Storage_articles[r - 23].NameCat + */ "\n" + Convert.ToString(Stek_links[Stek_links.Count - 2].NameCat);
                return "";
            }
            catch (UriFormatException)
            {
                return "";
            }
            catch (ArgumentNullException)
            {
                return "";
            }
        }

        public List<Category> search_untouched_link(List<Category> ddd, Category root, List<Category> ooo)

// 3) Собирает категории в два массива            

            //  ищет ссылки и вставляет если их нет
        {
            Category[] arts = ddd.ToArray(); //массив из категорий
            int w = 0;
            foreach (Category ioi in ddd) //из списка в массив
            {
                arts[w] = ioi; w++;
            }
            int i, j, k, f, cres;
            int length = arts.Length;//Размер 
            i = 0;//Начало 
            j = length - 1;//Конец 
            f = 0;//Флаг обнаружения в массиве 
            while (i <= j)//Поиск до схождения в точку, равенство чтобы гарантировать хоть 1 сравнение 
            {
                k = (i + j) / 2;//Середина 
                cres = String.Compare(arts[k].NameCat, root.NameCat);//Сравнили 
                if (cres == 0)//Нашли 
                {
                    f = 1;
                    i = k;
                    j = k - 1;
                }
                if (cres < 0)//Нужное после середины 
                { i = k + 1; }
                if (cres > 0)//Нужное перед серединой 
                {
                    if (j == i)//если ушли в точку - корректируем, чтобы не было бесконечного цикла 
                        j = i - 1;
                    else
                        j = k;
                }
            }
            //Теперь i или позиция для вставки или позиция найденного элемента 
            if (f == 0)//проверили что не нашли - вставляем 
            {
                Array.Resize(ref arts, arts.Length + 1);//увеличили массив 
                j = length - 1;
                while (j >= i)//подвинули 
                {
                    arts[j + 1] = arts[j];
                    j--;
                }
                arts[i] = new Category() { IdCat = root.IdCat, LinkCat = root.LinkCat, NameCat = root.NameCat };
                //вставили в стек
                ooo.Add(root);
                //Name_links.Add(link_of_id);
            }
            ddd.Clear();
            for (int pi = 0; pi < arts.Length; pi++) //преобразовываем обратно в Category
            {
                ddd.Add(arts[pi]);
            }
            return ddd;
        }

        public Category search_waste_link(List<Category> yyy, Category wood)

// 5) Выбростить или присвоить id статье

            //List<object> выбрасываем слова которых нет в области
        {
            Category[] arts = yyy.ToArray(); //массив из категорий
            int w = 0;
            foreach (Category ioi in yyy) //из списка в массив
            {
                arts[w] = ioi; w++;
            }
            int i, j, k = 0, f, cres;
            int length = arts.Length;//Размер 
            i = 0;//Начало 
            j = length - 1;//Конец 
            f = 0;//Флаг обнаружения в массиве 
            while (i <= j)//Поиск до схождения в точку, равенство чтобы гарантировать хоть 1 сравнение 
            {
                k = (i + j) / 2;//Середина 
                cres = String.Compare(arts[k].NameCat, wood.NameCat);//Сравнили 
                if (cres == 0)//Нашли 
                {
                    mesto = arts[k].IdCat;
                    f = 1;
                    i = k;
                    j = k - 1;
                }
                if (cres < 0)//Нужное после середины 
                { i = k + 1; }
                if (cres > 0)//Нужное перед серединой 
                {
                    if (j == i)//если ушли в точку - корректируем, чтобы не было бесконечного цикла 
                        j = i - 1;
                    else
                        j = k;
                }
            }
            //Теперь i или позиция для вставки или позиция найденного элемента 
            if (f == 1)//  ---  НАШЛИ, НАДО ВставиТЬ
            {
                //Array.Resize(ref arts, arts.Length + 1);//увеличили массив 
                //j = length - 1;
                //while (j >= i)//подвинули 
                //{
                //    arts[j + 1] = arts[j];
                //    j--;
                //}
                // arts[i] = new Category() { IdCat = wood.IdCat, LinkCat = wood.LinkCat, NameCat = wood.NameCat };
                //вставили в стек
                wood = new Category { IdCat = arts[k].IdCat, NameCat = arts[k].NameCat, LinkCat = arts[k].NameCat };
                // con = new Connect { node = arts[k].IdCat, nodeName = arts[k].NameCat, leaf = wood.IdCat, leafName = wood.NameCat };
                // ggg.Add(con); ooover++;

                //ggg.Add(wood);

                // wood.IdCat = arts[k].IdCat;
                //idCon.Add(wood);
            }
            else
            {
                //wood = new Category { NameCat = " не пренадлежит области" }; 
                nis = false;
            }
            // yyy.Clear();
            //for (int pi = 0; pi < arts.Length; pi++) //преобразовываем обратно в Category
            //{
            //    yyy.Add(arts[pi]);
            //}
            return wood;
        }

        public List<Category> Collect_connecting(Category ppp)

// 4) Пройти по списку статей

            // собирает слова в статье
        {
            bool gran = false; int vv = 0; int index = 0;
            string wikiText = get_http(ppp.LinkCat);
            List<string> words = (wikiText.Split(new[] { '>', '<', '"' }, StringSplitOptions.RemoveEmptyEntries).ToList());// Разбиваем строку на массив строк
            for (int i = 0; i < words.Count; i++)
            {
                if (words[i] == ("contentSub"))
                {
                    for (int p = i; p < words.Count; p++)
                    {
                        if (words[p] == ("p") || words[p] == ("li") || words[p] == ("td"))
                        {
                            for (int k = p; k < words.Count; k++)
                            {
                                //исключения
                                if (words[k] == ("mw-editsection-divider"))
                                {
                                    gran = true;
                                }
                                if (words[k] == ("mw-editsection-bracket"))
                                {
                                    gran = true;
                                }

                                if (words[k] == (" title="))
                                {
                                    p = k;
                                    if (gran == false)
                                    {
                                        slovo = new Category() { IdCat = 0, NameCat = words[k + 1], LinkCat = "https://ru.wikipedia.org" + words[k - 1] }; e++;
                                        //Slova.Add(slovo);
                                        nis = true;
                                        slovo = search_waste_link(Storage_articles, slovo); //List<object> выбрасываем слова которых нет в области
                                        if (nis == true)
                                        {
                                            con = new Connect { node = ppp.IdCat, nodeName = ppp.NameCat, leaf = slovo.IdCat, leafName = slovo.NameCat }; vv++;
                                            // поиска в Storage_articles
                                            // search_untouched_link(Storage_articles, slovo);
                                            idCon.Add(con); ooover++;
                                            // con.leaf = search_waste_link(Stek_articles, con).leaf;//проверяем ее в списке старниц
                                        }
                                    }
                                    else
                                    {
                                        gran = false;
                                        break;
                                    }
                                }
                            }
                        }
                        if (words[p] == ("reference-text"))// отделяет обзацы    mw-editsection-bracket   ol class=
                        {
                            goto Yup;//кончилась страница
                        }
                    }
                }
            }
        Yup:
            return Slova;

        }

        public /*string[]*/ List<Category> Parser(string url, bool check)
        {
            Category stiker = new Category();
            // string way = @"d:\test\TestFile.txt";// записывает массив строк в новый файл!!!
            string wikiText = get_http(url);
            List<string> words = (wikiText.Split(new[] { '>', '<', '"' }, StringSplitOptions.RemoveEmptyEntries).ToList());// Разбиваем строку на массив строк
            for (int i = 0; i < words.Count; i++)
            {
                if (check == true)
                //для категорий

                // 2а) Записывать в Name_links в алфавитном порядке
                {
                    if (words[i] == "mw-subcategories")
                    {
                        for (int k = i; k < words.Count; k++)
                        {
                            if (words[k] == ("CategoryTreeChildren"))
                            {
                                link_of_id = new Category() { IdCat = u, LinkCat = "https://ru.wikipedia.org" + words[k - 13], NameCat = words[k - 12] }; u++;//нашли категорию
                                search_untouched_link(Name_links, link_of_id, Stek_links);//поиск повторений
                            }
                        }
                        goto NextAddress;//кончилась страница
                    }
                }
                if (check == false)
                //для статей
                {
                    if (words[i] == ("mw-pages"))
                    {
                        for (int k = i; k < words.Count; k++)
                        {
                            if (words[k] == ("mw-normal-catlinks"))
                            {
                                goto NextAddress;//кончилась страница
                            }
                            if (words[k] == ("li")/* & (words[k+4].Contains("Портал:")==false)*/)//" title="+1
                            {
                                stiker = new Category() { IdCat = r, LinkCat = "https://ru.wikipedia.org" + words[k + 2], NameCat = words[k + 4] }; r++;
                                search_untouched_link(Storage_articles, stiker, Stek_articles);//поиск повторений
                                ove++;
                            }
                        }
                    }
                }
            }
        NextAddress:
            return Stek_links;//sss ;
        }

        public void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int over_link = 0;
            int stop = 0;
            //int j = 0;

            // 1) Пройти по корневой ссылке

            link_of_name = new Category() { IdCat = 0, LinkCat = "https://ru.wikipedia.org/wiki/Категория:Космос", NameCat = "Космос" };
            Stek_links.Add(link_of_name);
            Name_links.Add(link_of_name);
        NextAddress: stop++; over_link++;
            //if (over_link > 10) //стопарик slog.Count
            //{
            //    goto Out;
            //}
            //if (j - 1 == 10)// собрать только 3 штуки
            //{
            //    goto Out;
            //}
            // if (j == Stek_links.Count) goto Out;

            if (Stek_links[j].NameCat == "Межзвёздная среда‎‎") goto Out;//Спутники столкнувшиеся на орбите или повреждённые космическим мусором

            // 2) Собрать категории в два массива

            Parser(Stek_links[j].LinkCat, false);//статьи
            Parser(Stek_links[j].LinkCat, true);//категории
            j++;

            goto NextAddress;
        Out:
            //for (int r = 0; r < idCon.Count; r++)
            //{
            //    richTextBox1.Text += Convert.ToString(idCon[r].leafName) + " - " + Convert.ToString(idCon[r].nodeName) + "\n";
            //}
            //for (int r = 0; r < Stek_links.Count; r++)
            //{
            //    richTextBox1.Text += Convert.ToString(Stek_links[r].NameCat) + "\n";
            //}
            int uiu = 0;
            for (int r = 0; r < Stek_articles.Count; r++)//Stek_articles.Count
            {
                Collect_connecting(Stek_articles[r]); ut++; uiu++;// берет слова из каждой статьи
                //if (uiu == 50)
                //{

                // 6) Добавляем в граф

                string way = @"d:\test\Graf.txt";
                StreamWriter file = new StreamWriter(way);
                //    запись в файл


                //               for (int p = 0; p < Slova.Count; p++)
                for (int p = 0; p < idCon.Count; p++)
                {
                    //                       file.WriteLine(Convert.ToString(Slova[p].IdCat) + ") " + Convert.ToString(Slova[p].NameCat)+ "\n");
                    file.WriteLine(Convert.ToString(idCon[p].node) + ":" + Convert.ToString(idCon[p].leaf) + ";\n");
                    //                        file.WriteLine(Convert.ToString(idCon[p].node) + ") " + Convert.ToString(idCon[p].nodeName) + " ------ " + Convert.ToString(idCon[p].leaf) + ")" + Convert.ToString(idCon[p].leafName) + "\n");
                }
                file.Close();

                //string way1 = @"d:\test\Kategorii.txt";
                //StreamWriter file1 = new StreamWriter(way1);
                //for (int p = 0; p < Stek_links.Count; p++)
                //{
                //    file1.WriteLine(Convert.ToString(Stek_links[p].IdCat) + " --- " + Convert.ToString(Stek_links[p].NameCat) + "\n ");
                //}
                //file1.Close();

                //string way2 = @"d:\test\Slova.txt";
                //StreamWriter file2 = new StreamWriter(way2);

                //for (int p = 0; p < Stek_articles.Count; p++)
                //{
                //    file2.WriteLine(Convert.ToString(Stek_articles[p].IdCat) + " --- " + Convert.ToString(Stek_articles[p].NameCat) + "\n" + "\n");
                //}
                //file2.Close();


                //    uiu = 0;
                //}
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // StreamReader file = new StreamReader(@"d:\test\Graf.txt");
            //int i = 1000000;
            //int [,] arr = new int [i,2];
            // for (int y = 0; y>


            StreamReader file = new StreamReader(@"d:\test\Graf.txt"); //Открываем файл для чтения
            string str = ""; //Объявляем переменную, в которую будем записывать текст из файла

            while (!file.EndOfStream) //Цикл длиться пока не будет достигнут конец файла
            {
                str += file.ReadLine(); //В переменную str по строчно записываем содержимое файла
            }
            // richTextBox1.Text += str;
            //foreach (int oii in arr)
            //{
            //    if(
            //}
            //List<int> ver = new List<int>();
            //Задать вершину
            //    найти все вершины связанные связанные с ней
            //        найти отношение числа связей к общему числу вершин

            int tutu = 0; int tyty = 0; double pop = 0.0;
            string[] wor = str.Split(new[] { ':', ';' });// Разбиваем строку на массив строк
            string way1 = @"d:\test\foi1.txt";
            double colst = 354111.0;
            StreamWriter filef = new StreamWriter(way1);
            for (int t = 1; t < 35425; t++)// связь 
            {
                tutu = 0; tyty = 0;
                for (int koy = 0; koy < wor.Length; koy += 2)// ищем везде
                {
                    if ((t == Convert.ToInt32(wor[koy])))
                    {
                        tutu++;
                    }
                    if (t == Convert.ToInt32(wor[koy + 1]))
                    {
                        tyty++;
                    }
                    //Weight pup = new Weight { ver = t, ves = tutu };
                }

                pop = (double)((tutu + tyty) / colst);
                rat = new Weight { ver = t, vesI = tutu, vesV = tyty, otn = pop };
                filef.WriteLine("[" + t + "] - " + tutu + " | " + tyty + " (" + rat.otn + ") ;\n");
            }
            richTextBox1.Text += "каниц\n";
            filef.Close();
            //int tutu = 0;
            //for (int t = 0; t > idCon.Count; t++)
            //{
            //    for (int koy = 0; koy > idCon.Count; koy++) //каждая связь
            //    {
            //        if (( t==idCon[koy].leaf ) || (t==idCon[koy].node)) 
            //        {
            //            tutu++;
            //        } 
            //        //Weight pup = new Weight { ver = t, ves = tutu };
            //    }
            //    richTextBox1.Text += Convert.ToString(t) + " - " + Convert.ToString(tutu) + "\n";

            //}


            file.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(System.Drawing.Color.Black);
            //PointF X = new PointF(); 
            //PointF Y = new PointF(); 
            //formGraphics.DrawCubicCurve(myPen, 3.5F, 4.5F, X, Y, 103F, 24F, 34F, 4F, 12F, 23F, 3F, 24F);
            //formGraphics.DrawCubicCurve(myPen, 3.5F, 4.5F, X, Y, 103F, 904F, 234F, 53F, 17F, 3F, 2F, 35F);
            formGraphics.DrawEllipse(myPen, 1, 1, 700, 700);// проверка
            //int R = 300; double X, Y = 0;
            //double rad = 0;
            //double grad = 0;
            //int colst = 354111; double del = 0.0010166304;
            // // Point[] pppp = new Point[colst];
            //  for (int gr = 0; gr < colst; gr ++)
            //  {
            //      rad = (grad * (Math.PI / 180));
            //      Y = (double)(R * Convert.ToInt32(Math.Cos(rad)));
            //      X = (double)(R * Convert.ToInt32(Math.Sin(rad)));
            //      grad = grad + del;
            //      richTextBox1.Text += Convert.ToString(X + ";\n");
            //      Point pppp = new Point (X,Y);
            //  }



            //        Point[] points = {
            //new Point(60, 60),
            //new Point(150, 80),
            //new Point(200, 40),
            //new Point(180, 120),
            //new Point(120, 100),
            //new Point(80, 160)};

            //Pen pen = new Pen(Color.FromArgb(255, 0, 0, 255));
            // e.Graphics.DrawClosedCurve(pen, points);



            //// глаз
            //formGraphics.DrawArc(myPen, 40, 40, 40, 40, 180, -180);

            //// открытый глаз
            //formGraphics.DrawEllipse(myPen, 120, 40, 40, 40);

            //// нос
            //formGraphics.DrawBezier(myPen, 100, 60, 120, 100, 90, 120, 80, 100);

            //// рот
            //Point[] apt = new Point[4];
            //apt[0] = new Point(60, 140);
            //apt[1] = new Point(140, 140);
            //apt[2] = new Point(100, 180);
            //apt[3] = new Point(60, 140);
            //formGraphics.DrawCurve(myPen, apt, 0, 3, 0.9f);

            //myPen.Dispose();
            //formGraphics.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<int> Candidate = new List<int>(); // тут потенциальные кандидаты на ПО
            int Ver = Convert.ToInt32(textBox3.Text); // номер вершины
            for (int z = 0; z < idCon.Count; z++) // пройти по списку вершин
            {
                if (idCon[z].node == Ver)
                {
                    Candidate.Add(idCon[z].leaf); // набрать кандидатов (всех, на кого ссылается Ver)
                }
            }
            //посчитать отношение
            int dr = 0; //счетчик кандидатов
            int svaaaz = 0;
            for (int rr = 0; rr < Candidate.Count; rr++) // пройтись по кандидатам
            {
                if (idCon[rr].node == Candidate[dr]) // есть ссылка между кандидатами
                {
                    for (int ds = 0; ds < Candidate.Count; ds++)
                    {
                        if (idCon[rr].leaf == Candidate[ds])// интересные нам виды связей
                        {
                            svaaaz++;
                        }
                    }
                    dr++;
                    Fracion fra = new Fracion { root = idCon[rr].leaf, ratio = (svaaaz / Convert.ToDouble(Candidate.Count)) };
                }
            }



            //private void pictureBox1_Click(object sender, EventArgs e)
            //{
            //    //e.Graphics.DrawEllipse(MyPen, 50, 50, 250, 250);
            //    //e.Graphics.DrawLine(MyPen, 175, 175, x2, y2);
            //}


            //
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string sr = System.IO.File.ReadAllText(@"d:\test\Raf.txt").Replace("\n", " ");
            //StreamReader file5 = new StreamReader(@"d:\test\Raf.txt"); //Открываем файл для чтения
            //string sr = ""; //Объявляем переменную, в которую будем записывать текст из файла

            //while (!file5.EndOfStream) //Цикл длиться пока не будет достигнут конец файла
            //{
            //    sr += file5.ReadLine(); //В переменную str по строчно записываем содержимое файла
            //}
            string[] wor = sr.Split(new[] { ':', ';' });// Разбиваем строку на массив строк
           // StreamWriter filef = new StreamWriter(wa);
            for (int t = 1; t < 977214 / 2; t++)// связь 
            {
                con = new Connect { leaf = Convert.ToInt32(wor[t+1]), leafName = "", node = Convert.ToInt32(wor[t]), nodeName = ""};
                idCon.Add(con);
            }
           // file.Close();
        }
    }

        static class GraphicsExtension
        {
            public static void DrawCubicCurve(this Graphics graphics, Pen pen, float beta, float step, PointF start, PointF end, float a3, float a2, float a1, float a0, float b3, float b2, float b1, float b0)
            {
                float xPrev, yPrev;
                float xNext, yNext;
                bool stop = false;

                xPrev = beta * a0 + (1 - beta) * start.X;
                yPrev = beta * b0 + (1 - beta) * start.Y;

                for (float t = step; ; t += step)
                {
                    if (stop)
                        break;

                    if (t >= 1)
                    {
                        stop = true;
                        t = 1;
                    }

                    xNext = beta * (a3 * t * t * t + a2 * t * t + a1 * t + a0) + (1 - beta) * (start.X + (end.X - start.X) * t);
                    yNext = beta * (b3 * t * t * t + b2 * t * t + b1 * t + b0) + (1 - beta) * (start.Y + (end.Y - start.Y) * t);

                    graphics.DrawLine(pen, xPrev, yPrev, xNext, yNext);

                    xPrev = xNext;
                    yPrev = yNext;
                }
            }

            /// <summary>
            /// Рисует кривую B-сплайна через заданный массив точечных структур.
            /// </summary>
            /// <param name="pen">Ручка для рисования линий.</param>
            /// <param name="points">Массив контрольных точек, которые определяют сплайн.</param>
            /// <param name="beta">Прочность на изгиб, 0 <= beta <= 1.</param>
            /// <param name="step">Шаг кривой рисования, определяет качество рисунка, 0 < step <= 1</param>
            internal static void DrawBSpline(this Graphics graphics, Pen pen, PointF[] points, float beta, float step)
            {
                if (points == null)
                    throw new ArgumentNullException("Массив точек не должен быть пустым.");

                if (beta < 0 || beta > 1)
                    throw new ArgumentException("Прочность на изгиб должен быть >= 0 and <= 1.");

                if (step <= 0 || step > 1)
                    throw new ArgumentException("Шаг должен быть > 0 and <= 1.");

                if (points.Length <= 1)
                    return;

                if (points.Length == 2)
                {
                    graphics.DrawLine(pen, points[0], points[1]);
                    return;
                }

                float a3, a2, a1, a0, b3, b2, b1, b0;
                float deltaX = (points[points.Length - 1].X - points[0].X) / (points.Length - 1);
                float deltaY = (points[points.Length - 1].Y - points[0].Y) / (points.Length - 1);
                PointF start, end;

                {
                    a0 = points[0].X;
                    b0 = points[0].Y;

                    a1 = points[1].X - points[0].X;
                    b1 = points[1].Y - points[0].Y;

                    a2 = 0;
                    b2 = 0;

                    a3 = (points[0].X - 2 * points[1].X + points[2].X) / 6;
                    b3 = (points[0].Y - 2 * points[1].Y + points[2].Y) / 6;

                    start = points[0];
                    end = new PointF
                    (
                      points[0].X + deltaX,
                      points[0].Y + deltaY
                    );

                    graphics.DrawCubicCurve(pen, beta, step, start, end, a3, a2, a1, a0, b3, b2, b1, b0);
                }

                for (int i = 1; i < points.Length - 2; i++)
                {
                    a0 = (points[i - 1].X + 4 * points[i].X + points[i + 1].X) / 6;
                    b0 = (points[i - 1].Y + 4 * points[i].Y + points[i + 1].Y) / 6;

                    a1 = (points[i + 1].X - points[i - 1].X) / 2;
                    b1 = (points[i + 1].Y - points[i - 1].Y) / 2;

                    a2 = (points[i - 1].X - 2 * points[i].X + points[i + 1].X) / 2;
                    b2 = (points[i - 1].Y - 2 * points[i].Y + points[i + 1].Y) / 2;

                    a3 = (-points[i - 1].X + 3 * points[i].X - 3 * points[i + 1].X + points[i + 2].X) / 6;
                    b3 = (-points[i - 1].Y + 3 * points[i].Y - 3 * points[i + 1].Y + points[i + 2].Y) / 6;

                    start = new PointF
                    (
                      points[0].X + deltaX * i,
                      points[0].Y + deltaY * i
                    );

                    end = new PointF
                    (
                      points[0].X + deltaX * (i + 1),
                      points[0].Y + deltaY * (i + 1)
                    );

                    graphics.DrawCubicCurve(pen, beta, step, start, end, a3, a2, a1, a0, b3, b2, b1, b0);
                }

                {
                    a0 = points[points.Length - 1].X;
                    b0 = points[points.Length - 1].Y;

                    a1 = points[points.Length - 2].X - points[points.Length - 1].X;
                    b1 = points[points.Length - 2].Y - points[points.Length - 1].Y;

                    a2 = 0;
                    b2 = 0;

                    a3 = (points[points.Length - 1].X - 2 * points[points.Length - 2].X + points[points.Length - 3].X) / 6;
                    b3 = (points[points.Length - 1].Y - 2 * points[points.Length - 2].Y + points[points.Length - 3].Y) / 6;

                    start = points[points.Length - 1];

                    end = new PointF
                    (
                      points[0].X + deltaX * (points.Length - 2),
                      points[0].Y + deltaY * (points.Length - 2)
                    );

                    graphics.DrawCubicCurve(pen, beta, step, start, end, a3, a2, a1, a0, b3, b2, b1, b0);
                }
            }
        }
    }

