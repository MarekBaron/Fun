using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace Mandelbrot
{
    public class CPUGenerator
    {
        public byte[] Generate(int aWidth, int aHeight, float aSetStartX, float aSetWidth, float aSetStartY, float aSetHeight, byte[] aPalette)
        {
            byte[] rgbaValues = new byte[aWidth * aHeight * 4];
            float x_step = aSetWidth / aWidth;
            float y_step = aSetHeight / aHeight;

            int bitmapPointer = 0;
            for (int screenY = 0; screenY < aHeight; screenY++)
            {
                for (int screenX = 0; screenX < aWidth; screenX++)
                {

                    float x0 = aSetStartX + screenX * x_step;
                    float y0 = aSetStartY + screenY * y_step;
                    byte iteration = 0;
                    float x = 0;
                    float y = 0;
                    while (x * x + y * y <= 4 && iteration < 255)
                    {
                        float tempX = x * x - y * y + x0;
                        y = 2 * x * y + y0;
                        x = tempX;
                        iteration++;
                    }
                    if (iteration > 250)
                    {

                    }
                    rgbaValues[bitmapPointer++] = aPalette[3 * iteration]; //B
                    rgbaValues[bitmapPointer++] = aPalette[3 * iteration + 1] ; //G
                    rgbaValues[bitmapPointer++] = aPalette[3 * iteration + 2]; //R
                    rgbaValues[bitmapPointer++] = 255; //A                 
                }

            }
            return rgbaValues;
        }
    }
}
