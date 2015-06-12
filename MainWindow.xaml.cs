using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Koch_Mandelbrot__Sierpinski
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public WriteableBitmap bm;
        bool firstDot;
        int xA = 0, yA = 0, xB = 0, yB = 0;
        int imgHeight, imgWidth;
        int deepth = 0;
        int what = 0;
        
        public MainWindow()
        {
            InitializeComponent();
            initImage();
        }

        private void initImage()
        {
            imgHeight = (int)image.Height;
            imgWidth = (int)image.Width;
            //image = new Image();
            bm = new WriteableBitmap(imgWidth, imgHeight, 96, 96, PixelFormats.Bgra32, null);
            image.Source = bm;
        }

        private void drawPixel(int x1, int y1)
        {
            if (x1 < 1 || x1 > imgWidth || y1 < 1 || y1 > imgHeight)
            {
                return;
            }
            byte blue = 0;
            byte green = 0;
            byte red = 0;
            byte alpha = 255;
            byte[] colorData = { blue, green, red, alpha };
            Int32Rect rect = new Int32Rect(x1, y1, 1, 1);
            bm.WritePixels(rect, colorData, 4, 0);
        }

        private void Swap<T>(ref T lhs, ref T rhs) { T temp; temp = lhs; lhs = rhs; rhs = temp; }

        private void drawPixel(int x1, int y1, byte red)
        {
            if (x1 < 1 || x1 > imgWidth || y1 < 1 || y1 > imgHeight)
            {
                return;
            }
            byte blue = 150;
            byte green = 0;
            //byte red = 0;
            byte alpha = 255;
            byte[] colorData = { blue, green, red, alpha };
            Int32Rect rect = new Int32Rect(x1, y1, 1, 1);
            bm.WritePixels(rect, colorData, 4, 0);
        }

        private void drawPixelTransparent(int x1, int y1, double transparent)
        {
            if (x1 < 1 || x1 > imgWidth - 1 || y1 < 1 || y1 > imgHeight - 1)
            {
                return;
            }
            byte blue = 0;
            byte green = 0;
            byte red = 0;
            byte alpha = (byte)(255 * transparent);
            byte[] colorData = { blue, green, red, alpha };
            Int32Rect rect = new Int32Rect(x1, y1, 1, 1);
            bm.WritePixels(rect, colorData, 4, 0);
        }

        private void drawPixelWhite(int x1, int y1)
        {
            if (x1 < 1 || x1 > imgWidth - 1 || y1 < 1 || y1 > imgHeight - 1)
            {
                return;
            }
            byte blue = 255;
            byte green = 255;
            byte red = 255;
            byte alpha = (byte)(255);
            byte[] colorData = { blue, green, red, alpha };
            Int32Rect rect = new Int32Rect(x1, y1, 1, 1);
            bm.WritePixels(rect, colorData, 4, 0);
        }
        
        private void algorithmDDA(int x1, int y1, int x2, int y2)
        {
            double l = Math.Max(Math.Abs(x2 - x1), Math.Abs(y2 - y1)),
                xStart = x1,
                yStart = y1,
                deltaX = (x2 - x1) / l,
                deltaY = (y2 - y1) / l;

            for (int i = 0; i < (int)l + 1; i++)
            {
                drawPixel((int)xStart, (int)yStart);
                xStart += deltaX;
                yStart += deltaY;
            }

        }

        private void algorithmDDAwhite(int x1, int y1, int x2, int y2)
        {
            double l = Math.Max(Math.Abs(x2 - x1), Math.Abs(y2 - y1)),
                xStart = x1,
                yStart = y1,
                deltaX = (x2 - x1) / l,
                deltaY = (y2 - y1) / l;

            for (int i = 0; i < (int)l + 1; i++)
            {
                drawPixelWhite((int)xStart, (int)yStart);
                xStart += deltaX;
                yStart += deltaY;
            }

        }
        
        private void image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(image);
            drawPixel((int)p.X, (int)p.Y);
            if (firstDot)
            {
                xB = (int)p.X;
                yB = (int)p.Y;
            }
            else
            {
                xA = (int)p.X;
                yA = (int)p.Y;
            }

            firstDot = !firstDot;
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            clear();
            what = 0;
        }

        private void clear()
        {
            clearOnly();
            buttonDeeper.Content = deepth = 0;
        }

        private void clearOnly()
        {
            image.Source = null;
            bm = new WriteableBitmap(imgWidth, imgHeight, 96, 96, PixelFormats.Bgra32, null);
            image.Source = bm;
        }

        private void buttonSierpinski_Click(object sender, RoutedEventArgs e)
        {
            clear();
            what = 1;
            buttonDeeper.Content = deepth = 1;
            Carpet(imgWidth / 3, imgWidth / 3, imgWidth / 3, 1);
        }

        public void Carpet(double coordinateX, double coordinateY, double width, int n)
        {
            if (n > deepth)
            {
                return;
            }
            rectangle(coordinateX, coordinateY, Math.Pow((1.0 / 3), n) * imgWidth);
            n++;

            double newWidth = Math.Pow((1.0 / 3), n) * imgWidth;
            Carpet(coordinateX - 2 * newWidth,
                coordinateY - 2 * newWidth,
                newWidth, n);
            Carpet(coordinateX + newWidth,
                coordinateY - 2 * newWidth,
                newWidth, n);
            Carpet(coordinateX + 4 * newWidth,
                coordinateY - 2 * newWidth,
                newWidth, n);
            Carpet(coordinateX - 2 * newWidth,
                coordinateY + newWidth,
                newWidth, n);
            Carpet(coordinateX + 4 * newWidth,
                coordinateY + newWidth,
                newWidth, n);
            Carpet(coordinateX - 2 * newWidth,
                coordinateY + 4 * newWidth,
                newWidth, n);
            Carpet(coordinateX + newWidth,
                coordinateY + 4 * newWidth,
                newWidth, n);
            Carpet(coordinateX + 4 * newWidth,
                coordinateY + 4 * newWidth,
                newWidth, n);
        }

        public void rectangle(double coordinateX, double coordinateY, double width)
        {
            int square = (int)width * (int)width;
            byte[] colorData = new byte[square * 4];
            for (int i = 0; i < square; i++)
            {
                colorData[i * 4] = 204;
                colorData[i * 4 + 1] = 0;
                colorData[i * 4 + 2] = 255;
                colorData[i * 4 + 3] = 255;
            }
            Int32Rect rect = new Int32Rect((int)coordinateX, (int)coordinateY, (int)width, (int)width);
            bm.WritePixels(rect, colorData, (int)width * 4, 0);
        }

        private void buttonMandelbrot_Click(object sender, RoutedEventArgs e)
        {
            clear();
            what = 2;
            buttonDeeper.Content = deepth = 1;
            cloud(1);
        }
        
        private void cloud(int n)
        {
            double minr = -2,
                mini = -2,
                maxr = 2,
                maxi = 2;

            double zx = 0;
            double zy = 0;
            double cx = 0;
            double cy = 0;
            double xjump = ((maxr - minr) / Convert.ToDouble(imgWidth));
            double yjump = ((maxi - mini) / Convert.ToDouble(imgHeight));
            double tempzx = 0;
            int loopmax = (int)Math.Pow(2, n);
            int loopgo = 0;
            for (int x = 0; x < imgWidth; x++)
            {
                cx = (xjump * x) - Math.Abs(minr);
                for (int y = 0; y < imgHeight; y++)
                {
                    zx = 0;
                    zy = 0;
                    cy = (yjump * y) - Math.Abs(mini);
                    loopgo = 0;
                    while (zx * zx + zy * zy <= 4 && loopgo < loopmax)
                    {
                        loopgo++;
                        tempzx = zx;
                        zx = (zx * zx) - (zy * zy) + cx;
                        zy = (2 * tempzx * zy) + cy;
                    }
                    if (loopgo != loopmax)
                        drawPixel(x, y, (byte)(255 - 255 / loopgo));
                    else
                        drawPixel(x, y, 0);

                }
            }
        }

        private void buttonKoch_Click(object sender, RoutedEventArgs e)
        {
            clear();
            what = 3;
            buttonDeeper.Content = deepth = 1;
            starStart();
        }

        private void starStart()
        {
            algorithmDDA(xA, yA, xB, yB);
        }

        private void star1(int x1, int y1, int x2, int y2, int n)
        {
            if (n > deepth || x2 - x1 == 0)
            {
                return;
            }

            double k = ((double)(y2 - y1) / (double)(x2 - x1));
            
            int leftX = (int)(x2 - x1) / 3 + x1,
                leftY = (int)(y2 - y1) / 3 + y1,
                rightX = (int)(x2 - x1) * 2 / 3 + x1,
                rightY = (int)(y2 - y1) * 2 / 3 + y1;

            algorithmDDAwhite(x1, y1, x2, y2);
            //algorithmDDAwhite((int)(x2 - x1) / 3 + x1, (int)(y2 - y1) / 3 + y1, 
                //(int)(x2 - x1) * 2 / 3 + x1, (int)(y2 - y1) * 2 / 3 + y1);
            algorithmDDA(x1, y1, leftX, leftY);
            algorithmDDA(rightX, rightY, x2, y2);

            //buttonDeeper.Content = Math.Atan(k).ToString();
            double kLeft = Math.Tan(2.094 - Math.Atan(k)),
                kRight = Math.Tan(1.047 - Math.Atan(k)),
                bLeft = leftY - kLeft * leftX,
                bRight = rightY - kRight * rightX,
                targetX = (bRight - bLeft) / (kLeft - kRight),
                targetY = kLeft * leftX + bLeft;

            algorithmDDA(leftX, leftY, (int)targetX, (int)targetY);
            algorithmDDA((int)targetX, (int)targetY, rightX, rightY);
        }

        private void star(int order, int x1, int y1, int x2, int y2)
        {

            if (order == 0)
            {
                algorithmDDA(x1, y1, x2, y2);
            }
            else
            {
                double alpha = Math.Atan2(y2 - y1, x2 - x1);
                double R = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
                double Xa = x1 + R * Math.Cos(alpha) / 3,
                Ya = y1 + R * Math.Sin(alpha) / 3;
                double Xc = Xa + R * Math.Cos(alpha - Math.PI / 3) / 3,
                Yc = Ya + R * Math.Sin(alpha - Math.PI / 3) / 3;
                double Xb = x1 + 2 * R * Math.Cos(alpha) / 3,
                Yb = y1 + 2 * R * Math.Sin(alpha) / 3;
                star(order - 1, x1, y1, (int)Xa, (int)Ya);
                star(order - 1, (int)Xa, (int)Ya, (int)Xc, (int)Yc);
                star(order - 1, (int)Xc, (int)Yc, (int)Xb, (int)Yb);
                star(order - 1, (int)Xb, (int)Yb, x2, y2);
            }
        }

        private void buttonDeeper_Click(object sender, RoutedEventArgs e)
        {
            
            switch (what)
            {
                case 1:
                    buttonDeeper.Content = ++deepth;
                    Carpet(imgWidth / 3, imgWidth / 3, imgWidth / 3, 1);
                    break;
                case 2:
                    buttonDeeper.Content = ++deepth;
                    cloud(deepth);
                    break;
                case 3:
                    clearOnly();
                    buttonDeeper.Content = ++deepth;
                    star(deepth, xA, yA, xB, yB);
                    break;
                default:
                    break;
            }
        }

    }
}
