﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace KataWpf
{
    public partial class MainWindow : Window
    {
        CancellationTokenSource cts;
        TaskScheduler taskScheduler;

        public MainWindow()
        {
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
