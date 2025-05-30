using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace YSCertificateCheck
{
    /// <summary>
    /// Interaction logic for CertificateCheckControl.xaml
    /// </summary>
    public partial class CertificateCheckControl : UserControl
    {
        public CertificateCheckControl()
        {
            InitializeComponent();
        }

        private async void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            CheckButton.IsEnabled = false;
            CheckResult.Text = "Wait...";
            var cursor = CheckResult.Cursor;
            CheckResult.Cursor = Cursors.Wait;

            try
            {
                var result = await CertificateCheckService.Check(URL.Text).WaitAsync(TimeSpan.FromSeconds(30));
                if (result != null)
                {
                    CheckResult.Text = result;
                }
            }
            catch (TimeoutException)
            {
                CheckResult.Text = "The timeout occurred.";
            }
            catch (Exception ex)
            {
                CheckResult.Text = ex.ToString();
            }

            CheckResult.Cursor = cursor;
            CheckButton.IsEnabled = true;
        }
    }
}
