using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataGrid2DLibrary;
using Mommo;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        int[,] matrix;
        int max = 0;
        bool check = false;

        public void Button1_Click(object sender, RoutedEventArgs e)
        {
            string a = textBox1.Text; //вводимое предложение
            byte[] b = Encoding.Default.GetBytes(a); //перевод предложение в байтовый массив
            matrix = new int[b.Length + 1, 8 + 1];//объявление матрицы
            int p = 0;//проверочный бит строк
            int q = 0;//проверочный бит столбцов
            max = b.Length;
            //Перевод символов из десятичного представления в двоичное и заполнение матрицы
            int[] array = new int[8];
            for (int i = 0; i < b.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    array[j] = b[i] % 2;
                    b[i] /= 2;
                }
                Array.Reverse(array);
                for (int j = 0; j < 8; j++)
                {
                    matrix[i, j] = array[j];
                }
            }

            //подсчет проверочных бит строк
            for (int i = 0; i < b.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    p += matrix[i, j];
                }
                matrix[i, 8] = p % 2;
                p = 0;
            }

            //подсчет проверочных бит столбцов
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < b.Length; i++)
                {
                    q += matrix[i, j];
                }
                matrix[b.Length, j] = q % 2;
                q = 0;
            }

            label5.Content = "Выберите координаты" + "\n" + "искаженного бита";

            for (int i =0; i < b.Length;i++)
            {
                ComboBox1.Items.Add(i+1);
            }
            for (int i = 0; i < 8; i++)
            {
                ComboBox2.Items.Add(i+1);
            }
            
            Matrix[] shit = new Matrix[b.Length+1];

            for (int i = 0; i < b.Length+1; i++)
            {
                int j = 0;
                shit[i] = new Matrix
                {
                    One = matrix[i, j],
                    Two = matrix[i, j + 1],
                    Three = matrix[i, j + 2],
                    Four = matrix[i, j + 3],
                    Five = matrix[i, j + 4],
                    Six = matrix[i, j + 5],
                    Seven = matrix[i, j + 6],
                    Eight = matrix[i, j + 7],
                    ProvBit = matrix[i, j + 8]
                };
                if(i<b.Length)
                    Matrix1.Items.Add(shit[i]);
                Matrix2.Items.Add(shit[i]);
                
            }

        }
        //вывод инвертированной матрицы
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Matrix[] shit = new Matrix[max+1];
            if (matrix[ComboBox1.SelectedIndex, ComboBox2.SelectedIndex] == 0)
                matrix[ComboBox1.SelectedIndex, ComboBox2.SelectedIndex] = 1;
            else
                matrix[ComboBox1.SelectedIndex, ComboBox2.SelectedIndex] = 0;
            for (int i = 0; i < max+1; i++)
            {                
                int j = 0;                
                shit[i] = new Matrix
                {
                    One = matrix[i, j],
                    Two = matrix[i, j + 1],
                    Three = matrix[i, j + 2],
                    Four = matrix[i, j + 3],
                    Five = matrix[i, j + 4],
                    Six = matrix[i, j + 5],
                    Seven = matrix[i, j + 6],
                    Eight = matrix[i, j + 7],
                    ProvBit = matrix[i, j + 8]
                };                
                Matrix3.Items.Add(shit[i]);
            }            
        }
        //подсветка инвертированной и обычной ячеек
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            if (check == false)
            {
                DataGridCell NMcell = GetCellColor(ComboBox1.SelectedIndex, ComboBox2.SelectedIndex, Matrix2);
                DataGridCell WMcell = GetCellColor(ComboBox1.SelectedIndex, ComboBox2.SelectedIndex, Matrix3);
                NMcell.Background = new SolidColorBrush(Colors.Red);
                WMcell.Background = new SolidColorBrush(Colors.Red);
                check = true;
            }
                
        }

        //очистка
        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            Matrix1.Items.Clear();
            Matrix2.Items.Clear();
            Matrix3.Items.Clear();
            textBox1.Clear();
            ComboBox1.Items.Clear();
            ComboBox2.Items.Clear();
            check = false;
        }

        public class Matrix
        {
            public int One { get; set;}
            public int Two { get; set; }
            public int Three { get; set; }
            public int Four { get; set; }
            public int Five { get; set; }
            public int Six { get; set; }
            public int Seven { get; set; }
            public int Eight { get; set; }
            public int ProvBit { get; set; }
        }
        //изменение цвета ячейки
        public DataGridCell GetCellColor(int rowindex, int columnindex, DataGrid dg)
        {
            DataGridRow row = dg.ItemContainerGenerator.ContainerFromIndex(rowindex) as DataGridRow;
            DataGridCellsPresenter p = GetVisualChild<DataGridCellsPresenter>(row);
            DataGridCell cell = p.ItemContainerGenerator.ContainerFromIndex(columnindex) as DataGridCell;
            return cell;
        }
                
        static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }
}
