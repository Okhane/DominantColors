using System.Collections.Generic;
using System.Drawing;
using System;
using System.Linq;
using System.Windows.Forms;

namespace DominantColorsForm
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static public void findDominantColor(string path, ref Color averageColor, ref Color mostUsedColor)
        {
            // Create a bitmap object using a picture
            Bitmap myBitmap = new Bitmap(path);
            // Extract the height and width of that picture
            int imageHeight = myBitmap.Size.Height;
            int imageWidth = myBitmap.Size.Width;

            List<Color> pixelList = new List<Color>();

            // Scan every pixels in X and Y dimensions and extract the color as ARGB
            // A = Alpha (0 to 255)
            // R = Reds (0 to 255)
            // G = Green (0 to 255)
            // B = Blue (0 to 255)
            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {

                    // Add the color to the pixelList
                    pixelList.Add(myBitmap.GetPixel(x, y));
                }
            }

            long redAmount = 0;
            long greenAmount = 0;
            long blueAmount = 0;

            List<string> RGBValuesList = new List<string>();

            // Scan the pixel list
            foreach (Color pixel in pixelList)
            {
                int red = Convert.ToInt32(pixel.R);
                int green = Convert.ToInt32(pixel.G);
                int blue = Convert.ToInt32(pixel.B);

                string test = red.ToString() + "," + green.ToString() + "," + blue.ToString();

                redAmount += red;
                greenAmount += green;
                blueAmount += blue;
                RGBValuesList.Add(test);
            }


            // OPTION 1 - Find the average colors for Red, Green and Blue as bytes
            byte averageRed = Convert.ToByte(redAmount / (imageHeight * imageWidth));
            byte averageGreen = Convert.ToByte(greenAmount / (imageHeight * imageWidth));
            byte averageBlue = Convert.ToByte(blueAmount / (imageHeight * imageWidth));

            averageColor = Color.FromArgb(averageRed, averageGreen, averageBlue);

            Console.WriteLine("Average color is {0},{1},{2}.", averageRed, averageGreen, averageBlue);
            // OPTION 2 - Find the most used combination of colors for Red, Green and Blue as bytes

            // Select in descending order the most used colors
            var duplicates = RGBValuesList.GroupBy(x => new { x })
                .Select(group => new { Name = group.Key, Count = group.Count() })
                .OrderByDescending(x => x.Count);

            // Take the first element in the selection, as it is ordered by descending count
            int mostUsedRGBCount = duplicates.ElementAt(0).Count;
            string[] mostUsedRGB = duplicates.ElementAt(0).Name.x.Split(',');
            byte mostUsedRed = Convert.ToByte(mostUsedRGB[0]);
            byte mostUsedGreen = Convert.ToByte(mostUsedRGB[1]);
            byte mostUsedBlue = Convert.ToByte(mostUsedRGB[2]);

            mostUsedColor = Color.FromArgb(mostUsedRed, mostUsedGreen, mostUsedBlue);

            Console.WriteLine("Most used color is {0},{1},{2} and appeared {3} times.", mostUsedRed, mostUsedGreen, mostUsedBlue, mostUsedRGBCount);


        }
    }
}
