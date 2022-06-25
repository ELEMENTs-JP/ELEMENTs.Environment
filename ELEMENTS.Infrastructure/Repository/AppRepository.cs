using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ELEMENTS.Infrastructure;

namespace ELEMENTS.Infrastructure
{
    public interface IAppRepository
    {
        List<IApp> Apps { get; set; }

        void Init();
    }
    public class AppRepository : IAppRepository, INavigationRepository
    {
        public List<IApp> Apps { get; set; } = new List<IApp>();

        public void Init()
        {
            Apps.Clear();
            Apps.Add(new RUNer());
            Apps.Add(new FILEster());

            Items.Clear();
            foreach (IApp app in Apps)
            {
                Items.Add(new NavigationEntry() { Title = app.Title, ID = app.ID });
            }
        }

        public string Title { get; set; } = "Applications";
        public string Text { get; set; }
        public List<NavigationEntry> Items { get; set; } = new List<NavigationEntry>();
        public void Save()
        { }
    }
}
