using System;
using System.IO;
using SQLite;
using Xamarin.Forms;

namespace WateringFlowerpots.Models
{
    [Table("flowerpot")]
    public class Flowerpot
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }

        public int Volume { get; set; }

        public byte[] ImageData { get; set; }

        public string DayOfTheWeek { get; set; }

        [Ignore]
        public ImageSource Image
        {
            get
            {
                return ImageSource.FromStream(() =>
                {
                    return new MemoryStream(this.ImageData);
                });
            }
        }
    }
}
