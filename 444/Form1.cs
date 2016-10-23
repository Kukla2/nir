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
using System.Media;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;


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
        public int r = 1, u = 1, e = 1, ut = 0, mesto = 0, ove = 0, rove = 0, j = 0, ooover = 0, sizeDD = 0;
        public double greed = 0.0; //количество связей в ПО
        public bool Rip = false;
        public bool nis = false;
        public string way, wayy, wat, wap, wer, swey, slovaey, otvey, serway = "";
         //public int ds;
        public BinaryFormatter formatter = new BinaryFormatter();
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
        [Serializable]
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
            public double sv; //связей кличество
            public double ratioN; //отношение
            public double ratioK;
        }
        [Serializable]
        public class Graf
        {
            public int vhod { get; set; }
            public int vyhod { get; set; }
            public class GrafCollection
            {
                //public Graf Collection { get; set; }
                //vhod = Vhod;
                //vyhod = Vyhod;
            }
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

        //  ищет ссылки и вставляет если их нет
        public List<Category> search_untouched_link(List<Category> ddd, Category root, List<Category> ooo)
        {
// 3) Собирает категории в два массива     
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

        // выбрасываем слова которых нет в области
        public Category search_waste_link(List<Category> yyy, Category wood)

// 5) Выбростить или присвоить id статье

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

        public double search_untouched_connect(List<Connect> svazi, int candid, List<int> po) 
            //количество связей кандидата с ПО
        /*List<Category> ddd, Category root, List<Category> ooo*/ 
        {   
            Connect[] arts = svazi.ToArray(); //массив из категорий
            int w = 0;
            int CanSvaz = 0;
            int dk = 0;
            foreach (Connect ioi in svazi) //из списка в массив
            {
                arts[w] = ioi; w++;
            }
            int i, j, k, f, cres;
            int length = arts.Length;//Размер 
            i = 0;//Начало 
            j = length - 1;//Конец 
            f = 0;//Флаг обнаружения в массиве 
            int y = 1;
            while (i <= j)//Поиск до схождения в точку, равенство чтобы гарантировать хоть 1 сравнение 
            {
                k = (i + j) / 2;//Середина 
               
                //cres = String.Compare(Convert.ToString(arts[k].node), Convert.ToString(candid));//Сравнили 
                //dk = po.IndexOf(arts[k].leaf);//есть ли ПО
                //cres = String.Compare(arts[k].leaf, po[pl]);
                //dk = po.IndexOf(arts[i].leaf);//есть ли ПО

                //if ((cres == 0)/* && */)//Нашли 
                //{
                //    j = k;
                //}

                if (arts[k].node == candid)//Нашли 
                {
                    f = 1; 
                    //i = k;
                    j = k - 1;
                }
                
                if (arts[k].node < candid)//Нужное после середины 
                { i = k + 1; }
                if (arts[k].node > candid)//Нужное перед серединой 
                {
                    if (j == i)//если ушли в точку - корректируем, чтобы не было бесконечного цикла 
                        j = i - 1;
                    else
                        j = k;
                }
            }
            //Теперь i или позиция для вставки или позиция найденного элемента 
            if (f == 1)//проверили что нашли 
            {
                //cres = String.Compare(Convert.ToString(arts[i].node), Convert.ToString(candid));//Сравнили
                while ((i <= arts.Length - 1) && (arts[i].node == candid))
                {
                    //greed++;зачем оно тут???
                    //cres = String.Compare(Convert.ToString(arts[i].node), Convert.ToString(candid));//Сравнили
                    dk = po.IndexOf(arts[i].leaf);//есть ли ПО
                    if (dk != -1)
                    { CanSvaz += 1;} 
                    i++; 
                } 
               //else { return CanSvaz; }
                //while ((cres == 0) && (i>=0))
                //{
                //    cres = String.Compare(Convert.ToString(arts[i].node), Convert.ToString(candid));//Сравнили 
                //    if ((cres == 0) && (dk != -1))//есть связь "кандидат-по"
                //    {
                    //  CanSvaz += 1;
                //    }
                //    i--;
                //} 
                //Array.Resize(ref arts, arts.Length + 1);//увеличили массив 
                //j = length - 1;
                //while (j >= i)//подвинули 
                //{
                //    arts[j + 1] = arts[j];
                //    j--;
                //}
                //arts[i] = new Category() { IdCat = root.IdCat, LinkCat = root.LinkCat, NameCat = root.NameCat };
                ////вставили в стек
                //ooo.Add(root);
                ////Name_links.Add(link_of_id);
            }

            //svazi.Clear();
            //for (int pi = 0; pi < arts.Length; pi++) //преобразовываем обратно в Category
            //{
            //    svazi.Add(arts[pi]);
            //}
            return CanSvaz;
        }


        public List<Category> Collect_connecting(Category ppp)

// 4) Пройти по списку статей

            // собирает слова в статье
        {
            bool gran = false; int vv = 0;
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
                                        slovo = search_waste_link(Storage_articles, slovo); // выбрасываем слова которых нет в области
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
                            if (words[k] == "Следующая страница")
                            {
                                link_of_id = new Category() { IdCat = u, LinkCat = "https://ru.wikipedia.org" + words[k - 3], NameCat = "" }; u++;//нашли категорию
                                Stek_links.Add(link_of_id);
                            }
                            if (words[k] == ("CategoryTreeChildren"))
                            {
                                link_of_id = new Category() { IdCat = u, LinkCat = "https://ru.wikipedia.org" + words[k - 13], NameCat = words[k - 12] }; u++;//нашли категорию
                                //search_untouched_link(Name_links, link_of_id, Stek_links);//поиск повторений
                                
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

            if (Stek_links[j].NameCat == "BLITS") goto Out;//Спутники столкнувшиеся на орбите или повреждённые космическим мусором    Межзвёздная среда‎‎

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

                string way = @"d:\test\PO\graf.txt";//@"C:\Users\CalcuFox\Desktop\Три\Graf.txt";
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

        private void button4_Click(object sender, EventArgs e)//алгоритм
        {
           
            var sw = new Stopwatch();
            sw.Start();
           // double [] porog = new double [] {0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9} ;
            //int[] Ver = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            
            List<int> Candidate = new List <int>(); // тут потенциальные кандидаты на ПО
            //List <int> Ver = new List <int>();
            //for (int vt=0; vt<Ver.Length ; vt++)
            //    {
            DataDomain.Add(Convert.ToInt32(/*Ver[vt]*/textBox3.Text)); // номер вершины
            //Candidate.Add(/*Ver[0]*/Convert.ToInt32(textBox3.Text));
            int ll = 0; List<int> chop = new List<int>();
            double piki = 0;
            int prohod = Convert.ToInt32(textBox5.Text);
            //double porog = Convert.ToDouble(porogN.Text);
                
                    //for (int sa=0; sa<porog.Length;sa++)
                    //{
                List <Connect> Can = new List <Connect>(); // список связей кандидатов
                List <Fracion> Vrem = new List<Fracion>();// временное хранилище для колличества связей каждого кандидата
                var swop = new Stopwatch();
                    double svaaaz = 0; //количество связей вершины с ПО
                    for (int z = 0; z < idCon.Count; z++) // пройти по списку вершин 
                    {
                        // if ((idCon[z].node == DataDomain[vi]) && (DataDomain.IndexOf(idCon[z].leaf) == -1/*Candidate.Find(x => x == idCon[z].leaf) == -1*/) && (Candidate.IndexOf(idCon[z].leaf) == -1))
                        //набрать кандидатов (всех, на кого ссылается по кроме по)
                        if ((idCon[z].node == DataDomain[0]) && (Candidate.IndexOf(idCon[z].leaf) == -1))
                        {
                            // набрать кандидатов (всех, на кого ссылается по)
                            //svaaaz++;
                            Candidate.Add(idCon[z].leaf);
                        }
                    }
                    const int g = 35411;
                    //List<int> fer = new List<int>();
                    int[] fer = Enumerable.Range(1, g).ToArray();
                   
                   // fe.CopyTo(fer.ToArray(), 0);
           // fer.ToList<int>();
                    Fracion fra = new Fracion { root = (DataDomain[0]), ratioN = 1.0/*Convert.ToDouble(search_untouched_connect(idCon, DataDomain[0], DataDomain))*/, ratioK = Convert.ToDouble(search_untouched_connect(idCon, DataDomain[0], fer.ToList<int>())), sv = search_untouched_connect(idCon, DataDomain[0], Candidate) };
                    Fract.Add(fra);
            
                    Candidate.Clear();
                    StreamWriter file61 = new StreamWriter(wat, true);
                    file61.WriteLine(" Вершина [" + Convert.ToInt32(/*Ver[vt]*/textBox3.Text) + "] порог:" + porogN.Text/*[sa]*/ + " & " + porogK.Text + "\n");
                    file61.Close();
                    StreamWriter file41 = new StreamWriter(way, true);
                    file41.WriteLine(" Вершина [" + Convert.ToInt32(/*Ver[vt]*/textBox3.Text) + "] порог:" + porogN.Text/*[sa]*/ + " & " + porogK.Text + "\n");
                    file41.Close();
                    StreamWriter file91 = new StreamWriter(wayy, true);
                    file91.WriteLine(" Вершина [" + Convert.ToInt32(/*Ver[vt]*/textBox3.Text) + "] порог:" + porogN.Text/*[sa]*/ + " & " + porogK.Text + "\n");
                    file91.Close();
                    for (int lip = 0; lip < prohod; lip++)
                    {
                        swop.Start();
                 //       string way = @"d:\test\Три\РостПО.txt";

                        StreamWriter file6 = new StreamWriter(wat, true);
                        file6.WriteLine(" --------------------------- " + Convert.ToString(lip + 1) + " ---------------------------\n");
                        file6.Close();
                        StreamWriter file4 = new StreamWriter(way, true);
                        file4.WriteLine(" --------------------------- " + Convert.ToString(lip + 1) + " ---------------------------\n");
                        file4.Close();
                        StreamWriter file9 = new StreamWriter(wayy, true);
                        file9.WriteLine(" --------------------------- " + Convert.ToString(lip + 1) + " ---------------------------\n");
                        for (int vi = 0; vi < DataDomain.Count; vi++)
                        {
                            for (int z = 0; z < idCon.Count; z++) // пройти по списку вершин 
                            {
                                //if ((idCon[z].node == DataDomain[vi]) && (DataDomain.IndexOf(idCon[z].leaf) == -1/*Candidate.Find(x => x == idCon[z].leaf) == -1*/) && (Candidate.IndexOf(idCon[z].leaf) == -1))
                                //набрать кандидатов (всех, на кого ссылается по кроме по)
                                if ((idCon[z].node == DataDomain[vi]) && (Candidate.IndexOf(idCon[z].leaf) == -1))  
                                    // набрать кандидатов (всех, на кого ссылается по)
                                {
                                    //svaaaz++;
                                    Candidate.Add(idCon[z].leaf);
                                    file9.WriteLine("(" + Convert.ToString(idCon[z].leaf) + ")\n");
                                }
                                //if (idCon[z].node == DataDomain[vi])//считаем связи ПО
                                //набрать кандидатов (всех, на кого ссылается по кроме по)
                                //if ((idCon[z].node == DataDomain[vi]) && (DataDomain.IndexOf(idCon[z].leaf) == -1) && (Candidate.IndexOf(idCon[z].leaf) == -1))
                                //{
                                //    sizeDD += 1;
                                //}
                                // удвоение массива связей
                                //if ((idCon[z].leaf == DataDomain[vi]) && (DataDomain.IndexOf(idCon[z].node) == -1) && (Candidate.IndexOf(idCon[z].node) == -1))
                                //{
                                //    //svaaaz++;
                                //    // набрать кандидатов (всех, кто ссылается на по кроме по)
                                //    Candidate.Add(idCon[z].node);
                                //}
                            }
                        }
                        //Candidate.Add(Convert.ToInt32(/*Ver[vt]*/textBox3.Text));
                        file9.Close();
                        //Candidate = Candidate.Where(x => x != 0).ToArray();
                        // НА ДОБАВЛЕНИЕ
                        //поиск всех интересующих наc связей кандидатов
                        for (int yui = 0; yui < Candidate.Count; yui++)
                        {
                            if (DataDomain.IndexOf(Candidate[yui]) != -1) //если есть такой
                            {
                                //DataDomain.Add(Candidate[yui]);
                                DataDomain.Remove(Candidate[yui]); 
                            }
                            DataDomain.Add(Candidate[yui]);
                            greed = 0.0;
                            foreach (int too in DataDomain)
                            {
                                greed += Convert.ToDouble(search_untouched_connect(idCon, too, DataDomain)/2.0);
                            }
                            //come here Candidate[yui]
                            //greed = 0;
                            //svaaaz = 0; 

                            svaaaz = search_untouched_connect(idCon, Candidate[yui], DataDomain); //количество связей кандидата с ПО
                            //количство связей кандидата
                            //for (int ryr = 0; ryr < DataDomain.Count; ryr++)
                            //{
                            //    for (int rr = 0; rr < idCon.Count; rr++)
                            //    {
                            //        if ((DataDomain[ryr] == idCon[rr].node) && ((DataDomain.IndexOf(idCon[rr].leaf)) != -1))/* || (DataDomain[ryr] == idCon[rr].leaf))*/
                            //        {
                            //            greed++;//все связи кандидата
                            //        }
                            //    }
                            //}
                            //    
                            //        if (((Candidate[yui] == idCon[rr].node) && (DataDomain[ryr] == idCon[rr].leaf)) /*|| ((Candidate[yui] == idCon[rr].leaf) && (DataDomain[ryr] == idCon[rr].node))*/) // связи кандидатов
                            //        {//двойной массив
                            //            //fra = new Fracion { root = Candidate[yui].root, sv = sv+1 }; связь типа кандидат - по или по - кандидат
                            //            svaaaz++;
                            //        }

                            // n - количество связей в по, k - количество связей вершины
                            fra = new Fracion { root = Candidate[yui], ratioN = (greed), ratioK = (Convert.ToDouble(search_untouched_connect(idCon, Candidate[yui], fer.ToList<int>()))), sv = svaaaz };//двойной массив
                            
                            //svaaaz = 0; 
                            //greed = 0;
                            //if ((fra.ratioK > porog/*[sa]*/)/* && (DataDomain.IndexOf(fra.root) == -1)*/)
                            if ((((svaaaz / fra.ratioN) >= Convert.ToDouble(porogN.Text)) || ((Double.IsNaN(svaaaz / fra.ratioN)) == true)) && ((svaaaz / fra.ratioK) >= Convert.ToDouble(porogK.Text)))
                            {
                                //if (/*DataDomain.IndexOf(fra.root)*/Fract.IndexOf() != -1) //заменяем если вершина уже есть >> не будет ее
                                     
                                //{
                                   // Fract.RemoveAll(Fract[DataDomain.IndexOf(fra.root)]);
                             
                                 Fract.RemoveAll(x => x.root == Candidate[yui]);
                                //   // DataDomain.Remove(fra.root); можно не заменять
                                   //Fract.Add(fra);
                                //   // DataDomain.Add(fra.root); //chop++;
                                //    //пересчитать кол связей по мб не надо, все равно переситываем при заходе
                                //    //greed = 0.0;
                                //    //foreach (int too in DataDomain)
                                //    //{
                                //    //    greed += Convert.ToDouble(search_untouched_connect(idCon, too, DataDomain));
                                //    //}
                                //}
                                //if (DataDomain.IndexOf(fra.root) == -1) //добавляет если не нашел такой вершины
                                //{
                                    Fract.Add(fra);
                                   // DataDomain.Add(fra.root);
                                    //пересчитать кол связей по
                                    //greed = 0.0;
                                    //foreach (int too in DataDomain)
                                    //{
                                    //    greed += Convert.ToDouble(search_untouched_connect(idCon, too, DataDomain));
                                    //}
                               // }
                            }
                            else 
                            { 
                                DataDomain.Remove(Candidate[yui]);
                            }
                        }
                        //   DataDomain.Clear();
                        //for (int yh = 0; yh < Fract.Count; yh++)
                        //{
                        //    if (DataDomain.IndexOf(Fract[yh].root) == -1)
                        //    {
                        //        DataDomain.Add(Fract[yh].root);
                        //    }
                        //    //else
                        //    //{
                        //    //    DataDomain.Remove(Fract[yh].root);
                        //    //    DataDomain.Add(Fract[yh].root);
                        //    //}
                        //}



                        //string way = @"d:\test\Rezy.txt";
                        //StreamWriter file4 = new StreamWriter(way, true);
                        //////    запись в файл
                        ////for (int ol = 0; ol < Fract.Count; ol++)
                        ////{
                        ////    richTextBox1.Text +="[" + Fract[ol].root + "] - " + Fract[ol].ratio + "\n";
                        ////}
                        ////    richTextBox1.Text += "Предметная область: ";
                        //for (int ol = 0; ol < Fract.Count; ol++)
                        //    //for (int ole = 0; ole < DataDomain.Count; ole++)
                        //{
                        //    file4.WriteLine(" [" + Convert.ToString(Fract[ol].root) + "] - " + Convert.ToString(Fract[ol].ratio) + "\n"); 
                        //  //  richTextBox1.Text += " [" + DataDomain[ole] + "] ";
                        //}
                        //file4.WriteLine("Конец этапа" + lip + "\n");
                        //file4.Close();
                        List<int> Del = new List<int>();
                        Candidate.Clear();
                        //  Fract.Clear();
                        //НА ВЫКИДЫВАНИЕ
                        for (int ryr = 0; ryr < DataDomain.Count; ryr++)
                        {
                             greed = 0.0;
                                foreach (int too in DataDomain)
                                {
                                    greed += Convert.ToDouble(search_untouched_connect(idCon, too, DataDomain)/2.0);
                                }
                            //svaaaz = 0; greed = 0;
                            svaaaz = search_untouched_connect(idCon, DataDomain[ryr], DataDomain);

                            //for (int ror = 0; ror < DataDomain.Count; ror++)
                            //{
                            //    for (int rr = 0; rr < idCon.Count; rr++)
                            //    {
                            //        if ((DataDomain[ror] == idCon[rr].node) && (DataDomain.IndexOf(idCon[rr].leaf) != -1))
                            //        {
                            //            greed++;
                            //        }
                            //        //if ((DataDomain[ryr] == idCon[rr].leaf) /*|| (DataDomain[ryr] == idCon[rr].node*/)// связи  в по 
                            //        //    //двойной массив
                            //        //{
                            //        //    //fra = new Fracion { root = Candidate[yui].root, sv = sv+1 };
                            //        //    svaaaz++;// связи типа По - по
                            //        //}
                            //    }
                            //}
                            //if ((((svaaaz / fra.ratioN) >= Convert.ToDouble(porogN.Text)) || ((Double.IsNaN(svaaaz / fra.ratioN)) == true)) && ((svaaaz / fra.ratioK) >= Convert.ToDouble(porogK.Text)))
                            //svaaaz = 0; 
                            //Fract.Add(fra);
                            //greed = 0;
                            //if (fra.ratio < porog/*[sa]*/)
                            //if (/*(Fract.IndexOf(Fract[DataDomain.IndexOf(fra.root)]) > 0  )&&*/  ((svaaaz /fra.ratioN) <= Convert.ToDouble(porogN.Text)) && ((svaaaz /fra.ratioK) <= Convert.ToDouble(porogK.Text)))
                            if (((svaaaz / fra.ratioN) < Convert.ToDouble(porogN.Text)) && ((svaaaz / fra.ratioK) < Convert.ToDouble(porogK.Text)))
                            {
                                chop.Add(fra.root);
                               // Fract.Remove(Fract[DataDomain.IndexOf(fra.root)]);
                                Fract.Remove(fra);
                                DataDomain.Remove(fra.root);
                                Del.Add(fra.root);
                                //file61.WriteLine("{" + Convert.ToString(fra.root) + "};\n");
                                //тут надо пересчитывать greed
                                //пересчитать кол связей по не надо
                                
                               
                            }
                        }
                        StreamWriter file611 = new StreamWriter(wat, true);

                        for (int s = 0; s < Del.Count; s++)
                        {
                            file611.WriteLine("{" + Convert.ToString(Del[s]) + "};\n");
                        }
                        file611.Close();
                      


                        //DataDomain.Clear();
                        //for (int ww = 0; ww < Fract.Count; ww++)
                        //{
                        //    DataDomain.Add(Fract[ww].root);
                        //}
                        //// Fract.Clear();
                        //richTextBox1.Text += " Вычитание \n";

                        //for (int ole = 0; ole < DataDomain.Count; ole++)
                        //{
                        //    richTextBox1.Text += " [" + DataDomain[ole] + "] ";
                        //}
                        //richTextBox1.Text += "Конец " + (lip + 1) + " этапа \n";


                        //string way = @"d:\test\Rezy.txt";
                        //StreamWriter file4 = new StreamWriter(way, true);
                        //////    запись в файл
                        ////for (int ol = 0; ol < Fract.Count; ol++)
                        //file4.WriteLine("--- Этап №" + (lip+1) + " из " + prohod + " порог: " + porog+ " ---\n");
                        //for (int ole = 0; ole < DataDomain.Count; ole++)
                        //{
                        //    file4.WriteLine(" [" + Convert.ToString(Fract[ole].root) + "] - " + Convert.ToString(Fract[ole].ratio) + "\n");
                        //}
                        ////file4.WriteLine("Конец этапа" + lip + "\n");
                        //file4.Close();
          // string wap = @"d:\test\Три\ТестРостПО.txt";
                        StreamWriter file5 = new StreamWriter(wap, true);
                        for (int ole = 0; ole < Fract.Count; ole++)
                        {
                            file5.WriteLine(" [" + Convert.ToString(Fract[ole].root) + "] - " + Convert.ToString(Fract[ole].sv / Fract[ole].ratioN) + " || " + Convert.ToString(Fract[ole].sv/ Fract[ole].ratioK)  /*+" || " + Convert.ToString(Fract[ole].sv)*/ + "\n");
                        }
                        //file5.WriteLine("Удалено: ");
                        //for (int kk = 0; kk < chop.Count; kk++)
                        //{
                        //    file5.WriteLine(Convert.ToString((chop[kk]) + " "));
                        //}
                        file5.Close();
                        //sizeDD = 0;

               // }
                //SoundPlayer sp = new SoundPlayer(@"d:\test\End.wav");
                //sp.Play();
                    //}
                    //}
                        swop.Stop();
                        richTextBox1.Text += "Время этапа (" + (lip + 1) + "): " + swop.Elapsed.ToString() + "\n";
                        swop.Reset();
                    };
                    sw.Stop();
                    richTextBox1.Text += " Время работы алгоритма: " + sw.Elapsed.ToString() +"\n Размер ПО: " + Convert.ToInt32(DataDomain.Count) + "\n";

                    DataDomain.Clear();
                    Fract.Clear();
                    Vrem.Clear();
                    Can.Clear();
                    Candidate.Clear();
               
        }

        private void button5_Click(object sender, EventArgs e) //из графа в массив
        {
            
            //for (int i = 0; i < 100; i++)
            //{
            //    progressBar1.Value = i;
            //    System.Threading.Thread.Sleep(100);
            //}
            var sw1 = new Stopwatch();
            sw1.Start();
            //string sr = System.IO.File.ReadAllText(@"c:\test\RafTestTest.txt").Replace("\n", " ");
            //      StreamReader file5 = new StreamReader(@"d:\test\RafRaf.txt"); //Открываем файл для чтения
            System.IO.FileInfo il = new FileInfo(wer);
//          int ili = Convert.ToInt32(il.Length);
//          progressBar1.Maximum = ili; int to=0;
            StreamReader file5 = new StreamReader(wer); //Открываем файл для чтения
            string sr = ""; //Объявляем переменную, в которую будем записывать текст из файла
            while (!file5.EndOfStream) //Цикл длиться пока не будет достигнут конец файла
            {
//                to += 1;
                sr += file5.ReadLine() + ' '; //В переменную str по строчно записываем содержимое файла
//                progressBar1.Value = to;
//                System.Threading.Thread.Sleep(ili);
            }
            richTextBox1.Text += "Почти готово\n";            
            string[] wor = sr.Split(new[] { ':', ';' });// Разбиваем строку на массив строк
           
            // StreamWriter filef = new StreamWriter(wa);
            //StreamWriter fil = new StreamWriter(@"d:\test\Raf.txt");
            //for (int iop = 0; iop < wor.Length; iop=iop+2)
            //{
            //    fil.WriteLine(wor[iop] + ":" + wor[iop+1] + ";\n");
            //}
//            progressBar1.Maximum = ds;
            for (int t = 0; t < wor.Length - 1; t += 2)// связь УМНОЖАТЬ НА 4
            {
                con = new Connect { node = Convert.ToInt32(wor[t]), nodeName = "", leaf = Convert.ToInt32(wor[t + 1]), leafName = "" };
                idCon.Add(con);
//                progressBar1.Value = t;
//                System.Threading.Thread.Sleep(ds);
            }
//            progressBar1.Value = ds;
//            System.Threading.Thread.Sleep(ds);
            //sr.Remove();
            richTextBox1.Text += "Граф запихали\n";
           // file.Close();
           
            //SoundPlayer sp = new SoundPlayer(@"d:\test\End.wav");
            //sp.Play();
            file5.Close();
            richTextBox1.Text += " Время выполнения: " + sw1.Elapsed.ToString() + "\n";
            sw1.Stop();
            var sw2 = new Stopwatch();
            sw2.Start();
            List<Connect> Newidcon = new List<Connect>();
            Connect gp = new Connect();
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream ggg = new FileStream(serway, FileMode.OpenOrCreate))
            {
                //for (int i = 0; i < idCon.Count; i++)
                //{
                    //gp.vhod = idCon[i].node;
                    //gp.vyhod = idCon[i].leaf;
                    
                    //Newidcon.Add(gp);
                    
                //}
                formatter.Serialize(ggg, idCon);

            }
            sw2.Stop();
            richTextBox1.Text += " Время выполнения: " + sw2.Elapsed.ToString() + "\n";
            
        }

        private void button2_Click_1(object sender, EventArgs e)//преобразование  в  слова
        {
            StreamReader file5 = new StreamReader(swey); //Открываем файл для чтения
     //  StreamReader file5 = new StreamReader(@"C:\Users\CalcuFox\Desktop\Три\РостПО.txt"); //Открываем файл для чтения
            
            string sr = ""; //Объявляем переменную, в которую будем записывать текст из файла
            while (!file5.EndOfStream) //Цикл длиться пока не будет достигнут конец файла
            {
                sr += file5.ReadLine() + ' '; //В переменную sr построчно записываем содержимое файла
            }

            string[] wor = sr.Split(new[] { '[', ']' });// для вершин
       // StreamReader file57 = new StreamReader(@"C:\Users\CalcuFox\Desktop\Slova.txt"); //Открываем файл для чтения
            StreamReader file57 = new StreamReader(slovaey); //Открываем файл для чтения
            string sr1 = ""; //Объявляем переменную, в которую будем записывать текст из файла
            while (!file57.EndOfStream) //Цикл длиться пока не будет достигнут конец файла
            {
                sr1 += file57.ReadLine() + ' '; //В переменную str по строчно записываем содержимое файла
            }

            string[] wol = sr1.Split(new[] { '%', ';' });// для слов
            string u = "Вершина";

      //StreamWriter fil = new StreamWriter(@"C:\Users\CalcuFox\Desktop\Три\РостПОСлова.txt");
            StreamWriter fil = new StreamWriter(otvey);
      for (int t = 1; t < wor.Length/*Convert.ToInt32(kostyli.Text) *2*/; t += 2)// МЕНЯТЬ
            {
                //lim[gp] = Convert.ToInt16(wor[t]);
                for (int sl = 0; sl < wol.Length; sl+=2)
                {
                    if (Convert.ToInt32(wor[t]) == Convert.ToInt32(wol[sl]))
                    {
                        //DataDomain.Add(Convert.ToInt32(wor[t]));
                        fil.WriteLine("[" + wol[sl + 1] + "]" + wor[t+1] + ";\n"); break;
                    }
                    if (wor[t].Contains(u) == true)
                    {
                        fil.WriteLine("\n" + "\n");/* wor[t].Intersect(u);*/
                    }
                }
            }
            fil.Close();
            //StreamWriter fil = new StreamWriter(@"d:\test\Raf.txt");
            //for (int iop = 0; iop < wor.Length; iop=iop+2)
            //{
            //    fil.WriteLine(wor[iop] + ":" + wor[iop+1] + ";\n");
            //}
            //
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Form2 Form2 = new Form2();
            Form2.Show();
            

        }

        private void serverRB_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
             way = @"C:\Users\CalcuFox\Desktop\Три\РостПО.txt";
             wayy = @"C:\Users\CalcuFox\Desktop\Три\КандидатыПО.txt";
             wat = @"C:\Users\CalcuFox\Desktop\Три\Выкинули.txt";
             wap = @"C:\Users\CalcuFox\Desktop\Три\РостПО.txt";
             wer = @"C:\Users\CalcuFox\Desktop\RafRaf.txt";
             swey = @"C:\Users\CalcuFox\Desktop\Три\РостПО.txt";
             slovaey = @"C:\Users\CalcuFox\Desktop\Slova.txt";
             otvey = @"C:\Users\CalcuFox\Desktop\Три\РостПОСлова.txt";
             serway = @"C:\Users\CalcuFox\Desktop\Три\Ser.txt";
             //ds = Convert.ToInt32(textBox2.Text) * 2;
        }

        private void kompRB_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            way = @"d:\test\Три\РостПО.txt";
            wayy = @"d:\test\Три\КандидатыПО.txt";
            wat = @"d:\test\Три\Выкинули.txt";
            wap = @"d:\test\Три\РостПО.txt";
            wer = @"d:\test\Три\RafRaf.txt";
            swey = @"d:\test\Три\РостПО.txt";
            slovaey = @"d:\test\mini\Slova.txt";
            otvey = @"d:\test\Три\РостПОСлова.txt";
            serway = @"d:\test\Три\Ser.txt";
            //ds = Convert.ToInt32(textBox2.Text) * 2;
        }

        private void testRB_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            way = @"d:\test\Тест\РостПО.txt";
            wayy = @"d:\test\Тест\КандидатыПО.txt";
            wat = @"d:\test\Тест\Выкинули.txt";
            wap = @"d:\test\Тест\РостПО.txt";
            wer = @"d:\test\Тест\Raftest.txt";
            swey = @"d:\test\Тест\РостПО.txt";
            slovaey = @"d:\test\mini\Slova.txt";
            otvey = @"d:\test\Тест\РостПОСлова.txt";
            serway = @"d:\test\Тест\Ser.txt";
            //ds = Convert.ToInt32(textBox1.Text) * 2;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var sw3 = new Stopwatch();
            sw3.Start();
            idCon.Clear();
            List<Connect> Newidconew = new List<Connect>();
            using (FileStream ggg = new FileStream(serway, FileMode.OpenOrCreate))
            {
                List<Connect> Newidcone = (List<Connect>)formatter.Deserialize(ggg);// list<graf>
                Connect cont = new Connect ();
                foreach (Connect bn in Newidcone)
                {
                    cont.node = bn.node; cont.leaf = bn.leaf;
                    idCon.Add(cont);
                    
                    //richTextBox1.Text += bn.node + "|" + bn.leaf + "\n";
                }
            }
            sw3.Stop();
            richTextBox1.Text += "Время выполнения десериализации: " + sw3.Elapsed.ToString() + "\n";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            {
            int over_link = 0;
            int stop = 0;
            //int j = 0;

            // 1) Пройти по корневой ссылке

            link_of_name = new Category() { IdCat = 0, LinkCat = Convert.ToString(textBox1.Text), NameCat = Convert.ToString(textBox1.Text) };
            Stek_links.Add(link_of_name);
            Name_links.Add(link_of_name);
                for (int ws = 0; ws < Stek_links.Count; ws++)
                {
                    Parser(Stek_links[ws].LinkCat, false);//статьи
                    Parser(Stek_links[ws].LinkCat, true);//категории
                
        //NextAddress: stop++; over_link++;
        //    //if (over_link > 10) //стопарик slog.Count
        //    //{
        //    //    goto Out;
        //    //}
        //    //if (j - 1 == 10)// собрать только 3 штуки
        //    //{
        //    //    goto Out;
        //    //}
        //    // if (j == Stek_links.Count) goto Out;

        //    if (Stek_links[j].NameCat == "BLITS") goto Out;//Спутники столкнувшиеся на орбите или повреждённые космическим мусором    Межзвёздная среда‎‎

        //    // 2) Собрать категории в два массива

        //    Parser(Stek_links[j].LinkCat, false);//статьи
        //    Parser(Stek_links[j].LinkCat, true);//категории
        //    j++;

        //    goto NextAddress;
        //Out:
        //    //for (int r = 0; r < idCon.Count; r++)
        //    //{
        //    //    richTextBox1.Text += Convert.ToString(idCon[r].leafName) + " - " + Convert.ToString(idCon[r].nodeName) + "\n";
        //    //}
        //    //for (int r = 0; r < Stek_links.Count; r++)
        //    //{
        //    //    richTextBox1.Text += Convert.ToString(Stek_links[r].NameCat) + "\n";
        //    //}
        //    int uiu = 0;


                    string way22 = @"C:\Users\CalcuFox\Desktop\HJ\ЗвездыПО.txt";
                StreamWriter filer = new StreamWriter(way22);
                //    запись в файл
                for (int r = 0; r < Stek_articles.Count; r++)//Stek_articles.Count
                {
                    filer.WriteLine(Convert.ToString(Stek_articles[r].NameCat) + ";\n"); //ut++; uiu++;// берет слова из каждой статьи
                }
                filer.Close();
            }
/*
                //               for (int p = 0; p < Slova.Count; p++)
                for (int p = 0; p < idCon.Count; p++)
                {
                    //                       file.WriteLine(Convert.ToString(Slova[p].IdCat) + ") " + Convert.ToString(Slova[p].NameCat)+ "\n");
                    file.WriteLine(Convert.ToString(idCon[p].node) + ":" + Convert.ToString(idCon[p].leaf) + ";\n");
                    //                        file.WriteLine(Convert.ToString(idCon[p].node) + ") " + Convert.ToString(idCon[p].nodeName) + " ------ " + Convert.ToString(idCon[p].leaf) + ")" + Convert.ToString(idCon[p].leafName) + "\n");
                }
                file.Close();*/

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
