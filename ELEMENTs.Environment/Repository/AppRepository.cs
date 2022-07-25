using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ELEMENTS.Data.SQLite;
using ELEMENTS.Infrastructure;

namespace ELEMENTS
{


    public class AppRepository : IAppRepository, INavigationRepository
    {
        public List<IApp> Apps { get; set; } = new List<IApp>();

        public AppRepository()
        {
            Init();
        }
        public void Init()
        {
            try
            {
                Apps.Clear();

                Apps.Add(new tsp.DEFAULT.DEFAULT());

                Items.Clear();
                foreach (IApp app in Apps)
                {
                    NavigationEntry entry = new NavigationEntry()
                    {
                        Title = app.Title,
                        ID = app.ID,
                        Group = app.Group
                    };

                    Store.Add(entry);
                    Items.Add(entry);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Fehler: " + ex.Message);
            }
        }

        public IItemType GetItemTypeByName(string ItemTypeName)
        {
            try
            {
                foreach (IApp a in Apps)
                {
                    foreach (IItemType i in a.GetItemTypes())
                    {
                        if (i.Title == ItemTypeName)
                        {
                            return i;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Fehler: " + ex.Message);
            }

            return null;
        }
        public IApp GetAppByItemTypeName(string ItemTypeName)
        {
            try
            {
                foreach (IApp a in Apps)
                {
                    foreach (IItemType i in a.GetItemTypes())
                    {
                        if (i.Title == ItemTypeName)
                        {
                            return a;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Fehler: " + ex.Message);
            }

            return null;
        }
        public string Title { get; set; } = "Applications";
        public string Text { get; set; }
        public List<NavigationEntry> Items { get; set; } = new List<NavigationEntry>();
        public List<NavigationEntry> Store { get; set; } = new List<NavigationEntry>();
        public void Save()
        { }

        public List<NavigationFilter> Groups { get; set; } = new List<NavigationFilter>();
        public void Filter(string GroupID)
        {
            try
            {
                Items.Clear();
                Items = Store.Where(se => se.Group == GroupID).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Fehler: " + ex.Message);
            }
        }
    }
}
