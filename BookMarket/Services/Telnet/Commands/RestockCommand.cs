using Entities.Telnet;
using TelnetServer;

namespace BookMarket.Services.Telnet.Commands
{
    public class RestockCommand : ITelnetCommand
    {
        CommandParameter[] m_Parameters;

        public string CommandName
        {
            get { return "restock"; }
        }

        public string Description
        {
            get { return "Increases the random number of random items by a random positive number. If the flag indicates the ID or quantity, then top up according to the specified flags."; }
        }

        public IEnumerable<CommandParameter> Parameters { get { return m_Parameters; } }

        public RestockCommand()
        {
            m_Parameters = new CommandParameter[]
            {
                new CommandParameter("--id", true, "--id=%% Id of the product, the quantity of which needs to be replenished."),
                new CommandParameter("--count", true, "--count=%% The number by which you need to increase the quantity of the product"),
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
