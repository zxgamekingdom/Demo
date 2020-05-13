using Demo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Demo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private uint _count;
        private bool _isLoading;
        private IHttpClientFactory _httpClientFactory;

        public MainWindow()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient();
            ServiceProvider provider = serviceCollection.BuildServiceProvider();
            _httpClientFactory = provider.GetService<IHttpClientFactory>();
            InitializeComponent();
            LoadImages();
        }

        private async Task CloseWait()
        {
            _isLoading = false;
            await Invoke(() => WaitLabel.Visibility = Visibility.Collapsed);
        }

        private string GetApiUri()
        {
            return
                $@"https://www.1miba.com/api/model/getList?type=0&cid=0&style=0&page={
                    _count}&sort=1&rendering=0";
        }

        private async Task Invoke(Action action)
        {
            await Dispatcher.InvokeAsync(action);
        }

        private Task LoadImages()
        {
            return Task.Run(async () =>
                {
                    _ = ShowWait();
                    string requestUri = GetApiUri();
                    string stringAsync = await _httpClientFactory.CreateClient()
                        .GetStringAsync(requestUri);
                    _count++;
                    var root = stringAsync.JsonDeserialize<Root>();
                    double offset = default;
                    _ = Invoke(() => offset = ScrollViewer.VerticalOffset);
                    // foreach (string s in root.Data.Select(data => data.Thumb))
                    // {
                    //     _ = Invoke(() =>
                    //     {
                    //         ImageWrapPanel.Children.Add(new Image()
                    //         {
                    //             Source = new BitmapImage(new Uri(s, UriKind.Absolute))
                    //         });
                    //     });
                    // }
                    IEnumerable<Task<Stream>> tasks = root.Data.Select(
                        async data =>
                        {
                            HttpClient httpClient = _httpClientFactory.CreateClient();
                            Stream streamAsync =
                                await httpClient.GetStreamAsync(data.Thumb);
                            return streamAsync;
                        });
                    Stream[] streams = await Task.WhenAll(tasks);
                    MessageBox.Show("死锁");
                    foreach (Stream stream in streams)
                    {
                        _ = Invoke(() =>
                        {
                            var bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = stream;
                            bitmapImage.EndInit();
                            ImageWrapPanel.Children.Add(new Image()
                            {
                                Source = bitmapImage
                            });
                        });
                    }

                    _ = Invoke(() => ScrollViewer.ScrollToVerticalOffset(offset));
                    _ = CloseWait();
                })
                .ContinueWith(task =>
                {
                    if (task.Exception != null)
                        throw task.Exception;
                });
        }

        private void ScrollViewer_OnScrollChanged(object sender,
            ScrollChangedEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                //到底了
                if (_isLoading == false
                    && Math.Abs(e.ExtentHeight - (e.VerticalOffset + e.ViewportHeight))
                    < 1)
                    LoadImages();
            }
            else
            {
                throw new ArgumentException($@"{nameof(sender)}类型不正确");
            }
        }

        private async Task ShowWait()
        {
            _isLoading = true;
            await Invoke(() => WaitLabel.Visibility = Visibility.Visible);
        }
    }
}