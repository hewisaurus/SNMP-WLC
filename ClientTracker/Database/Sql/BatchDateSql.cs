using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Sql
{
    public class BatchDateSql
    {
        public const string GetDateRange = "SELECT * FROM BatchDate WHERE Date > @startDate AND Date < @endDate ORDER BY Date";
    }
}
