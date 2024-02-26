using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OpusLink.Admin.Hosted.Pages.Dashboard
{
    public class DashboardAdminModel : PageModel
    {
        public class SpacingModel
        {
            public double[] CellSpacing { get; set; }
        }

        public class ChartData
        {
            public string Month { get; set; }
            public double Sales { get; set; }
        }

        public class LineData
        {
            public double X { get; set; }
            public double Y { get; set; }
        }

        public class PieData
        {
            public string X { get; set; }
            public double Y { get; set; }
            public string Text { get; set; }
        }

        public List<ChartData> ChartDataList { get; set; }
        public List<LineData> LineDataList { get; set; }
        public List<PieData> PieDataList1 { get; set; }
        public SpacingModel ModelValue { get; set; }

        public void OnGet()
        {
            ChartDataList = new List<ChartData>
            {
                // ... (same as the original code)
            };

            LineDataList = new List<LineData>
            {
                // ... (same as the original code)
            };

            PieDataList1 = new List<PieData>
            {
                // ... (same as the original code)
            };

            ModelValue = new SpacingModel
            {
                CellSpacing = new double[] { 10, 10 }
            };
        }
    }

}
