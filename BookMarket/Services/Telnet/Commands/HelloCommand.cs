using TelnetServer;

namespace BookMarket.Services.Telnet.Commands
{
    class HelloCommand : ITelnetCommand
    {
        public string CommandName
        {
            get { return "hello"; }
        }

        public string Description
        {
            get { return "Displays hello message."; }
        }

        public IEnumerable<CommandParameter> Parameters { get { return null; } }

        public TelnetCommandResult Execute(Dictionary<string, string> parameters)
        {
            return TelnetCommandResult.Success("Hello from Telnet service!");
        }
    }
}
