using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using WateringFlowerpots.Models;
using Xamarin.Essentials;

namespace WateringFlowerpots.Repository
{
    public class FlowerpotRepository : IFlowerpotRepository
    {
        SQLiteAsyncConnection connection;
        public string StatusMessage { get; set; }

        public FlowerpotRepository()
        {
            connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, "wateringflowerpots.db3"));
            connection.CreateTableAsync<Flowerpot>().Wait();
        }

        public async Task AddNewFlowerpotAsync(byte[] imageArray, string name, int volume, string dayOfWeek)
        {
            int result = 0;
            try
            {
                result = await connection.InsertAsync(new Flowerpot
                {
                    Name = name,
                    ImageData = imageArray,
                    Volume = volume,
                    DayOfTheWeek = dayOfWeek
                });
                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, name);
                Debug.WriteLine(StatusMessage);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
                Debug.WriteLine(StatusMessage);
            }
        }

        public async Task<List<Flowerpot>> GetAllFlowerpotAsync()
        {
            try
            {
                return await connection.Table<Flowerpot>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
                Debug.WriteLine(StatusMessage);
            }

            return new List<Flowerpot>();
        }

        public async Task DeleteFlowerpotAsync(int id)
        {
            int result = 0;
            try
            {
                result = await connection.DeleteAsync<Flowerpot>(id);
                StatusMessage = string.Format("{0} record deleted [ID: {1})", result, id);
                Debug.WriteLine(StatusMessage);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to delete data [ID: {0}. Error: {1}", id, ex.Message);
                Debug.WriteLine(StatusMessage);
            }
        }
    }
}
