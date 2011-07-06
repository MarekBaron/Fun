using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baron.Mandelbrot
{
    class SimpleOpenCLGenerator : IGenerator
    {
        public byte[] Generate(int aWidth, int aHeight, float aSetStartX, float aSetWidth, float aSetStartY, float aSetHeight, byte[] aPalette)
        {
            return new byte[aWidth * aHeight * 4];
        }

        public override string ToString()
        {
            return "Simple OpenCL Generator";
        }
    }
}
