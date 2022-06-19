using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ELEMENTS;
using ELEMENTS.Data.SQLite;
using ELEMENTS.Infrastructure;

namespace ELEMENTS
{
  
    public class ItemsRepository : IItemsRepository
    {
        public ISQLiteService Service { get; set; }
        public int PageSize { get; set; } = 10;
        public int CurrentPage { get; set; } = 1;
        public string Matchcode { get; set; } = string.Empty;
        public List<IDTO> Items { get; set; } = new List<IDTO>();
        public List<IDTO> Load()
        {
            try
            {
                // Prepare 
                IQueryParameter qp = QueryParameter.DefaultItemsQuery(
                    Service.Factory.MasterGUID, "ELEMENTs", "Product");
                qp.PageSize = PageSize;
                qp.CurrentPage = CurrentPage;

                // Query 
                Items = Service.Factory.GetItems(qp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAIL: " + ex.Message);
            }

            return new List<IDTO>();
        }
        public List<IDTO> Search()
        {
            try
            {
                // Prepare 
                IQueryParameter qp = QueryParameter.DefaultItemsQuery(
                    Service.Factory.MasterGUID, "ELEMENTs", "Product");
                qp.Matchcode = Matchcode;
                qp.PageSize = PageSize;
                qp.CurrentPage = CurrentPage;

                // Query 
                Items = Service.Factory.GetItems(qp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAIL: " + ex.Message);
            }

            return new List<IDTO>();
        }

        public int ItemCount()
        {
            try
            {
                // Prepare 
                IQueryParameter qp = QueryParameter.DefaultItemsQuery(
                    Service.Factory.MasterGUID, "ELEMENTs", "Product");

                // Query 
                return Service.Factory.GetItemsCount(qp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAIL: " + ex.Message);
            }

            return 0;
        }
    
    }
}
