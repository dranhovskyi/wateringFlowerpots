using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace WateringFlowerpots.Extensions
{
    public static class ConversionExtensions
    {
        public async static Task<byte[]> GetBase64FromImageByteArrayAsync(this string imagePath)
        {
            var imageByteArray = new byte[] { };
            try
            {
                using (FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    imageByteArray = new byte[stream.Length];
                    await stream.ReadAsync(imageByteArray, 0, (int)stream.Length);
                    return imageByteArray;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error: {e.Message}");
            }

            return imageByteArray;
        }
    }
}
