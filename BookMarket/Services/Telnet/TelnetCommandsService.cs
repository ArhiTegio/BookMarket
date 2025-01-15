using BookMarket.Services.Telnet.Commands;
using TelnetServer;
using System.Text;
using Entities.Telnet;
using BookMarket.Services.Entities;
using BookMarket.Services.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BookMarket.Services.Telnet
{
    public class TelnetCommandsService: IDisposable
    {

        private TelnetService _TelnetService { get; set; }
        private DbContextOptions<DbContextBase>? _DbOptions { get; set; }
        public TelnetCommandsService(DbContextOptions<DbContextBase>? dbOptions)
        {
            _DbOptions = dbOptions;
            //Create Telnet service
            _TelnetService = new TelnetService(
                new TCPServer(), //Multi client TCP server
                new ITelnetCommand[]//Custom commands we implemented
            {
                new HelpCommand(),
                new HelloCommand(),
                new EchoCommand(),
                new GetCommand(dbOptions),
                new BuyCommand(dbOptions),
                new RestockCommand(dbOptions),
                new ExitCommand(),
            });

            Start();
        }

        public void Start()
        {
            //Settings for the Telnet service
            TelnetServiceSettings _telnetSettings = new TelnetServiceSettings();
            _telnetSettings.PromtText = "BookMarket@" + Environment.MachineName;
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
