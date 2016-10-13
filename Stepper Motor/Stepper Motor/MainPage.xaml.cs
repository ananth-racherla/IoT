using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using ppatierno.AzureSBLite.Messaging;

namespace Stepper_Motor
{
    public sealed partial class MainPage
    {
        private readonly Uln2003Driver _uln2003Driver;
        static string eventHubName = "";
        static string connectionString = "";

        public MainPage()
        {
            InitializeComponent();

            _uln2003Driver = new Uln2003Driver(26, 13, 6, 5);

            Loaded += MainPage_Loaded;
            Unloaded += MainPage_Unloaded;
        }
        
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>
            {
                var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);
                for (var i = 0; i < 10; i++)
                {
                    eventHubClient.Send(new EventData(Encoding.UTF8.GetBytes($"Turning left 360 degrees. Try {i+1}")));
                    await _uln2003Driver.TurnAsync(360, TurnDirection.Left);
                    eventHubClient.Send(new EventData(Encoding.UTF8.GetBytes($"Turning right 360 degrees. Try {i + 1}")));
                    await _uln2003Driver.TurnAsync(360, TurnDirection.Right);
                }
            });
        }

        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            _uln2003Driver?.Dispose();
        }
    }
}
