using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ELEMENTS.Infrastructure
{
    public interface IChartRepository
    {
        List<ChartItemDTO> ChartItems { get; set; }
        void Init(string Property, ChartAggregation Aggregation, ChartType Typ, List<IDTO> Items);
        ChartType Typ { get; set; }
        string Property { get; set; }
        ChartAggregation Aggregation { get; set; }
    }

    public class ChartRepository : IChartRepository
    {
        public string Property { get; set; } = string.Empty;
        public ChartType Typ { get; set; } = ChartType.Pie;
        public ChartAggregation Aggregation { get; set; } = ChartAggregation.CreatorAsCount;
        public List<ChartItemDTO> ChartItems { get; set; } = new List<ChartItemDTO>();
        List<IDTO> Items { get; set; } = new List<IDTO>();
        public void Init(string Property, ChartAggregation Aggregation, ChartType Typ, List<IDTO> Items)
        {
            this.Property = Property;
            this.Aggregation = Aggregation;
            this.Typ = Typ;
            this.Items = Items;

            ChartItems = new List<ChartItemDTO>();

            LoadItems();
        }
        private void LoadItems()
        {
            if (string.IsNullOrEmpty(Property))
            { return; }

            if (Aggregation == ChartAggregation.NULL)
                return;

            if (Typ == ChartType.NULL)
                return;

            if (Items == null)
                return;

            if (Items.Count == 0)
            { return; }

            // Year by Date as Count
            AggregateYearByDatePropertyAsCount();

            // DEFAULTS 
            YearByCreatedAsCount();
            YearByModifiedAsCount();
            CreatorAsCount();
            ModifyerAsCount();

            // Speicifc 
            AgeOfItems();
            UpToDatenessOfItems();
            AgregAgeOfItems();
            AgregUpToDatenessOfItems();

        }

        // Year 
        private void AggregateYearByDatePropertyAsCount()
        {
            // Year 
            if (Aggregation == ChartAggregation.YearByDateAsCount)
            {
                try
                {
                    // Gruppierung 
                    List<int> Years = new List<int>();
                    foreach (IDTO dto in Items)
                    {
                        Years.Add(dto.GetProperty(this.Property).Value.ToSecureDateTime().Year);
                    }

                    // Distinct 
                    Years = Years.Distinct().ToList();

                    // Chart Items 
                    ChartItems.Clear();

                    foreach (int year in Years)
                    {
                        int count = Items.Where(e => e.GetProperty(this.Property).Value.ToSecureDateTime().Year == year).Count();

                        ChartItems.Add(new ChartItemDTO { Key = year.ToSecureString(), Title = year.ToSecureString(), Value = count });
                    }

                    // Sortierung 
                    ChartItems = ChartItems.OrderBy(e => e.Title).ToList();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("FAIL: " + ex.Message);
                }
            }
        }

        // DEFAULT
        private void YearByCreatedAsCount()
        {
            // Year 
            if (Aggregation == ChartAggregation.YearByCreatedAtAsCount)
            {
                try
                {
                    // Gruppierung 
                    List<int> Years = new List<int>();
                    foreach (IDTO dto in Items)
                    {
                        Years.Add(dto.CreatedAt.Year);
                    }

                    // Distinct 
                    Years = Years.Distinct().ToList();

                    // Chart Items 
                    ChartItems.Clear();

                    foreach (int year in Years)
                    {
                        int count = Items.Where(e => e.CreatedAt.Year == year).Count();

                        ChartItems.Add(new ChartItemDTO { Key = year.ToSecureString(), Title = year.ToSecureString(), Value = count });
                    }

                    // Sortierung 
                    ChartItems = ChartItems.OrderBy(e => e.Title).ToList();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("FAIL: " + ex.Message);
                }
            }
        }
        private void YearByModifiedAsCount()
        {
            // Year 
            if (Aggregation == ChartAggregation.YearByModifiedAtAsCount)
            {
                try
                {
                    // Gruppierung 
                    List<int> Years = new List<int>();
                    foreach (IDTO dto in Items)
                    {
                        Years.Add(dto.ModifiedAt.Year);
                    }

                    // Distinct 
                    Years = Years.Distinct().ToList();

                    // Chart Items 
                    ChartItems.Clear();

                    foreach (int year in Years)
                    {
                        int count = Items.Where(e => e.ModifiedAt.Year == year).Count();

                        ChartItems.Add(new ChartItemDTO { Key = year.ToSecureString(), Title = year.ToSecureString(), Value = count });
                    }

                    // Sortierung 
                    ChartItems = ChartItems.OrderBy(e => e.Title).ToList();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("FAIL: " + ex.Message);
                }
            }
        }
        private void CreatorAsCount()
        {
            // Year 
            if (Aggregation == ChartAggregation.CreatorAsCount)
            {
                try
                {
                    // Gruppierung 
                    List<string> Authors = new List<string>();
                    foreach (IDTO dto in Items)
                    {
                        Authors.Add(dto.Creator);
                    }

                    // Distinct 
                    Authors = Authors.Distinct().ToList();

                    // Chart Items 
                    ChartItems.Clear();

                    foreach (string auth in Authors)
                    {
                        int count = Items.Where(e => e.Creator == auth).Count();

                        ChartItems.Add(new ChartItemDTO { Key = auth.ToSecureString(), Title = auth.ToSecureString(), Value = count });
                    }

                    // Sortierung 
                    ChartItems = ChartItems.OrderByDescending(e => e.Value.ToSecureInt()).ToList();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("FAIL: " + ex.Message);
                }
            }
        }
        private void ModifyerAsCount()
        {
            // Year 
            if (Aggregation == ChartAggregation.ModifyerAsCount)
            {
                try
                {
                    // Gruppierung 
                    List<string> Editors = new List<string>();
                    foreach (IDTO dto in Items)
                    {
                        Editors.Add(dto.Modifier);
                    }

                    // Distinct 
                    Editors = Editors.Distinct().ToList();

                    // Chart Items 
                    ChartItems.Clear();

                    foreach (string auth in Editors)
                    {
                        int count = Items.Where(e => e.Modifier == auth).Count();

                        ChartItems.Add(new ChartItemDTO { Key = auth.ToSecureString(), Title = auth.ToSecureString(), Value = count });
                    }

                    // Sortierung 
                    ChartItems = ChartItems.OrderByDescending(e => e.Value.ToSecureInt()).ToList();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("FAIL: " + ex.Message);
                }
            }
        }

        private void AgeOfItems()
        {
            // Year 
            if (Aggregation == ChartAggregation.AgeOfItemsByMonth)
            {
                try
                {
                    int Year = DateTime.Now.Year;

                    // Gruppierung 
                    List<int> Month = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

                    // Chart Items 
                    ChartItems.Clear();

                    foreach (int m in Month)
                    {
                        int count = Items.Where(e => e.CreatedAt.Year == Year && e.CreatedAt.Month == m).Count();
                        ChartItems.Add(new ChartItemDTO
                        {
                            Key = Helper.MonthNameByNumber(m),
                            Title = Helper.NormalizeNumber(m) + "." + Year,
                            Value = count
                        });
                    }

                    // Sortierung 
                    ChartItems = ChartItems.OrderBy(e => e.Title).ToList();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("FAIL: " + ex.Message);
                }
            }
        }
        private void UpToDatenessOfItems()
        {
            // Year 
            if (Aggregation == ChartAggregation.UpToDatenessOfItemsByMonth)
            {
                try
                {
                    int Year = DateTime.Now.Year;

                    // Gruppierung 
                    List<int> Month = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

                    // Chart Items 
                    ChartItems.Clear();

                    foreach (int m in Month)
                    {
                        int count = Items.Where(e => e.ModifiedAt.Year == Year && e.ModifiedAt.Month == m).Count();
                        ChartItems.Add(new ChartItemDTO
                        {
                            Key = Helper.MonthNameByNumber(m),
                            Title = Helper.NormalizeNumber(m) + "." + Year,
                            Value = count
                        });
                    }

                    // Sortierung 
                    ChartItems = ChartItems.OrderBy(e => e.Title).ToList();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("FAIL: " + ex.Message);
                }
            }
        }

        private void AgregAgeOfItems()
        {
            // Year 
            if (Aggregation == ChartAggregation.AgregAgeOfItemsByMonth)
            {
                try
                {
                    int Year = DateTime.Now.Year;

                    // Gruppierung 
                    List<int> Month = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

                    // Chart Items 
                    ChartItems.Clear();
                    int sum = 0;
                    foreach (int m in Month)
                    {
                        int count = Items.Where(e => e.CreatedAt.Year == Year && e.CreatedAt.Month == m).Count();
                        sum += count;

                        ChartItems.Add(new ChartItemDTO
                        {
                            Key = Helper.MonthNameByNumber(m),
                            Title = Helper.NormalizeNumber(m) + "." + Year,
                            Value = sum
                        });

                    }

                    // Sortierung 
                    ChartItems = ChartItems.OrderBy(e => e.Title).ToList();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("FAIL: " + ex.Message);
                }
            }
        }
        private void AgregUpToDatenessOfItems()
        {
            // Year 
            if (Aggregation == ChartAggregation.AgregUpToDatenessOfItemsByMonth)
            {
                try
                {
                    int Year = DateTime.Now.Year;

                    // Gruppierung 
                    List<int> Month = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

                    // Chart Items 
                    ChartItems.Clear();
                    int sum = 0;
                    foreach (int m in Month)
                    {
                        int count = Items.Where(e => e.ModifiedAt.Year == Year && e.ModifiedAt.Month == m).Count();
                        sum += count;

                        ChartItems.Add(new ChartItemDTO
                        {
                            Key = Helper.MonthNameByNumber(m),
                            Title = Helper.NormalizeNumber(m) + "." + Year,
                            Value = sum
                        });

                    }

                    // Sortierung 
                    ChartItems = ChartItems.OrderBy(e => e.Title).ToList();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("FAIL: " + ex.Message);
                }
            }
        }

    }

    public class EditorDTO
    {
        public string DIV { get; set; } = "";
        public string ID { get; set; } = "";
        public string ItemType { get; set; } = "";
        public string AppType { get; set; } = "";
        public string Text { get; set; } = "";
    }
    public class ChartDTO
    {
        public string DIV { get; set; } = "";
        public string ItemType { get; set; } = "";
        public string AppType { get; set; } = "";
        public string Parameter { get; set; } = "";
        public string ChartType { get; set; } = "";
        public string Title { get; set; } = "";
        public List<ChartSeriesDTO> Series { get; set; } = new List<ChartSeriesDTO>();
    }
    public class ChartSeriesDTO
    {
        public string Title { get; set; } = "";
        public List<ChartItemDTO> Items { get; set; } = new List<ChartItemDTO>();
    }
    public class ChartItemDTO
    {
        public string Title { get; set; } = "";
        public string Key { get; set; } = "";
        public object Value { get; set; } = "";
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

    }

    // Chart 
    public interface IChartConfiguration
    {
        public ChartType Typ { get; set; }
        public string ItemType { get; set; }
        public ChartAggregation Aggregation { get; set; }
        public string Title { get; set; }
        IQueryParameter Query { get; set; }
    }
    public class ChartConfiguration : IChartConfiguration
    {
        public string Title { get; set; } = string.Empty;
        public IQueryParameter Query { get; set; } 

        private ChartType chartType = ChartType.NULL;
        public ChartType Typ
        {
            get { return chartType; }
            set { chartType = value; }
        }

        private string _ItemType = string.Empty;
        public string ItemType
        {
            get { return _ItemType; }
            set { _ItemType = value; }
        }

        private ChartAggregation chartAggregation = ChartAggregation.NULL;
        public ChartAggregation Aggregation
        {
            get { return chartAggregation; }
            set { chartAggregation = value; }
        }

        public static List<IChartConfiguration> GetDefaultChartConfiguration(string itemTypeName)
        {
            List<IChartConfiguration> configs = new List<IChartConfiguration>();

            // configs.Add(new ChartConfiguration { Title = "Alter (aggregiert)", Typ = ChartType.StepLine, Aggregation = ChartAggregation.AgregAgeOfItemsByMonth, ItemType = Name });
            configs.Add(new ChartConfiguration { Title = "Aktualität (aggregiert)", Typ = ChartType.Area, Aggregation = ChartAggregation.AgregUpToDatenessOfItemsByMonth, ItemType = itemTypeName });
            configs.Add(new ChartConfiguration { Title = "Aktualität", Typ = ChartType.VerticalBar, Aggregation = ChartAggregation.UpToDatenessOfItemsByMonth, ItemType = itemTypeName });

            // configs.Add(new ChartConfiguration { Title = "Autor:in", Typ = ChartType.Pie, Aggregation = ChartAggregation.CreatorAsCount, ItemType = Name });
            configs.Add(new ChartConfiguration { Title = "Editor:in", Typ = ChartType.Doughnut, Aggregation = ChartAggregation.ModifyerAsCount, ItemType = itemTypeName });

            return configs;
        }
    }

    public enum ChartType
    {
        NULL = 0,

        Line = 1,
        Area = 2,
        StepLine = 3,

        HorizontalBar = 11,
        VerticalBar = 12,

        Pie = 21,
        Doughnut = 22,
        PolarArea = 23,
        Radar = 24,

        Progress = 31,
    }
    public enum ChartAggregation
    {
        NULL = 0,

        YearByDateAsCount = 1,

        YearByCreatedAtAsCount = 11,
        YearByModifiedAtAsCount = 12,
        CreatorAsCount = 13,
        ModifyerAsCount = 14,

        AgeOfItemsByMonth = 21,
        UpToDatenessOfItemsByMonth = 22,
        AgregAgeOfItemsByMonth = 23,
        AgregUpToDatenessOfItemsByMonth = 23,

    }

    // KPI 
    public interface IKPIConfiguration
    {
        string Title { get; set; }
        string Description { get; set; }
        string ItemType { get; set; }
        IQueryParameter Query { get; set; }
    }
    public class KPIConfiguration : IKPIConfiguration
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ItemType { get; set; } = string.Empty;
        public IQueryParameter Query { get; set; }

        public static List<IKPIConfiguration> GetDefaultKPIConfiguration(Guid MasterGUID, string appTypeName, string itemTypeName)
        {
            List<IKPIConfiguration> configs = new List<IKPIConfiguration>();

            configs.Add(
                new KPIConfiguration { 
                    Title = "Menge", 
                    Description = "Anzahl der Einträge: " + itemTypeName, 
                    ItemType = itemTypeName,
                    Query = QueryParameter.DefaultItemsQuery(
                        MasterGUID,
                        appTypeName,
                        itemTypeName, QueryType.List)
                
                });

            return configs;
        }
    }

    // Dashboard 
    public interface IDashboardConfiguration
    {
        string Title { get; set; }
        string Description { get; set; }
        DashboardType DashboardTyp { get; set; }
        List<IKPIConfiguration> KPIConfigurations { get; set; }
        List<IChartConfiguration> ChartConfigurations { get; set; }
    }
    public class DashboardConfiguration : IDashboardConfiguration
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DashboardType DashboardTyp { get; set; } = DashboardType.SystemDashboard;
        public List<IKPIConfiguration> KPIConfigurations { get; set; } = new List<IKPIConfiguration>();
        public List<IChartConfiguration> ChartConfigurations { get; set; } = new List<IChartConfiguration>();

      
    }

}
