﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace Baron.Mandelbrot
{
    public class CPUGenerator : IGenerator
    {
        public byte[] Generate(int aWidth, int aHeight, double aSetStartX, double aSetWidth, double aSetStartY, double aSetHeight, byte[] aPalette)
        {
            byte[] rgbaValues = new byte[aWidth * aHeight * 4];
            double x_step = aSetWidth / aWidth;
            double y_step = aSetHeight / aHeight;

            int bitmapPointer = 0;
            for (int screenY = 0; screenY < aHeight; screenY++)
            {
                for (int screenX = 0; screenX < aWidth; screenX++)
                {

                    double x0 = aSetStartX + screenX * x_step;
                    double y0 = aSetStartY + screenY * y_step;
                    byte iteration = 0;
                    double x = 0;
                    double y = 0;
                    while (x * x + y * y <= 4 && iteration < 255)
                    {
                        double tempX = x * x - y * y + x0;
                        y = 2 * x * y + y0;
                        x = tempX;
                        iteration++;
                    }
                    rgbaValues[bitmapPointer++] = aPalette[4 * iteration]; //B
                    rgbaValues[bitmapPointer++] = aPalette[4 * iteration + 1] ; //G
                    rgbaValues[bitmapPointer++] = aPalette[4 * iteration + 2]; //R
                    rgbaValues[bitmapPointer++] = aPalette[4 * iteration + 3]; //A                 
                }

            }
            return rgbaValues;
        }        

        public override string ToString()
        {
            return "Single threaded generator";
        }
    }
}
