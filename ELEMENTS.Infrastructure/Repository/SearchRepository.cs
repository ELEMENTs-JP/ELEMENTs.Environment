using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ELEMENTS.Infrastructure;

namespace ELEMENTS
{
    public interface ISearchRepository
    {
        string Matchcode { get; set; }
        string FilterValue { get; set; }
        IList<IDTO> Items { get; set; }
        IList<IDTO> Store { get; set; }
        IList<string> Filter { get; set; }
        void SearchItems();
        void FilterValues(KeyValuePair<string, bool> filter);
        void Init();
    }
    public class SearchRepository : ISearchRepository
    {
        public string Matchcode { get; set; } = string.Empty;
        public string FilterValue { get; set; } = string.Empty;

        public IList<IDTO> Items { get; set; } = new List<IDTO>();
        public IList<IDTO> Store { get; set; } = new List<IDTO>();
        public IList<string> Filter { get; set; } = new List<string>();

        public void Init()
        {
            Store.Clear();

            Store.Add(new DTO { ID = Guid.NewGuid().ToString(), Title = "New York", Content = "Content", NavigateUrl = "https://www.google.de" });
            Store.Add(new DTO { ID = Guid.NewGuid().ToString(), Title = "Washington", Content = "Content", NavigateUrl = "https://www.google.de" });
            Store.Add(new DTO { ID = Guid.NewGuid().ToString(), Title = "Seattle", Content = "Content", NavigateUrl = "https://www.google.de" });
            Store.Add(new DTO { ID = Guid.NewGuid().ToString(), Title = "Chicago", Content = "Content", NavigateUrl = "https://www.google.de" });
            Store.Add(new DTO { ID = Guid.NewGuid().ToString(), Title = "Detroit", Content = "Content", NavigateUrl = "https://www.google.de" });
            Store.Add(new DTO { ID = Guid.NewGuid().ToString(), Title = "Texas", Content = "Content", NavigateUrl = "https://www.google.de" });
            Store.Add(new DTO { ID = Guid.NewGuid().ToString(), Title = "Las Vegas", Content = "Content", NavigateUrl = "https://www.google.de" });

            Filter.Clear();
            Filter = Store.DistinctBy(se => se.Title).Select(se => se.Title).ToList();
        }

        public void SearchItems()
        {
            Items.Clear();

            if (string.IsNullOrEmpty(Matchcode))
            {
                return;
            }

            foreach (IDTO dto in Store.Where(se => se.Title.ToLower().Contains(Matchcode.ToLower())))
            {
                Items.Add(dto);
            }
        }

        public void FilterValues(KeyValuePair<string, bool> filter)
        {
            FilterValue = filter.Key.ToSecureString() + " - " + filter.Value.ToSecureString();

            if (filter.Value == true)
            {
                Matchcode = filter.Key.ToSecureString();
            }
            else
            {
                Matchcode = string.Empty;
            }

            SearchItems();
        }
    }
}
