using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FernNamespace
{
    /*
     * this class draws a fractal fern when the constructor is called.
     */
    class Fern
    {
        private static int BERRYMIN = 2;
        private static int TENDRILMIN = 1;
        private static double DELTATHETA = 0.1;
        private static double SEGLENGTH = 7.0;
        private static Random random = new Random();

        /* 
         * Fern constructor erases screen and draws a fern
         * 
         * Size: number of 1-pixel segments of tendrils
         * Redux: how much smaller children clusters are compared to parents
         * Turnbias: how likely to turn right vs. left (0=always left, 0.5 = 50/50, 1.0 = always right)
         * canvas: the canvas that the fern will be drawn on
         */
        public Fern(double size, double redux, double turnbias, Canvas canvas)
        {
            // delete old canvas contents
            canvas.Children.Clear();                             
            
            // draw a new fern with the given parameters starting from the stem
            tendril((int)(canvas.Width / 2), (int)(canvas.Height/2), size/1.5, redux, turnbias, 3, canvas);
        }

        /*
         * cluster draws a cluster at the given location and then draws a bunch of tendrils out in 
         * regularly-spaced directions out of the cluster.
         */
        private void cluster(int x, int y, double size, double redux, double turnbias, double direction, Canvas canvas)
        {
            // computes the outgoing tendril's angle
            double theta = (30 * random.NextDouble()/180) - 240 / 180;

            // draws the main/original tendril line either clockwise or counter-clockwise depending on the turn bias
            tendril(x, y, size, redux, turnbias, direction, canvas);

            // draws the tendrils branching out to the right from the original stem
            tendril(x, y, size/1.25, redux, turnbias, direction + theta, canvas);
        }

        /*
         * tendril draws a tendril (a randomly-wavy line) in the given direction, for the given length, 
         * and draws a cluster at the other end if the line is big enough.
         */
        private void tendril(int x1, int y1, double size, double redux, double turnbias, double direction, Canvas canvas)
        {
            int x2 = x1, y2 = y1;
            // makes the stem/line look smooth as it moves around
            double directions = (random.NextDouble() < turnbias) ? -1.5 : 1.5;

            for (int i = 0; i < size; i++)
            {
                // sets the direction of the lines
                direction += directions * DELTATHETA;

                x1 = x2; y1 = y2;
                x2 = x1 + (int)(SEGLENGTH * Math.Sin(direction));
                y2 = y1 + (int)(SEGLENGTH * Math.Cos(direction));

                // Selects a random r,g,b value for the line/stem
                int red = random.Next(0, 255);
                int green = random.Next(0, 255);
                int blue = random.Next(0, 255);

                line(x1, y1, x2, y2, (byte)red, (byte)green, (byte)blue, 2 + size / 80, canvas);
            }

            if (size > TENDRILMIN)
                cluster(x2, y2, size / redux, redux, turnbias, direction, canvas);
            if (size < BERRYMIN)
                berry(x2, y2, 3, canvas);
        }

        /*
         * draw a red circle centered at (x,y), radius radius, with a black edge, onto canvas
         */
        private void berry(int x, int y, double radius, Canvas canvas)
        {
            // Selects a random r,g,b value for the berry
            int red = random.Next(0, 255);
            int green = random.Next(0,255);
            int blue = random.Next(0, 255);

            Ellipse myEllipse = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(255, (byte)red, (byte)green, (byte)blue);
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.StrokeThickness = 1;
            myEllipse.Stroke = Brushes.Olive;
            myEllipse.HorizontalAlignment = HorizontalAlignment.Center;
            myEllipse.VerticalAlignment = VerticalAlignment.Center;
            myEllipse.Width = 2 * radius;
            myEllipse.Height = 2 * radius;
            myEllipse.SetCenter(x, y);
            canvas.Children.Add(myEllipse);
        }

        /*
         * draw a line segment (x1,y1) to (x2,y2) with given color, thickness on canvas
         */
        private void line(int x1, int y1, int x2, int y2, byte r, byte g, byte b, double thickness, Canvas canvas)
        {
            Line myLine = new Line();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(255, r, g, b);
            myLine.X1 = x1;
            myLine.Y1 = y1;
            myLine.X2 = x2;
            myLine.Y2 = y2;
            myLine.Stroke = mySolidColorBrush;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.StrokeThickness = thickness;
            canvas.Children.Add(myLine);
        }
    }
}

/*
 * this class is needed to enable us to set the center for an ellipse (not built in?!)
 */
public static class EllipseX
{
    public static void SetCenter(this Ellipse ellipse, double X, double Y)
    {
        Canvas.SetTop(ellipse, Y - ellipse.Height / 2);
        Canvas.SetLeft(ellipse, X - ellipse.Width / 2);
    }
}

