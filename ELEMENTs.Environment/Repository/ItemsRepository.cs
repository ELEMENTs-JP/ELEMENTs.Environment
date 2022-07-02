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
        public int TotalPageCount { get; set; } = 1;
        public ISQLiteService Service { get; set; }
        public int PageSize { get; set; } = 10;
        public int QueryCount { get; set; } = 0;
        public int CurrentPage { get; set; } = 1;
        public string ItemType { get; set; } = string.Empty;

        private int ic = 0;
        public int ItemCount 
        {
            get 
            {
                if (ic == 0)
                {
                    ic = GetItemCount();
                    return ic;
                }
                else
                {
                    return ic;
                }
            }
        }
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

                QueryCount += 1;

                // Query 
                Items = Service.Factory.GetItems(qp);

                CalculatePaging();
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

                QueryCount += 1;

                // Query 
                Items = Service.Factory.GetItems(qp);

                CalculatePaging();
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAIL: " + ex.Message);
            }

            return new List<IDTO>();
        }

        int GetItemCount()
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

        private void CalculatePaging()
        {
            try
            {
                // Page Size 
                long rest = 0;
                long quotient = Math.DivRem(ItemCount, PageSize, out rest);
                TotalPageCount = Convert.ToInt32(quotient);
                TotalPageCount = (rest >= 1) ? TotalPageCount += 1 : TotalPageCount;
            }
            catch (Exception ex)
            {
                TotalPageCount = 1;
            }
        }

    }
}
