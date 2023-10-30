using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTracker2020.Models
{
    public class ChartJSModel
    {
        public ChartJSModel()
        {
            Labels = new List<string>();//stores labels and displayed on chart

            Data = new List<int>();//stores integers and displays on chart

            BackgroundColor = new List<string>();//stores background colors displayed on chart
        }
        public List<string> Labels { get; set; }

        public List<int> Data { get; set; }

        public List<string> BackgroundColor { get; set; }
    }
}
