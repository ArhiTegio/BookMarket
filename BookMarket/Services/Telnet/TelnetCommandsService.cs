using BookMarket.Services.Telnet.Commands;
using TelnetServer;
using System.Text;
using Entities.Telnet;

namespace BookMarket.Services.Telnet
{
    public class TelnetCommandsService: IDisposable
    {

        private TelnetService _TelnetService { get; set; }
        public TelnetCommandsService()
        {
            //Create Telnet service
            _TelnetService = new TelnetService(
                new TCPServer(), //Multi client TCP server
                new ITelnetCommand[]//Custom commands we implemented
            {
                new HelloCommand(),
                new EchoCommand(),
                new GetCommand(),
                new BuyCommand(),
                new RestockCommand(),
            });

            Start();
        }

        public void Start()
        {
            //Settings for the Telnet service
            TelnetServiceSettings _telnetSettings = new TelnetServiceSettings();
            _telnetSettings.PromtText = "SampleApp@" + Environment.MachineName;
            _telnetSettings.PortNumber = 32202;
            _telnetSettings.Charset = Encoding.Default.CodePage;

            //Start listening for incoming connections
            _TelnetService.Start(_telnetSettings);

            Console.WriteLine("Telnet BookMarket Service is running.");
        }

        public void Dispose()
        {
            //Stop service. Always stop the service when the application is shutting down.
            _TelnetService.Stop();
        }


    }
}
