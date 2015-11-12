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

namespace _444
{
    public partial class Form1 : Form
    {
        public struct Category
        {
            public int IdCat;// сколько раз ссылка повторялась
            public string NameCat;
            public string LinkCat;        
        }

        //public class Article
        //{
        //    public int IdArt { get; set; }
        //    public string NameArt { get; set; }
        //}

        public Form1()
        {
            InitializeComponent();
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
            catch (UriFormatException)
            {
                return "";
            }
        }

        public List<Category> search_untouched_link (List<Category> ddd, Category root)//List<object>
        {
            Category[] arts = ddd.ToArray();
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
                            j = k-1; 
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
                    j = length-1; 
                    while (j >= i)//подвинули 
                    { 
                        arts[j + 1] = arts[j];
                        j--; 
                    }
                    arts[i] =root;//вставили 
                }
                for (int pi = 0; pi < arts.Length; pi++) //преобразовываем обратно в Category
                {
                    ddd.Add(arts[pi]);
                }
            return ddd;
        } 
  
        public static Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

        private string[] /*List<Category>*/ Parser(string url, bool check)
        {
            List<Category> Links_by_id = new List<Category>();//по idшнику
            Category link_of_id = new Category();
            List<Category> Links_by_name = new List<Category>();//по имени
            //Category link_of_name = new Category();
            List<Category> Storage_articles = new List<Category>();
            Category stiker = new Category();
            string[] sss = new string[50];
            //string way = @"d:\test\TestFile.txt";// записывает массив строк в новый файл!!!
            //StreamWriter file = new StreamWriter(way);
            //link_of_name = new Category() { IdCat = 0, LinkCat = "https://ru.wikipedia.org/wiki/Категория:Космос", NameCat = "Космос" };// Storage_links.Add(link_of_name);
            //link_of_id = new Category() { IdCat = 0, LinkCat = "https://ru.wikipedia.org/wiki/Категория:Космос", NameCat = "Космос" };
            //
            int j = 0, over_link = 0, r = 0;
            
            string wikiText = get_http(url);
            List<string> words = (wikiText.Split(new[] { '>', '<', '"' }, StringSplitOptions.RemoveEmptyEntries).ToList());// Разбиваем строку на массив строк
            
            
                for (int i = 0; i < words.Count; i++)
                {
                    if(check == true)
            {
                    if (words[i] == "mw-subcategories")
                    {
                        for (int k = i; k < words.Count; k++)
                        {
                            if (words[k] == ("CategoryTreeSection"))
                            {
                                //j++;//колличество ссылок
                                link_of_id = new Category() { IdCat = r, LinkCat = "https://ru.wikipedia.org" + words[k + 23], NameCat = words[k + 24] };
                                sss[j] = link_of_id.NameCat; sss[j + 1] = link_of_id.LinkCat; j = j + 2;
                                search_untouched_link(Links_by_name, link_of_id);
                                over_link++;
                                  
                                //richTextBox1.Text += link_of_id.NameCat + "\n";
                            }
                        }goto NextAddress;//кончилась страница
                    }
                }
                if(check == false)
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
                                    stiker = new Category() { IdCat = r, LinkCat = "https://ru.wikipedia.org" + words[k + 2], NameCat = words[k + 4] };
                                    sss[j] = stiker.NameCat; sss[j + 1] = stiker.LinkCat; j = j + 2;
                                    // file.WriteLine(stiker.NameCat + "\n");
                                    // richTextBox2.Text += stiker.NameCat + "\n";
                                }
                            }
                        
                    }
                }
            }
        NextAddress: //file.Close(); 
            return sss;// Storage_links;
        }

        private void Form1_Load(object sender, EventArgs e) 
        {
        //int over_link = 0;
        List<Category> Storage_links = new List<Category>();//по idшнику
        //Category link_of_id = new Category();
        List<Category> Regulated_links = new List<Category>();//по имени
        Category link_of_name = new Category();
        List<Category> Storage_articles = new List<Category>();
       // Category stiker = new Category();
            string way = @"d:\test\TestFile.txt";// записывает массив строк в новый файл!!!
            StreamWriter file = new StreamWriter(way);
            link_of_name = new Category() { IdCat = 0, LinkCat = "https://ru.wikipedia.org/wiki/Категория:Космос", NameCat = "Космос" }; Storage_links.Add(link_of_name);
            //link_of_id = new Category() { IdCat = 0, LinkCat = "https://ru.wikipedia.org/wiki/Категория:Космос", NameCat = "Космос" };
            int stop = 0;
            //int j = 0;
       // NextAddress:
               for(int ide = 0; ide < Storage_links.Count; ide++)//массив со ссылками чет подазрительна
               {
                //j = 0;
               // stop++;
                if (stop >10000) //стопарик slog.Count
                {
                    goto Out;
                }
                string[] mas_articl = Parser(Storage_links[ide].LinkCat, true); 
                string[] mas_category =  Parser(Storage_links[ide].LinkCat, false);
                foreach (string el in mas_articl)
                {
                    richTextBox1.Text = Convert.ToString(el);
                }
                foreach (string el in mas_category)
                {
                    richTextBox2.Text = Convert.ToString(el);
                }
                }
           Out: stop++;
          //Close();
        }
    }
}
