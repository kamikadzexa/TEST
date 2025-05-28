using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using RJCP.IO.Ports;
using System;
using System.Text;

namespace TEST.Views
{
    public partial class MainWindow : Window
    {
        private SerialPortStream _serialPort;

        public MainWindow()
        {
            InitializeComponent();
            PopulatePorts();
        }

        private void PopulatePorts()
        {
            // Corrected: Create instance before calling GetPortNames()
            var portStream = new SerialPortStream();
            PortsComboBox.ItemsSource = portStream.GetPortNames(); // Corrected: Use ItemsSource instead of Items
            if (PortsComboBox.ItemsSource != null && PortsComboBox.ItemCount > 0)
                PortsComboBox.SelectedIndex = 0;
        }

        private async void OpenPort_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            if (_serialPort?.IsOpen == true)
            {
                ClosePort();
                button.Content = "Open Port";
            }
            else
            {
                if (PortsComboBox.SelectedItem == null) return;

                var portName = PortsComboBox.SelectedItem.ToString();
                _serialPort = new SerialPortStream
                {
                    PortName = portName,
                    BaudRate = 115200,
                    DataBits = 8,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                    ReadTimeout = 100
                };

                try
                {
                    _serialPort.Open();
                    _serialPort.DataReceived += SerialDataReceived;
                    button.Content = "Close Port";
                }
                catch (Exception ex)
                {
                    // Corrected: Use Avalonia's native dialog
                    await new MessageBox(this, "Error", ex.Message).ShowDialog(this);
                }
            }
        }

        private void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var buffer = new byte[_serialPort.BytesToRead];
            _serialPort.Read(buffer, 0, buffer.Length);
            var text = Encoding.ASCII.GetString(buffer);

            Dispatcher.UIThread.Post(() =>
            {
                OutputText.Text += text;
            });
        }

        private void ClosePort()
        {
            if (_serialPort == null) return;
            _serialPort.DataReceived -= SerialDataReceived;
            _serialPort.Close();
            _serialPort.Dispose();
            _serialPort = null;
        }
    }
}