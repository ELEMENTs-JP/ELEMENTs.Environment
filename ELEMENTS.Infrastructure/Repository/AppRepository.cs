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
        List<IApp> Items { get; set; }

        void Init();
    }
    public class AppRepository : IAppRepository
    {
        public List<IApp> Items { get; set; } = new List<IApp>();

        public void Init()
        {
            Items.Clear();
            Items.Add(new RUNer());
            Items.Add(new FILEster());
        }
    }
}
