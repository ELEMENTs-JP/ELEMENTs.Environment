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
        public string Information { get; set; } = string.Empty;
        public bool AssignItems { get; set; } = false;
        public int TotalPageCount { get; set; } = 1;
        public ISQLiteService Service { get; set; }
        public int PageSize { get; set; } = 10;
        public int QueryCount { get; set; } = 0;
        public int CurrentPage { get; set; } = 1;
        public IItemType ReferenceItemType { get; set; }
        public IItemType ItemType { get; set; } = new tsp.DEFAULT.SystemFileItemType();
        public QueryType DataQueryType { get; set; } = QueryType.List;
        public Guid ReferenceGUID { get; set; } = Guid.Empty;

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
                if (Service != null)
                {
                    // Prepare 
                    IQueryParameter qp = QueryParameter.DefaultItemsQuery(
                        Service.Factory.MasterGUID, "ELEMENTs", this.ItemType.Name);
                    qp.PageSize = PageSize;
                    qp.CurrentPage = CurrentPage;

                    QueryCount += 1;

                    // Query 
                    Items = Service.Factory.GetItems(qp);

                    CalculatePaging();
                }
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
                    Service.Factory.MasterGUID, "ELEMENTs", this.ItemType.Name);
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
        public IFactoryStatusInfo Delete(IDTO dto)
        {

            IFactoryStatusInfo info = new FactoryStatusInfo();
            info.Status = "OK";
            info.Message = "";

            if (dto == null)
            {
                info.Status = "FAIL";
                info.Message = "DTO = NULL";
                return info;
            }

            try
            {
                // Delete 
                IInputDTO input = InputDTO.CreateTemplate(
                    dto.GUID, dto.Title,
                    Service.Factory.MasterGUID,
                    "ELEMENTs", this.ItemType.Name);
                info = Service.Factory.Delete(input);
            }
            catch (Exception ex)
            {
                info.Status = "FAIL";
                info.Message = "Error: " + ex.Message;
            }

            return info;
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
        public IFactoryStatusInfo UnlinkItem(IDTO item)
        {
            IFactoryStatusInfo info = new FactoryStatusInfo();
            info.Status = "OK";
            info.Message = string.Empty;

            try
            {
                if (ReferenceGUID != Guid.Empty &&
                    ReferenceItemType != null)
                {
                    if (item != null)
                    {
                        IRelationDTO relation = null;

                        // Parent
                        if (DataQueryType == QueryType.ParentsByChild)
                        {
                            relation = RelationDTO.CreateTemplate(
                                item.GUID, item.ItemType,
                                ReferenceGUID, ReferenceItemType.Name,
                                "Association", Service.Factory.MasterGUID);
                        }

                        // Child
                        if (DataQueryType == QueryType.ChildrenByParent)
                        {
                            relation = RelationDTO.CreateTemplate(
                                ReferenceGUID, ReferenceItemType.Name,
                                item.GUID, item.ItemType,
                                "Association", Service.Factory.MasterGUID);
                        }

                        // Remove
                        if (relation != null)
                        {
                            info = Service.Factory.RemoveRelation(relation);
                            if (info.Status != "OK")
                            {
                                System.Diagnostics.Debug.WriteLine("Fehler beim Remove: " + info.Message);
                            }

                            Matchcode = string.Empty;
                            return info;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("");
            }

            return info;
        }
        public IFactoryStatusInfo LinkItem(IDTO item)
        {
            IFactoryStatusInfo info = new FactoryStatusInfo();
            info.Status = "OK";
            info.Message = string.Empty;

            try
            {
                if (ReferenceGUID != Guid.Empty &&
                    ReferenceItemType != null)
                {
                    if (item != null)
                    {
                        IRelationDTO relation = null;

                        // Parent
                        if (DataQueryType == QueryType.ParentsByChild)
                        {
                            relation = RelationDTO.CreateTemplate(
                                item.GUID, item.ItemType,
                                ReferenceGUID, ReferenceItemType.Name,
                                "Association", Service.Factory.MasterGUID);
                        }

                        // Child
                        if (DataQueryType == QueryType.ChildrenByParent)
                        {
                            relation = RelationDTO.CreateTemplate(
                                ReferenceGUID, ReferenceItemType.Name,
                                item.GUID, item.ItemType,
                                "Association", Service.Factory.MasterGUID);
                        }

                        // Remove
                        if (relation != null)
                        {
                            info = Service.Factory.AssignRelation(relation);
                            if (info.Status != "OK")
                            {
                                System.Diagnostics.Debug.WriteLine("Fehler beim Remove: " + info.Message);
                            }

                            Matchcode = string.Empty;
                            return info;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("");
            }

            return info;
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
