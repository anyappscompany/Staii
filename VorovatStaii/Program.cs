using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Threading;
using System.Web;

namespace VorovatStaii
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() < 0)
            {
                Console.WriteLine("Введены не все параметры: 1 - код diffbot, 2 - рубрика");
                //Console.ReadKey();
                //return;
            }
            //Console.WriteLine(args[0]); 
            //Console.WriteLine(args[1]);
            
            string PathKwlist = "kwlist.txt";
            StreamReader kwfile = new StreamReader(@PathKwlist);
            string kwline;
            while ((kwline = kwfile.ReadLine()) != null)
            {
                //парсинг гугла на ссылки
                string str = GetPage(@"http://www.google.ru/search?site=webhp&hl=ru&q=" + HttpUtility.UrlEncode(kwline) + @"&start=0&num=10");
                string pattern3 = @"<h3 class=""r""><a href=""/url\?q=(?<urls>.*?)&amp;sa=U&amp";
                MatchCollection matchlist3; 
                Regex exp3 = new Regex(pattern3, RegexOptions.IgnoreCase);
                matchlist3 = exp3.Matches(str);
                if (matchlist3.Count > 0)
                {
                    Console.WriteLine("11111111111111");
                    foreach (Match match3 in matchlist3)
                    {
                        //urllist.Add(match3.Groups["urls"].Value.Replace("http%3a%2f%2f", "http://").Replace("%2f", "/"));
                        try
                        {
                            //MessageBox.Show(match3.Groups["urls"].Value.ToString());
                            Console.WriteLine("------" + kwline + "--" + match3.Groups["urls"].Value.ToString());
                            //urllist.Add(Uri.UnescapeDataString(match3.Groups["urls"].Value.ToString())); //MessageBox.Show(HttpUtility.UrlDecode(match3.Groups["urls"].Value));
                            string kk = gettextdiffbot(Uri.UnescapeDataString(match3.Groups["urls"].Value.ToString()), kwline, "1d4763bd7b4bab581f700270ec046701", "lineage");
                        }
                        catch
                        {

                        }
                    }
                }
            }
            
            Console.ReadKey();
        }

        static string gettextdiffbot(string url, string kw, string keydiff, string rubr)
        {        
            lab1:
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://api.diffbot.com/v3/article?token=" + keydiff + "&paging=false&timeout=10000&url=" + url + "&fields=text");
                //87f62bc00bb01c80471b42dd4967a29f
                request.UserAgent = "Mozilla/5.0";
                request.AllowAutoRedirect = true;
                request.Referer = "http://www.webcrawler.com/";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string str = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                var jss = new JavaScriptSerializer();
                var dict = jss.Deserialize<Dictionary<string, dynamic>>(str);
                Console.WriteLine("+");

                                
                //Console.WriteLine(dict["objects"][0]["title"]);
                //Console.WriteLine(dict["objects"][0]["html"]);
                string text = dict["objects"][0]["text"];
                if(text.Length > 1500)
                {
                    string alltext = "";
                    string title = dict["objects"][0]["title"];
                    string html = dict["objects"][0]["html"];
                    //alltext += "<h1>" + rubr + "</h1>\r\n";
                    alltext += "<h6>" + UppercaseFirst(kw) + ": " + title + "</h6>\r\n" + "<h5>" + rubr + "</h5>\r\n" + html;
                    string curdir = System.IO.Directory.GetCurrentDirectory(); 
                    // УБРАТЬ ССЫЛКИ
                    alltext = Regex.Replace(alltext, "<a(.+?)>(.+?)</a>", "$2");
                    // ВСТАВИТЬ КЛЮЧИ
                    string newtext = addkw(alltext, kw);
                    File.WriteAllText(curdir + "\\pages\\" + translit(kw) + "-"+Guid.NewGuid()+".html", newtext, Encoding.UTF8);
                }
                else
                {
                    return "";
                }

                //File.WriteAllText("res.txt", str);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(1000);
                //goto lab1;
            }
            return "";
        }

        static string translit(string text)
        {
            string originalString = text;
            string replacedString;
            replacedString = originalString.Replace("Є", "YE");
            replacedString = replacedString.Replace("І", "I");
            replacedString = replacedString.Replace("Ѓ", "G");
            replacedString = replacedString.Replace("і", "i");
            replacedString = replacedString.Replace("№", "");
            replacedString = replacedString.Replace("є", "ye");
            replacedString = replacedString.Replace("ѓ", "g");
            replacedString = replacedString.Replace("А", "A");
            replacedString = replacedString.Replace("Б", "B");
            replacedString = replacedString.Replace("В", "V");
            replacedString = replacedString.Replace("Г", "G");
            replacedString = replacedString.Replace("Д", "D");
            replacedString = replacedString.Replace("Е", "E");
            replacedString = replacedString.Replace("Ё", "YO");
            replacedString = replacedString.Replace("Ж", "ZH");
            replacedString = replacedString.Replace("З", "Z");
            replacedString = replacedString.Replace("И", "I");
            replacedString = replacedString.Replace("Й", "J");
            replacedString = replacedString.Replace("К", "K");
            replacedString = replacedString.Replace("Л", "L");
            replacedString = replacedString.Replace("М", "M");
            replacedString = replacedString.Replace("Н", "N");
            replacedString = replacedString.Replace("О", "O");
            replacedString = replacedString.Replace("П", "P");
            replacedString = replacedString.Replace("Р", "R");
            replacedString = replacedString.Replace("С", "S");
            replacedString = replacedString.Replace("Т", "T");
            replacedString = replacedString.Replace("У", "U");
            replacedString = replacedString.Replace("Ф", "F");
            replacedString = replacedString.Replace("Х", "X");
            replacedString = replacedString.Replace("Ц", "C");
            replacedString = replacedString.Replace("Ч", "CH");
            replacedString = replacedString.Replace("Ш", "SH");
            replacedString = replacedString.Replace("Щ", "SHH");
            replacedString = replacedString.Replace("Ъ", "");
            replacedString = replacedString.Replace("Ы", "Y");
            replacedString = replacedString.Replace("Ь", "");
            replacedString = replacedString.Replace("Э", "E");
            replacedString = replacedString.Replace("Ю", "YU");
            replacedString = replacedString.Replace("Я", "YA");
            replacedString = replacedString.Replace("а", "a");
            replacedString = replacedString.Replace("б", "b");
            replacedString = replacedString.Replace("в", "v");
            replacedString = replacedString.Replace("г", "g");
            replacedString = replacedString.Replace("д", "d");
            replacedString = replacedString.Replace("е", "e");
            replacedString = replacedString.Replace("ё", "yo");
            replacedString = replacedString.Replace("ж", "zh");
            replacedString = replacedString.Replace("з", "z");
            replacedString = replacedString.Replace("и", "i");
            replacedString = replacedString.Replace("й", "j");
            replacedString = replacedString.Replace("к", "k");
            replacedString = replacedString.Replace("л", "l");
            replacedString = replacedString.Replace("м", "m");
            replacedString = replacedString.Replace("н", "n");
            replacedString = replacedString.Replace("о", "o");
            replacedString = replacedString.Replace("п", "p");
            replacedString = replacedString.Replace("р", "r");
            replacedString = replacedString.Replace("с", "s");
            replacedString = replacedString.Replace("т", "t");
            replacedString = replacedString.Replace("у", "u");
            replacedString = replacedString.Replace("ф", "f");
            replacedString = replacedString.Replace("х", "x");
            replacedString = replacedString.Replace("ц", "c");
            replacedString = replacedString.Replace("ч", "ch");
            replacedString = replacedString.Replace("ш", "sh");
            replacedString = replacedString.Replace("щ", "shh");
            replacedString = replacedString.Replace("ъ", "");
            replacedString = replacedString.Replace("ы", "y");
            replacedString = replacedString.Replace("ь", "");
            replacedString = replacedString.Replace("э", "e");
            replacedString = replacedString.Replace("ю", "yu");
            replacedString = replacedString.Replace("я", "ya");
            replacedString = replacedString.Replace("«", "");
            replacedString = replacedString.Replace("»", "");
            replacedString = replacedString.Replace("—", "-");
            replacedString = replacedString.Replace(" — ", "-");
            replacedString = replacedString.Replace(" - ", "-");
            replacedString = replacedString.Replace(" ", "-");
            replacedString = replacedString.Replace("...", "");
            replacedString = replacedString.Replace("..", "");
            replacedString = replacedString.Replace(":", "");
            replacedString = replacedString.Replace("\"", "");
            replacedString = replacedString.Replace(",", "");
            replacedString = replacedString.Replace("!", "");
            replacedString = replacedString.Replace(";", "");
            replacedString = replacedString.Replace("%", "");
            replacedString = replacedString.Replace("?", "");
            replacedString = replacedString.Replace("*", "");
            replacedString = replacedString.Replace("(", "");
            replacedString = replacedString.Replace(")", "");
            replacedString = replacedString.Replace("\\", "");
            replacedString = replacedString.Replace("/", "");
            replacedString = replacedString.Replace("=", "");
            replacedString = replacedString.Replace("'", "");
            replacedString = replacedString.Replace("&", "");
            replacedString = replacedString.Replace("^", "");
            replacedString = replacedString.Replace("$", "");
            replacedString = replacedString.Replace("#", "");
            replacedString = replacedString.Replace("@", "");
            replacedString = replacedString.Replace("~", "");
            replacedString = replacedString.Replace("`", "");
            replacedString = replacedString.Replace(" ", "-");


            return replacedString;
        }
        static string addkw(string text, string kw)
        {
            int totalsym = text.Length;
            int kwcount = totalsym / 1500;
            if (kwcount == 0) kwcount = 1;

            String pattern = @"[А-Яа-я]{5,50}\.";
            MatchCollection matchlist;
            Regex exp = new Regex(pattern, RegexOptions.IgnoreCase);
            matchlist = exp.Matches(text);
            
            List<string> conci = new List<string>();
            foreach (Match match in matchlist)
                {
                    conci.Add(match.Value);
                    Console.WriteLine(match.Value);
                }

            Random rand = new Random();
            int temp;

            List<int> predlojeniya_v_kotorih_zamena = new List<int>();
            for (int i = 0; i < kwcount; i++)
            {
                temp = rand.Next(conci.Count());
                if (temp > 6)
                {
                    predlojeniya_v_kotorih_zamena.Add(temp);
                }
            }

            var uniq = predlojeniya_v_kotorih_zamena.Distinct();

            foreach(int intem in uniq)
            {
                text = text.Replace(conci[intem], kw + " " + conci[intem]);
            }
            //text = text.Replace(conci[4], conci[4] + " \r\n <!--more--> \r\n "); 
            text = videlenie(text);
            Console.WriteLine(kwcount);
            return Regex.Replace(text, "<h(7|8)[^>]*>(.*?)</h(7|8)>", String.Empty);
        }
        static string videlenie(string str)
        {
            string result = "";
            string sellang = "ru";
            //string[] patterns = new string[] { @"\s([A-Za-z]|[А-Яа-я])+\s([A-Za-z]|[А-Яа-я])+\s", @"\s([A-Za-z]|[А-Яа-я])+\s([A-Za-z]|[А-Яа-я])+\s([A-Za-z]|[А-Яа-я])+\s", @"\s([A-Za-z]|[А-Яа-я])+\s([A-Za-z]|[А-Яа-я])+\s([A-Za-z]|[А-Яа-я])+\s([A-Za-z]|[А-Яа-я])+\s" };
            string[] patterns = new string[] { @"\s([A-Za-zА-Яа-я])+\s([A-Za-zА-Яа-я])+\s", @"\s([A-Za-zА-Яа-я])+\s([A-Za-zА-Яа-я])+\s([A-Za-zА-Яа-я])+\s", @"\s([A-Za-zА-Яа-я])+\s([A-Za-zА-Яа-я])+\s([A-Za-zА-Яа-я])+\s([A-Za-zА-Яа-я])+\s" };

            int totalsym = str.Length;
            int kwcount = totalsym / 1500;
            if (kwcount == 0) kwcount = 1;

            for (int i = 0; i <= kwcount; i++)
            {
                Random rand = new Random();
                int rnd = rand.Next(0, 2);
                string pattern = patterns[rnd];
                var arr = Regex.Matches(str, pattern)
    .Cast<Match>()
    .Select(m => m.Value)
    .ToArray();
                if (arr.Length <= 0) continue;
                string repl = arr[rand.Next(0, arr.Length - 1)].Trim();
                str = str.Replace(repl, "<strong>" + repl + "</strong>");
            }
            result = str;
            return result;
        }
        static string GetPage(string url)
        {
            Console.WriteLine("Загрузка страницы Гоши: " + url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@url);

            request.UserAgent = "Mozilla/5.0";
            request.AllowAutoRedirect = true;
            request.Referer = "http://www.webcrawler.com/";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string str = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            return str;
        }
        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
