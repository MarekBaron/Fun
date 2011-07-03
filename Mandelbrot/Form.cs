using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Mandelbrot
{
    internal enum Channel
    {
        B = 0,
        G = 1,
        R = 2,
        A = 3
    }

    public partial class Form1 : Form
    {
        private CPUGenerator cpuGen = new CPUGenerator();

        private float _setStartX = -2.25f;
        private float _setStartY = -1;
        private float _setWidth = 3.5f;
        private bool _redrawing = false;
        private bool _needsAnotherRedraw = false;
        private byte[] _palette = null;


        public Form1()
        {
            _palette = CreatePalette();
            InitializeComponent();
            Redraw();
        }


        #region Palette
        private byte[] CreatePalette()
        {
            byte[] p = new byte[256 * 4];

            FillChannelLinear(p, 0, 255, 255, 255, Channel.A);

            FillChannelLinear(p, 0, 200, 100, 255, Channel.R);
            FillChannelLinear(p, 0, 0, 100, 128, Channel.G);
            FillChannelLinear(p, 0, 100, 100, 0, Channel.B);

            FillChannelLinear(p, 100, 255, 200, 128, Channel.R);
            FillChannelLinear(p, 100, 128, 200, 0, Channel.G);
            FillChannelLinear(p, 100, 0, 200, 100, Channel.B);

            FillChannelLinear(p, 200, 128, 255, 0, Channel.R);
            FillChannelLinear(p, 200, 0, 255, 0, Channel.G);
            FillChannelLinear(p, 200, 100, 255, 0, Channel.B);
            return p;
        }

        private void FillChannelLinear(byte[] aPalette, int aStartIndex, byte aStartValue, int anEndIndex, byte anEndValue, Channel aChannelOffset)
        {
            float deltaValue = (anEndValue - aStartValue) / (float)(anEndIndex - aStartIndex);
            for (int index = aStartIndex; index <= anEndIndex; index++)
            {
                aPalette[index * 4 + (int)aChannelOffset] = (byte)(aStartValue + deltaValue * (index - aStartIndex));
            }
        } 
        #endregion

        private void Form1_Resize(object sender, EventArgs e)
        {          
            Redraw();
        }

        private PointF ScreenToSpace(int aScreenX, int aScreenY)
        {
            float coef = _setWidth / (float)pictureBox1.Width;
            return new PointF(_setStartX + coef * aScreenX, _setStartY + coef * aScreenY);
        }

        private float SetWidthToSetHeight(float aSetWidth)
        {
            return aSetWidth * (float)pictureBox1.Height / (float)pictureBox1.Width;
        }

        void Form1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var mouseInSpace = ScreenToSpace(e.X - pictureBox1.Location.X, e.Y - pictureBox1.Location.Y);

            var oldWidth = _setWidth;
            var oldHeight = SetWidthToSetHeight(oldWidth);
            var oldCenter = new PointF(_setStartX + oldWidth / 2f, _setStartY + oldHeight / 2f);

            if (e.Delta < 0)
                _setWidth *= 1.1f;            
            else            
                _setWidth *= 0.9f;
                       
            float setHeight = SetWidthToSetHeight(_setWidth);

            _setStartX = mouseInSpace.X - ((mouseInSpace.X - _setStartX) * _setWidth / oldWidth);
            _setStartY = mouseInSpace.Y - ((mouseInSpace.Y - _setStartY) * setHeight / oldHeight);

            Redraw();
        }

        private void Redraw()
        {
            if (!_redrawing)
            {         
                InvokeInternalRedraw();             
            }
            else
            {                
                _needsAnotherRedraw = true;
            }
        }

        private void InternalRedraw()
        {
            _redrawing = true;            
            var bitmap = (Bitmap)pictureBox1.Image;
            if (bitmap == null || bitmap.Width != pictureBox1.Width || bitmap.Height != pictureBox1.Height)
            {
                bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Image = bitmap;
            }

            var genTask = Task.Factory.StartNew(() =>
            {                
                Stopwatch sw = new Stopwatch();
                sw.Start();                
                byte[] rgbaValues = cpuGen.Generate(bitmap.Width, bitmap.Height, _setStartX, _setWidth, _setStartY, SetWidthToSetHeight(_setWidth), _palette);
                sw.Stop();
                RefreshElapsedTime(sw.ElapsedMilliseconds);

                Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                BitmapData bmpData =
                    bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly,
                    bitmap.PixelFormat);

                IntPtr ptr = bmpData.Scan0;
                System.Runtime.InteropServices.Marshal.Copy(rgbaValues, 0, ptr, rgbaValues.Length);
                // Unlock the bits.
                bitmap.UnlockBits(bmpData);
                pictureBox1.Invalidate();
                _redrawing = false;
                if (_needsAnotherRedraw)
                {                    
                    _needsAnotherRedraw = false;
                    InvokeInternalRedraw();
                }
                
            });

        }

        #region InvokeRedraw
        private delegate void InvokeInternalRedrawDelegate();

        private void InvokeInternalRedraw()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new InvokeInternalRedrawDelegate(InvokeInternalRedraw));
            }
            else
            {
                InternalRedraw();
            }
        }  
        #endregion

        #region WriteText
        //private delegate void WriteTextDelegate(string aText);

        //private void WriteText(string aText)
        //{
        //    if (tbConsole.InvokeRequired)
        //    {
        //        tbConsole.Invoke(new WriteTextDelegate(WriteText), aText);
        //    }
        //    else
        //    {
        //        var newLines = tbConsole.Lines.ToList();
        //        newLines.Add(aText);
        //        tbConsole.Lines = newLines.ToArray();
        //    }
        //} 
        #endregion

        #region GenerationTime label
        private delegate void RefreshElapsedTimeDelegate(long anElapsedMiliseconds);

        private void RefreshElapsedTime(long anElapsedMiliseconds)
        {
            if (lblGenerationTime.InvokeRequired)
            {
                lblGenerationTime.Invoke(new RefreshElapsedTimeDelegate(RefreshElapsedTime), anElapsedMiliseconds);
            }
            else
            {
                lblGenerationTime.Text = String.Format("{0} ms", anElapsedMiliseconds);
            }
        }
        #endregion

    }
}
