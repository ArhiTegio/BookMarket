using Entities.Telnet;

namespace BookMarket.Services.Telnet.Commands
{
    public class HelpCommand : ITelnetCommand
    {
        public string CommandName
        {
            get { return "help"; }
        }

        public string Description
        {
            get { return "Displays all commands."; }
        }

        public IEnumerable<CommandParameter> Parameters { get { return null; } }

        public TelnetCommandResult Execute(Dictionary<string, string> parameters)
        {
            return TelnetCommandResult.Success("Hello from Telnet service!");
        }
    }
}
