using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Database.Models;

namespace Database
{
    public interface IDatabaseRepository
    {
        void LogLine(string line);

        Task<ReturnValue> UpdateAccessPointModels(List<string> models);
        Task<List<AccessPointModel>> GetApModelsAsync();

        Task<ReturnValue> UpdateAccessPoints(List<AccessPoint> accessPoints);
    }
}
