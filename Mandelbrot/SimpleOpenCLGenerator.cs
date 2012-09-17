using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloo;
using System.Windows.Forms;

namespace Baron.Mandelbrot
{
    class SimpleOpenCLGenerator : IGenerator
    {
        private void Initialize()
        {
            var str = CLProgram();

            _initialized = true;
            var platform = ComputePlatform.Platforms[0];

            ComputeContextPropertyList properties = new ComputeContextPropertyList(platform);
            _context = new ComputeContext(platform.Devices, properties, null, IntPtr.Zero);

            // Create and build the opencl program.
            var program = new ComputeProgram(_context, str);
            try
            {
                program.Build(null, null, null, IntPtr.Zero);
            }
            catch
            {
                var msg = String.Join(Environment.NewLine, platform.Devices.Select(d => String.Format("{0}: {1} : {2}", d.Name, program.GetBuildStatus(d), program.GetBuildLog(d))));
                MessageBox.Show(msg, "Błąd OpenCL");
                Application.Exit();
            }

            // Create the event wait list. An event list is not really needed for this example but it is important to see how it works.
            // Note that events (like everything else) consume OpenCL resources and creating a lot of them may slow down execution.
            // If "null" is used instead of "eventList", no events will be created.
            _eventList = new ComputeEventList();

            // Create the command queue. This is used to control kernel execution and manage read/write/copy operations.
            _commands = new ComputeCommandQueue(_context, _context.Devices[0], ComputeCommandQueueFlags.None);

            _kernel = program.CreateKernel("Mandelbrot");

        }

        private ComputeBuffer<T> ValueToComputeBuffer<T>(T aValue)
            where T : struct
        {
            return new ComputeBuffer<T>(_context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, new T[] { aValue });
        }

        public byte[] Generate(int aWidth, int aHeight, float aSetStartX, float aSetWidth, float aSetStartY, float aSetHeight, byte[] aPalette)
        {
            if (!_initialized)
                Initialize();

           // _kernel.SetArgument(0, (IntPtr)sizeof(float), (IntPtr)aSetStartX);

            _kernel.SetMemoryArgument(0, ValueToComputeBuffer(aSetStartX));
            _kernel.SetMemoryArgument(1, ValueToComputeBuffer(aSetWidth / aWidth));
            _kernel.SetMemoryArgument(2, ValueToComputeBuffer(aSetStartY));
            _kernel.SetMemoryArgument(3, ValueToComputeBuffer(aSetHeight / aHeight));
            _kernel.SetMemoryArgument(4, new ComputeBuffer<byte>(_context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, aPalette));

            byte[] rgbaValues = new byte[aWidth * aHeight * 4];
            // The output buffer doesn't need any data from the host. Only its size is specified (rgbaValues.Length).
            ComputeBuffer<byte> rgbValuesBuffer = new ComputeBuffer<byte>(_context, ComputeMemoryFlags.WriteOnly, rgbaValues.Length);
            _kernel.SetMemoryArgument(5, rgbValuesBuffer);

            // Execute the kernel "aWidth * aHeight" times. After this call returns, "eventList" will contain an event associated with this command.            
            _commands.Execute(_kernel, null, new long[] { aWidth, aHeight }, null, _eventList);
            // Read back the results. "eventList" will now contain two events.
            _commands.ReadFromBuffer(rgbValuesBuffer, ref rgbaValues, true, _eventList);
            return rgbaValues;
        }

        private bool _initialized = false;
        private ComputeEventList _eventList = null;
        private ComputeCommandQueue _commands = null;
        private ComputeKernel _kernel = null;
        private ComputeContext _context = null;

        private string CLProgram()
        {
            return
            @"
            kernel void Mandelbrot(
                global  read_only float* setStartX,
                global  read_only float* xStep,
                global  read_only float* setStartY,
                global  read_only float* yStep,
                global  read_only uchar* palette,
                global write_only uchar* rgbaValues)
            {
                int startX = get_global_id(0);
                int startY = get_global_id(1);
                int width = get_global_size(0);
                int height = get_global_size(1);

                float x0 = setStartX[0] + startX * xStep[0];
                float y0 = setStartY[0] + startY * yStep[0];
                uchar iteration = 0;
                float x = 0;
                float y = 0;

                while (x * x + y * y <= 4 && iteration < 255)
                {
                    float tempX = x * x - y * y + x0;
                    y = 2 * x * y + y0;
                    x = tempX;
                    iteration++;
                }
                int rgbaValuesIndex = (startX + startY * width) * 4;
                int paletteIndex = iteration * 4;
                rgbaValues[rgbaValuesIndex] = palette[paletteIndex];
                rgbaValues[rgbaValuesIndex + 1] = palette[paletteIndex + 1];
                rgbaValues[rgbaValuesIndex + 2] = palette[paletteIndex + 2];
                rgbaValues[rgbaValuesIndex + 3] = palette[paletteIndex + 3];
            }
            ";  
        }


        public override string ToString()
        {
            return "Simple OpenCL Generator";
        }
    }
}
