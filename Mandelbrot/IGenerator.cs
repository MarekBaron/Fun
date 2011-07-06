using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baron.Mandelbrot
{
    interface IGenerator
    {      
        byte[] Generate(int aWidth, int aHeight, float aSetStartX, float aSetWidth, float aSetStartY, float aSetHeight, byte[] aPalette);
    }
}
