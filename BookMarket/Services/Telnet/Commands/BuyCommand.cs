using TelnetServer;

namespace BookMarket.Services.Telnet.Commands
{
    public class BuyCommand : ITelnetCommand
    {
        CommandParameter[] m_Parameters;

        public string CommandName
        {
            get { return "buy"; }
        }

        public string Description
        {
            get { return "reduces the number of specified items by 1."; }
        }

        public IEnumerable<CommandParameter> Parameters { get { return m_Parameters; } }

        public BuyCommand()
        {
            m_Parameters = new CommandParameter[]
            {
                new CommandParameter("--id", true, "--id=%% Id of the product to be purchased.")
            };
        }

        public TelnetCommandResult Execute(Dictionary<string, string> parameters)
        {
            string _enteredMessage = String.Empty;
            if (parameters.ContainsKey("message"))
                _enteredMessage = parameters["message"];

            return TelnetCommandResult.Success("You entered:" + _enteredMessage);
        }
    }
}
