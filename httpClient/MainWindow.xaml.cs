using HeyRed.Mime;
using httpClient.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
using Image = System.Windows.Controls.Image;
using Microsoft.Win32;
using System.CodeDom;

namespace httpClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BitmapImage image;
        private string mimeType;
        private string htmlContent;
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void Button_Click()
        {
            

        }

        private void BTNGuardarClick(object sender, RoutedEventArgs e)
        {
            if (mimeType.StartsWith("text/html"))
            {
                saveText();
            }
            if(mimeType.StartsWith("image/"))
            {
                saveImage();
            }


        }
        private void saveImage()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivos de imagen (*.bmp, *.gif, *.jpeg, *.png)|*.bmp;*.gif;*.jpeg;*.png|Todos los archivos (*.*)|*.*";
            var result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                var filePath = saveFileDialog.FileName;
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    encoder.Save(fileStream);
                }
            }
        }
        private void saveText()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
            var result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                var filePath = saveFileDialog.FileName;
                using (var file = new StreamWriter(filePath))
                {
                    file.WriteLine(htmlContent);
                }
            }
        }
        private static BitmapImage LoadImage(byte[] byteArray)
        {
            var bitmapImage = new BitmapImage();
            using (var mememoryStream = new MemoryStream(byteArray))
            {
                mememoryStream.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.UriSource = null;
                bitmapImage.StreamSource = mememoryStream;
                bitmapImage.EndInit();
            }
            bitmapImage.Freeze();
            return bitmapImage;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_url.Text))
            {
                this.bdr_indicador.BorderBrush = null;
                this.txt_headers.Text = "";
                this.txt_body.Text = "";
                this.lbl_statuscode.Content = "";

                try
                {
                    if (!txt_url.Text.ToLower().StartsWith("http") &&
                        !txt_url.Text.ToLower().StartsWith("https"))
                    {
                        txt_url.Text = "https://" + txt_url.Text;
                    }
                    this.bdr_indicador.BorderBrush = new SolidColorBrush(Colors.Blue);
                    this.lbl_statuscode.Content = "Procesando petición";

                    HttpResponseMessage response =
                        await ClienteBasicoHTTP.ejecutarPeticion(txt_url.Text, cbx_metodo.Text);
                    HttpStatusCode statusCode = response.StatusCode;
                    this.lbl_statuscode.Content = "" + (int)statusCode +
                        " - " + statusCode.ToString();
                    Stream contentBinary = response.Content.ReadAsStream();
                    if (response.IsSuccessStatusCode)
                    {
                        this.bdr_indicador.BorderBrush = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        this.bdr_indicador.BorderBrush = new SolidColorBrush(Colors.Red);
                    }


                    this.txt_headers.Text += " **** Generales y de respuesta ****\r\n\r\n";
                    this.txt_headers.Text += response.Headers.ToString();
                    this.txt_headers.Text += "\r\n";
                    this.txt_headers.Text += "**** De entidad ****\r\n\r\n";
                    this.txt_headers.Text += response.Content.Headers.ToString();
                    this.txt_headers.Text += "**** Content type ****\r\n\r\n";
                    this.lbl_tipo_contenido.Content = response.Content.Headers.ContentType.MediaType;
                    this.txt_headers.Text += response.Content.Headers.ContentType;


                    MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
                    mimeType = contentType.MediaType;
                    lbl_tipo_contenido.Content = mimeType;

                    if (mimeType.StartsWith("text/html"))
                    {
                        WebBrowser webBrowser = new WebBrowser();
                        htmlContent = await response.Content.ReadAsStringAsync();
                        webBrowser.NavigateToString(htmlContent);
                        sv_pretty.Content = webBrowser;
                    }
                    else
                    {
                        if (mimeType.StartsWith("image/"))
                        {
                            byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                            image = LoadImage(imageBytes);
                            Image imagen = new Image();
                            imagen.Source = image;
                            sv_pretty.Content = imagen;

                        }
                    }

                }
                catch (Exception ex)
                {
                    this.bdr_indicador.BorderBrush = new SolidColorBrush(Colors.Red);
                    this.lbl_statuscode.Content = ex.Message;
                }
            }
            else
            {
                MessageBox.Show("Debes de ingresar la url a consultar", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
