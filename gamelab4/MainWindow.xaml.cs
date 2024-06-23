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

namespace gamelab4
{
    public partial class MainWindow : Window
    {

        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            this.KeyDown += Window_KeyDown;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CreateRandomPoints(20);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) // или любая другая клавиша
            {
                Application.Current.Shutdown(); // закрытие приложения
            }
        }

        private void CreateRandomPoints(int count)
        {
            for (int i = 0; i < count; i++)
            {
                CreateRandomPoint();
            }
        }

        private List<Point> points = new List<Point>();
        private const double minDistance = 70;

        private void CreateRandomPoint()
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = 30;
            ellipse.Height = 30;
            ellipse.Fill = Brushes.Black;

            // Генерируем случайные координаты для точки
            double x, y;
            do
            {
                x = random.Next(30, (int)canvas.ActualWidth - 100); // отступ от левого и правого края
                y = random.Next(30, (int)canvas.ActualHeight - 100); // отступ от верхнего и нижнего края
            } while (IsTooClose(x, y));

            ellipse.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;

            Canvas.SetLeft(ellipse, x);
            Canvas.SetTop(ellipse, y);

            canvas.Children.Add(ellipse);
            points.Add(new Point(x, y));
        }

        private bool IsTooClose(double x, double y)
        {
            foreach (var point in points)
            {
                if (Math.Sqrt(Math.Pow(point.X - x, 2) + Math.Pow(point.Y - y, 2)) < minDistance)
                {
                    return true;
                }
            }
            return false;
        }
        private int clickcount = 0;
        private int loosered = 2;
        private int looseblue = 2;

        private class Line
        {
            public Point StartPoint { get; set; }
            public Point EndPoint { get; set; }
        }

        private List<Line> lines = new List<Line>();

        private Point startPoint; // переменные для хранения координат первой выбранной точки
        private Point endPoint; // переменные для хранения координат первой выбранной точки

        private Ellipse lastRedEllipse1;
        private Ellipse lastRedEllipse2;

        private Ellipse lastBLueEllipse1;
        private Ellipse lastBlueEllipse2;

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            if (ellipse.Fill != Brushes.Red && ellipse.Fill != Brushes.Blue)
            {
                clickcount++;
            }
            if (ellipse != null && clickcount == 1)
            {
                if (ellipse.Fill != Brushes.Red && ellipse.Fill != Brushes.Blue)
                {
                    ellipse.Fill = Brushes.Red; // Изменяем цвет на красный при нажатии
                }
                lastRedEllipse1 = ellipse;

                startPoint = new Point(Canvas.GetLeft(ellipse) + ellipse.Width / 2, Canvas.GetTop(ellipse) + ellipse.Height / 2); // сохраняем координаты первой точки
            }
            else if (ellipse != null && clickcount == 2)
            {
                if (ellipse.Fill != Brushes.Red && ellipse.Fill != Brushes.Blue)
                {
                    ellipse.Fill = Brushes.Red; // Изменяем цвет на красный при нажатии
                }
                lastRedEllipse2 = ellipse;

                endPoint = new Point(Canvas.GetLeft(ellipse) + ellipse.Width / 2, Canvas.GetTop(ellipse) + ellipse.Height / 2); // координаты второй точки
                Line line = new Line { StartPoint = startPoint, EndPoint = endPoint }; // создаем новую линию
          
                if (CheckIntersection(line) == false)
                {
                    DrawLine(line); // рисуем линию на Canvas
                    lines.Add(line); // добавляем линию в список
                }
                else
                {
                    lastRedEllipse1.Fill = Brushes.Black;
                    lastRedEllipse2.Fill = Brushes.Black;
                    clickcount = clickcount - 2;
                    loosered--;
                }
            }
            if (ellipse != null && clickcount == 3)
            {
                if (ellipse.Fill != Brushes.Red && ellipse.Fill != Brushes.Blue)
                {
                    ellipse.Fill = Brushes.Blue; // Изменяем цвет на красный при нажатии
                }
                lastBLueEllipse1 = ellipse;

                startPoint = new Point(Canvas.GetLeft(ellipse) + ellipse.Width / 2, Canvas.GetTop(ellipse) + ellipse.Height / 2); // сохраняем координаты первой точки
            }
            else if (ellipse != null && clickcount == 4)
            {
                if (ellipse.Fill != Brushes.Red && ellipse.Fill != Brushes.Blue)
                {
                    ellipse.Fill = Brushes.Blue; // Изменяем цвет на красный при нажатии
                }
                lastBlueEllipse2 = ellipse;


                Point endPoint = new Point(Canvas.GetLeft(ellipse) + ellipse.Width / 2, Canvas.GetTop(ellipse) + ellipse.Height / 2); // координаты второй точки
                Line line = new Line { StartPoint = startPoint, EndPoint = endPoint }; // создаем новую линию
                
                if (CheckIntersection(line) == false)
                {
                    DrawLine(line); // рисуем линию на Canvas
                    lines.Add(line); // добавляем линию в список
                }
                else
                {
                    lastBLueEllipse1.Fill = Brushes.Black;
                    lastBlueEllipse2.Fill = Brushes.Black;
                    clickcount = clickcount - 2;
                    looseblue--;
                }
            }
            if (clickcount > 3)
            {
                clickcount = 0;
            }
            if (loosered == 0 || looseblue == 0)
            {
                end();
            }
            endnone();
        }

        private void DrawLine(Line line)
        {
            System.Windows.Shapes.Line newLine = new System.Windows.Shapes.Line();
            newLine.Stroke = Brushes.Black;
            newLine.StrokeThickness = 5;
            newLine.X1 = line.StartPoint.X;
            newLine.Y1 = line.StartPoint.Y;
            newLine.X2 = line.EndPoint.X;
            newLine.Y2 = line.EndPoint.Y;
            canvas.Children.Add(newLine);
        }

        private bool CheckIntersection(Line line)
        {
            foreach (var existingLine in lines)
            {
                if (DoIntersect(line.StartPoint, line.EndPoint, existingLine.StartPoint, existingLine.EndPoint))
                {
                    return true; // Если найдено пересечение, вернуть true
                }
            }
            return false;
        }

        private bool DoIntersect(Point p1, Point q1, Point p2, Point q2)
        {
            int o1 = Orientation(p1, q1, p2);
            int o2 = Orientation(p1, q1, q2);
            int o3 = Orientation(p2, q2, p1);
            int o4 = Orientation(p2, q2, q1);

            if (o1 != o2 && o3 != o4)
                return true;

            return false;
        }

        private int Orientation(Point p, Point q, Point r)
        {
            double val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);

            if (val == 0) return 0;
            return (val > 0) ? 1 : 2;
        }

        private void end()
        {
            string winner = "";
            if (loosered == 0)
            {
                winner = "Победил синий игрок!";
            }
            else
            {
                winner = "Победил красный игрок!";
            }

            MessageBox.Show(winner, "Победа", MessageBoxButton.OK, MessageBoxImage.Information);
            Application.Current.Shutdown();
        }

        private void endnone()
        {
            if (canvas.Children.OfType<Ellipse>().All(x => x.Fill != Brushes.Black))
            {
                MessageBox.Show("Ничья!", "Ничья", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Shutdown();
            }
        }
    }
}
