using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Discord.Commands;
using HtmlAgilityPack;
using System.Net;
using System.IO;

namespace daily_store.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("Pong");
        }

        [Command("dailyshop")]
        public async Task DailyShopAsync()
        {
            WebClient web1 = new WebClient();
            WebClient web2 = new WebClient();

            List<string> mesImagesDL = new List<string>();

            string data = web1.DownloadString("https://fallguysstore.com");
            string dataSplit = "<h3>Featured Items</h3>";
            string dataParagraphe = data.Substring(data.IndexOf(dataSplit) + dataSplit.Length);
            string[] separateurs = new string[] { "</p>" };
            string[] paragraphes = dataParagraphe.Split(separateurs, StringSplitOptions.None);
            string monParagraphe = paragraphes[0];

            string[] separateursImg = new string[] { "</a>" };
            string[] mesImages = monParagraphe.Split(separateursImg, StringSplitOptions.None);

            // Pour toutes mes images (texte html)
            for (int i = 0; i < mesImages.Length - 1; i++)
            {
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(mesImages[i]);
                String link = doc.DocumentNode.SelectSingleNode("//a").Attributes["href"].Value;
                mesImagesDL.Add(link);
            }

            int i2 = 0;

            foreach (String images in mesImagesDL)
            {
                web2.DownloadFile(new Uri(images.ToString()), @"c:\temp\image" + i2 + ".jpg");
                i2++;
            }

            int i3 = 1;

            foreach (String jpg in Directory.GetFiles(@"c:\temp\", "*.jpg"))
            {
                await Context.Channel.SendFileAsync(jpg, "Item numéro " + i3);
                i3++;
                File.Delete(jpg);
            }
        }

        [Command("jeff")]
        public async Task jeff()
        {
            await ReplyAsync("Jeff le gros BG");
        }
    }
}
