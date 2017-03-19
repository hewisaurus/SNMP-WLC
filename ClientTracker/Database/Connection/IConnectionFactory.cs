using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Connection
{
    public interface IConnectionFactory
    {
        DbConnection Create();
        string GetConnectionString();
    }
}
