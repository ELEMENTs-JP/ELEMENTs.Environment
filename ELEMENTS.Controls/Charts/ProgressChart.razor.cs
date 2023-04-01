using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ELEMENTS.Infrastructure;

namespace ELEMENTS.Controls.Charts
{
    public partial class ProgressChart : IAsyncDisposable, IDisposable
    {
        // Fields 
        private ChartDTO Configuration { get; set; } = new ChartDTO();
        private DotNetObjectReference<ProgressChart>? objRef;
        private Lazy<Task<IJSObjectReference>> moduleTask;


        // ctr 
        public ProgressChart()
        {
            Helper.LogConsole("CTR");
        }

        // Events 
        public void Init(IJSRuntime jsRuntime)
        {
            try
            {
                // Import JS File 
                moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                   "import", "./_content/ELEMENTS.Controls/progress.js").AsTask());
                // Reference 
                objRef = DotNetObjectReference.Create(this);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Fail Init Chart");
            }
        }

        // Methods 
        public async ValueTask LoadChart(string divID)
        {
            try
            {
                // Execute function
                var module = await moduleTask.Value;
                await module.InvokeVoidAsync("loadProgressChart", divID, objRef);
                this.StateHasChanged();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Fail Invoke Chart");
            }
        }
        private void LoadDefaultItems()
        {
            // Serie 
            ChartSeriesDTO serie = new ChartSeriesDTO();
            serie.Title = Legende;

            // Default Queries 
            serie.Items.Add(new ChartItemDTO { Y = 1, X = 2, Key = "Progress", Value = this.Progress, Title = "Progress" });
            serie.Items.Add(new ChartItemDTO { Y = 2, X = 3, Key = "Open", Value = (100 - this.Progress), Title = "Open" });

            // Series Append 
            Configuration.Series.Add(serie);
        }


        // JS Methods 
        [JSInvokable]
        public Task<ChartDTO> LoadChartData(JsonElement json)
        {
            Configuration = new ChartDTO();
            try
            {
                Configuration.DIV = json.GetProperty("DIV").GetString();
                Configuration.ItemType = json.GetProperty("ItemType").GetString();
                Configuration.AppType = json.GetProperty("AppType").GetString();
                Configuration.Title = this.Title;
                Configuration.Parameter = json.GetProperty("DataParameter").GetString();
                Configuration.ChartType = json.GetProperty("ChartType").GetString();

                // Serie (max. 1 Serie in diesem Control)
                ChartSeriesDTO serie = new ChartSeriesDTO();
                serie.Title = Legende;

                // Items 
                if (this.Items == null || this.Items.Count == 0)
                {
                    //LoadDefaultItems();

                    if (serie.Items.Count == 0)
                    {
                        serie.Items.Add(new ChartItemDTO() { Key = "Progress", Value = Progress, Title = "Progress" });
                        serie.Items.Add(new ChartItemDTO() { Key = "Open", Value = (100 - Progress), Title = "Open" });

                        // Series Append 
                        Configuration.Series.Add(serie);
                    }
                }
                else
                {
                    // Default Queries 
                    foreach (ChartItemDTO dto in Items.Take(2))
                    {
                        serie.Items.Add(dto);
                    }

                    if (serie.Items.Count == 0)
                    {
                        serie.Items.Add(new ChartItemDTO() { Key = "Progress", Value = Progress, Title = "Progress" });
                        serie.Items.Add(new ChartItemDTO() { Key = "Open", Value = (100 - Progress), Title = "Open" });
                    }

                    // Series Append 
                    Configuration.Series.Add(serie);
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("FAIL: " + ex.Message);
            }

            return Task.FromResult(Configuration);
        }

        // Dispose 
        public async ValueTask DisposeAsync()
        {
            try
            {
                if (moduleTask.IsValueCreated)
                {
                    var module = await moduleTask.Value;
                    await module.DisposeAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("FAIL Dipose ASYNC: " + ex.Message);
            }
        }

        public void Dispose()
        {
            try
            {
                if (moduleTask.IsValueCreated)
                {
                    var module = moduleTask.Value;
                    module.Dispose();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("FAIL Dispose: " + ex.Message);
            }
        }
    }


}
