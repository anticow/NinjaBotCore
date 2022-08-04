using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NinjaBotCore.Database;

namespace NinjaBotCore.Modules.RandoSelector
{
    public class RandoData
    {
        public void setRandomToList(string listName, string listItem, int listItemWeight, int numberOfPlayers, string userName)
        {

            var rando = new Rando();
            using (var db = new NinjaBotEntities())
            {
                
                try 
                    {
                    rando = db.RandoList.Where(a => a.ListName == listName).FirstOrDefault();
                    rando.ListName = listName;
                    rando.ListItem = listItem;
                    rando.ListWeight = listItemWeight;
                    rando.AddedBy = userName;

                    rando.TimeSet = DateTime.Now;

                }
                catch (NullReferenceException)
                {
                    rando.ListName = listName;
                    rando.ListItem = listItem;
                    rando.ListWeight = listItemWeight;
                    rando.AddedBy = userName;
                    rando.TimeSet = DateTime.Now;

                }
                db.SaveChanges();
            }

        }

        public string getRandomFromList(string listName)
        {
            var rando = new Rando();
            string returnItem;
            using (var db = new NinjaBotEntities())
            {
                rando = db.RandoList.Where(a => a.ListName == listName).FirstOrDefault();
                var random = new Random();
                var listOfRandos = db.RandoList.ToList();
                int index = random.Next(listOfRandos.Count);
                returnItem = listOfRandos[index].ListItem;
            }
            return rando.ListItem;
        }

    }
}
