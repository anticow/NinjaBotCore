using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NinjaBotCore.Database;

namespace NinjaBotCore.Modules.RandoSelector
{
    public class Rando
    {
        public void setRandomToList(string listName, string listItem, int listItemWeight, int numberOfPlayers, string userName)
        {

            var rando = new Randos();
            using (var db = new NinjaBotEntities())
            {
                if (db.Randos.Where(a => a.ListName == listName).FirstOrDefault() != null)
                {
                    var listOfRandos = db.Randos.Where(a => a.ListName == listName).ToList();
                    Randos specificItem = new Randos();
                    bool update = true;
                    try { specificItem = listOfRandos.Where((a) => a.ListItem == listItem).First(); }
                    catch (System.InvalidOperationException ex) { update = false; }
                    specificItem.ListName = listName;
                    specificItem.ListItem = listItem;
                    specificItem.ListWeight = listItemWeight;
                    specificItem.AddedBy = userName;
                    specificItem.NumberOfPlayers = numberOfPlayers;
                    specificItem.TimeSet = DateTime.Now;
                    db.Randos.Update(specificItem);
                    db.SaveChanges();
                }
                else
                {
                    rando.ListId = new Guid();
                    rando.ListName = listName;
                    rando.ListItem = listItem;
                    rando.ListWeight = listItemWeight;
                    rando.AddedBy = userName;
                    rando.TimeSet = DateTime.Now;
                    db.Randos.Add(rando);
                    db.SaveChanges();
                }
            }

        }

        public string getRandomFromList(string listName, int? numberOfPlayers)
        {
            var rando = new Randos();
            string returnItem = "";
            using (var db = new NinjaBotEntities())
            {
                if (numberOfPlayers.HasValue)
                {
                    var listOfRandos = db.Randos.Where(a => a.ListName == listName).ToList();
                    var speicificRando = from someRando in listOfRandos
                                         where someRando.NumberOfPlayers <= numberOfPlayers
                                         select someRando;
                    var random = new Random();
                    int index = random.Next(speicificRando.Count());

                    returnItem = listOfRandos[index].ListItem;
                }
                else
                {
                    rando = db.Randos.Where(a => a.ListName == listName).FirstOrDefault();
                    var listOfRandos = db.Randos.Where(a => a.ListName == rando.ListName).ToList();

                    var random = new Random();
                    int index = random.Next(listOfRandos.Count);
                    returnItem = listOfRandos[index].ListItem;
                }
                return returnItem;

            }
        }

        public void removeRandomFromList(Guid listId)
        {
            var rando = new Randos();
            string returnItem;
            using (var db = new NinjaBotEntities())
            {
                rando = db.Randos.Where(a => a.ListId == listId).FirstOrDefault();
                db.Randos.Remove(rando);
                db.SaveChanges();

            }
        }
        public Randos getFullItemContext(string listName, string listItem, int playerCount)
        {
            var rando = new Randos();
            using (var db = new NinjaBotEntities())
            {
                //rando = db.Randos.Where(a => a.ListName == listName).FirstOrDefault();
                var listOfRandos = db.Randos.Where(a => a.ListName == listName).ToList();
                var speicificRando = from someRando in listOfRandos
                                     where someRando.ListItem == listItem
                                     where someRando.NumberOfPlayers == playerCount
                                     select someRando;

                return speicificRando.First();

            }
        }
        public Randos getFullItemContext(Guid guid)
        {
            var rando = new Randos();
            using (var db = new NinjaBotEntities())
            {
                //rando = db.Randos.Where(a => a.ListName == listName).FirstOrDefault();
                var listOfRandos = db.Randos.Where(a => a.ListId == guid).ToList();
                var speicificRando = from someRando in listOfRandos
                                     select someRando;

                return speicificRando.First();

            }
        }

        public List<Randos> getItemList(string listName)
        {
            var rando = new Randos();
            using (var db = new NinjaBotEntities())
            {
                var listOfRandos = db.Randos.Where(a => a.ListName == listName).ToList();
                return listOfRandos;
                //rando = db.Randos.Where(a => a.ListName == listName).FirstOrDefault();

            }
        }
        public Dictionary<string,int> getItemList()
        {
            var rando = new Randos();
            using (var db = new NinjaBotEntities())
            {
                var listItem = db.Randos.ToList();
                var query =
                    from ListName in listItem
                    group ListName by ListName.ListName;
                Dictionary<string, int> result = new Dictionary<string, int>();
                foreach (var item in query)
                {
                    result.Add(item.Key, item.Count());
                }

                return result;
            }
        }
    }
}