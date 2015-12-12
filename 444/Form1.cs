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

namespace _444
{
    public partial class Form1 : Form
    {
        public List<Category> Stek_links = new List<Category>();//по idшнику
        public Category link_of_id = new Category();
        public List<Category> Name_links = new List<Category>();//по имени
        public Category link_of_name = new Category();
        public List<Category> Storage_articles = new List<Category>();// статьи
        public List<Category> Slova = new List<Category>();// ссылки в статье
        public Category slovo = new Category();
        public List<Connect> idCon = new List<Connect>();// связи между статьями
        public Connect con = new Connect();
        public int r = 1, u = 1, e = 1, ut = 0, mesto = 0, ove = 0;
        public bool Rip = false;
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
            public int leaf; //лист
            public string leafName;
            public int node;// узел
            public string nodeName;
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
                richTextBox1.Text += Convert.ToString(Storage_articles[r - 2].NameCat) + /*"\n" + Storage_articles[r - 23].NameCat + */ "\n" + Convert.ToString(Stek_links[Stek_links.Count - 2].NameCat);
                return "";
            }
            //catch (UriFormatException)
            //{
            //    return "";
            //}
        }

        public List<Category> search_untouched_link(List<Category> ddd, Category root)//List<object>
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
                arts[i] = new Category() { IdCat = root.IdCat, LinkCat = root.LinkCat, NameCat = root.NameCat }; //вставили 
                Stek_links.Add(root);
                //Name_links.Add(link_of_id);
            }
            ddd.Clear();
            for (int pi = 0; pi < arts.Length; pi++) //преобразовываем обратно в Category
            {
                ddd.Add(arts[pi]);
            }
            return ddd;
        }

        public Connect search_waste_link(List<Category> yyy, Connect wood)//List<object>
        {
            Category[] arts = yyy.ToArray(); //массив из категорий
            int w = 0;
            //foreach (Category ioi in yyy) //из списка в массив
            //{
            //    arts[w] = ioi; w++;
            //}
            int i, j, k = 0, f, cres;
            int length = arts.Length;//Размер 
            i = 0;//Начало 
            j = length - 1;//Конец 
            f = 0;//Флаг обнаружения в массиве 
            while (i <= j)//Поиск до схождения в точку, равенство чтобы гарантировать хоть 1 сравнение 
            {
                k = (i + j) / 2;//Середина 
                cres = String.Compare(arts[k].NameCat, wood.leafName);//Сравнили 
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
            if (f == 1)//проверили что не нашли - вставляем    --- НАШЛИ, НАДО ВЫКИНУТЬ
            {
                wood.leaf = arts[k].IdCat;
                idCon.Add(wood);
            }
            return wood;
        }


        public List<Category> Collect_connecting(Category ppp) // собирает слова в статье
        {
            bool gran = false;
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
                                //

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
                                        slovo = new Category() { IdCat = e, NameCat = words[k + 1], LinkCat = "https://ru.wikipedia.org" + words[k - 1] }; e++;
                                        Slova.Add(slovo);
                                        con = new Connect { node = ppp.IdCat, nodeName = Storage_articles[ut].NameCat, leaf = 0, leafName = slovo.NameCat };
                                        // поиска в Storage_articles
                                        con.leaf = search_waste_link(Storage_articles, con).leaf;


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
            string way = @"d:\test\TestFile.txt";// записывает массив строк в новый файл!!!
            string wikiText = get_http(url);
            List<string> words = (wikiText.Split(new[] { '>', '<', '"' }, StringSplitOptions.RemoveEmptyEntries).ToList());// Разбиваем строку на массив строк
            for (int i = 0; i < words.Count; i++)
            {
                if (check == true)//для категорий
                {
                    if (words[i] == "mw-subcategories")
                    {
                        for (int k = i; k < words.Count; k++)
                        {
                            if (words[k] == ("CategoryTreeChildren"))//CategoryTreeSection 23 24
                            {
                                link_of_id = new Category() { IdCat = u, LinkCat = "https://ru.wikipedia.org" + words[k - 13], NameCat = words[k - 12] }; u++;
                                search_untouched_link(Name_links, link_of_id);//поиск повторений
                            }
                        }
                        goto NextAddress;//кончилась страница
                    }
                }
                if (check == false)//для статей
                {
                    if (words[i] == ("mw-pages"))
                    {
                        for (int k = i; k < words.Count; k++)
                        {
                            if (words[k] == ("mw-normal-catlinks"))
                            {
                                goto NextAddress;//кончилась страница
                            }
                            if (words[k] == ("li"))//" title="+1
                            {
                                stiker = new Category() { IdCat = r, LinkCat = "https://ru.wikipedia.org" + words[k + 2], NameCat = words[k + 4] }; r++;
                                Storage_articles.Add(stiker);//Stek_links.Add(stiker);
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
            int over_link = 0;
            int stop = 0;
            int j = 0;
            link_of_name = new Category() { IdCat = 0, LinkCat = "https://ru.wikipedia.org/wiki/Категория:Космос", NameCat = "Космос" }; Stek_links.Add(link_of_name); Name_links.Add(link_of_name);
        NextAddress: stop++; over_link++;
            if (over_link > 20) //стопарик slog.Count
            {
                goto Out;
            }
            if (j - 1 == Stek_links.Count)
            {
                goto Out;
            }
            //Собирает:
            Parser(Stek_links[j].LinkCat, false);//статьи
            Parser(Stek_links[j].LinkCat, true);//категории
            foreach (Category evl in Storage_articles)
            {
                richTextBox2.Text += Convert.ToString(evl.NameCat) + " ------ " + Convert.ToString(evl.LinkCat) + "\n";
            }
            j++;
            goto NextAddress;
        Out:
            int uiu = 0;
            foreach (Category lili in Storage_articles)
            {
                Collect_connecting(lili); ut++; uiu++;// берет слова из каждой статьи
                if (uiu == 100)
                {
                    string way = @"d:\test\TestFile.txt";
                    StreamWriter file = new StreamWriter(way);
                    //    запись в файл

                    for (int r = 0; r < idCon.Count; r++)
                    {
                        file.WriteLine(Convert.ToString(idCon[r].node) + " ------ " + Convert.ToString(idCon[r].leaf) + "\n");
                    }
                    file.Close();
                    uiu = 0;
                }
            }
        }
    }
}