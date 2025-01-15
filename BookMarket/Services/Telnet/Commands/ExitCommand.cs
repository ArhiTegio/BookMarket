using Entities.Telnet;

namespace BookMarket.Services.Telnet.Commands
{
    public class ExitCommand : ITelnetCommand
    {
        public string CommandName
        {
            get { return "exit"; }
        }

        public string Description
        {
            get { return "Exit from command line."; }
        }

        public IEnumerable<CommandParameter> Parameters { get { return null; } }

        public TelnetCommandResult Execute(Dictionary<string, string> parameters)
        {
            return TelnetCommandResult.Success("Hello from Telnet service!");
        }
    }
}
