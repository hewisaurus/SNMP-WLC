using System.Collections.Generic;
using System.Threading.Tasks;
using DotNet.Highcharts;

namespace WebMvc5.Helpers.Charts
{
    public interface IChartRepository
    {
        Task<Highcharts> GetOverallClientCount(int lastXHours);
        Task<Highcharts> GetOverallClientCount(ChartPeriod chartPeriod);

        Task<Highcharts> GetPerVlanClientCount(int lastXHours);
        Task<Highcharts> GetPerAccessPointClientCount(int lastXHours, int showTopX = 5);

        Task<Highcharts> GetClientsPerMonth(int monthNumber, int apId = 0, int vlanId = 0);
    }
}