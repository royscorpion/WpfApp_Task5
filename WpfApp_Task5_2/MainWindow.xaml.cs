using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp_Task5_2
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

        private void inkPen_Click(object sender, RoutedEventArgs e)
        {
            imageCanvas.EditingMode = InkCanvasEditingMode.Ink;
        }

        private void erase_Click(object sender, RoutedEventArgs e)
        {
            imageCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Графические файлы (*.png)|*.png|Все файлы (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                int margin = (int)imageCanvas.Margin.Left;
                //int width = (int)imageCanvas.ActualWidth + 2 * margin;
                //int height = (int)imageCanvas.ActualHeight + 2 * margin;
                Size size = new Size(imageCanvas.ActualWidth + margin * 2, imageCanvas.ActualHeight + margin * 2);
                imageCanvas.Measure(size);
                imageCanvas.Arrange(new Rect(size));
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Default);
                //RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);
                renderTargetBitmap.Render(imageCanvas);
                using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
                    pngBitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                    pngBitmapEncoder.Save(fileStream);
                }
                imageCanvas.UpdateLayout(); // не получилось возвратить окно в исходное состояние после сохранения,
                                            // помогает только изменение размера окна мышью.
            }
        }

        // ??? Как открыть файл изображения для редактирования ???
        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Графические файлы (*.png)|*.png|Все файлы (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open,FileAccess.ReadWrite))
                {
                    //PngBitmapDecoder pngBitmapDecoder = new PngBitmapDecoder(fileStream,BitmapCreateOptions.None,BitmapCacheOption.Default);
                    //pngBitmapDecoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                    
                    StrokeCollection strokeCollection = new StrokeCollection(fileStream);
                    imageCanvas.Strokes = strokeCollection;
                }
            }

        }

    }
}