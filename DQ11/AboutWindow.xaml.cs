using System.Windows;
using System.Windows.Navigation;

namespace DQ11
{
	/// <summary>
	/// AboutWindow1.xaml の相互作用ロジック
	/// </summary>
	public partial class AboutWindow : Window
	{
		public AboutWindow()
		{
			InitializeComponent();
		}

		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
			e.Handled = true;
		}
	}
}
