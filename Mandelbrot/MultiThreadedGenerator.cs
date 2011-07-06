using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baron.Mandelbrot
{
    public class MultiThreadedGenerator : IGenerator
    {
        public byte[] Generate(int aWidth, int aHeight, float aSetStartX, float aSetWidth, float aSetStartY, float aSetHeight, byte[] aPalette)
        {
            byte[] rgbaValues = new byte[aWidth * aHeight * 4];
            float x_step = aSetWidth / aWidth;
            float y_step = aSetHeight / aHeight;

            Parallel.For(0, aHeight, screenY =>
            {
                for (int screenX = 0; screenX < aWidth; screenX++)
                {
                    byte iteration = GetIteration(aSetStartX, aSetStartY, x_step, y_step, screenY, screenX);

                    int bitmapPointer = screenY * aWidth * 4 + screenX * 4;
                    rgbaValues[bitmapPointer++] = aPalette[4 * iteration]; //B
                    rgbaValues[bitmapPointer++] = aPalette[4 * iteration + 1]; //G
                    rgbaValues[bitmapPointer++] = aPalette[4 * iteration + 2]; //R
                    rgbaValues[bitmapPointer++] = aPalette[4 * iteration + 3]; //A                 
                }

            });
            return rgbaValues;
        }

        private byte GetIteration(float aSetStartX, float aSetStartY, float x_step, float y_step, int screenY, int screenX)
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
            return iteration;
        }

        public override string ToString()
        {
            return "Multi threaded generator";
        }
    }
}
