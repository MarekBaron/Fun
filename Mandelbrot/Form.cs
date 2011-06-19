﻿using System;
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
    public partial class Form1 : Form
    {
        private CPUGenerator cpuGen = new CPUGenerator();

        private float _setStartX = -2.25f;
        private float _setStartY = -1;
        private float _setWidth = 3.5f;
        private bool _redrawing = false;
        private bool _needsAnotherRedraw = false;


        public Form1()
        {
            InitializeComponent();
            Redraw();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Redraw();
        }

        private void Redraw()
        {
            if (!_redrawing)
            {
                InternalRedraw();
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

            //var genTask = Task.Factory.StartNew(() =>
            //{
                Stopwatch sw = new Stopwatch();
                sw.Start();                
                byte[] rgbaValues = cpuGen.Generate(bitmap.Width, bitmap.Height, _setStartX, _setWidth, _setStartY, _setWidth * bitmap.Height / bitmap.Width);//_setHeight);
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
                    InternalRedraw();
                }
            //});

        }

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
