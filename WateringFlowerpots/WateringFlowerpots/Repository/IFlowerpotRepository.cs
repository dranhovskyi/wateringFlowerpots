using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WateringFlowerpots.Models;

namespace WateringFlowerpots.Repository
{
    public interface IFlowerpotRepository
    {
        Task AddNewFlowerpotAsync(byte[] imageArray, string name, int volume, string dayOfWeek);
        Task DeleteFlowerpotAsync(int id);
        Task<List<Flowerpot>> GetAllFlowerpotAsync();
    }
}
