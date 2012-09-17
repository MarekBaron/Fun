using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baron.Mandelbrot
{
    interface IGenerator
    {      
        byte[] Generate(int aWidth, int aHeight, double aSetStartX, double aSetWidth, double aSetStartY, double aSetHeight, byte[] aPalette);
    }
}
