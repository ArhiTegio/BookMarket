using Entities.Telnet;
using TelnetServer;

namespace BookMarket.Services.Telnet.Commands
{
    public class GetCommand : ITelnetCommand
    {
        CommandParameter[] m_Parameters;

        public string CommandName
        {
            get { return "get"; }
        }

        public string Description
        {
            get { return "Returns the full list of products, sorted by Id by default. The flags can be used simultaneously, for example: `get --title \"for dummies\" --order-by=\"count\"` - the output should be a list of products with the words \"for dummies\" in the name, sorted by their number in the database."; }
        }

        public IEnumerable<CommandParameter> Parameters { get { return m_Parameters; } }

        public GetCommand()
        {
            m_Parameters = new CommandParameter[]
            {
                new CommandParameter("--title", false, "--title=%% consider the product in the output only if the specified substring occurs in the name, if there are no such products, write a message."),
                new CommandParameter("--author", false, "--author=%% consider the product in the output only if the specified substring occurs in the author field, if there are no such products, write a message."),
                new CommandParameter("--date", false, "--date=%%, but for the date, the date is in the yyyy-MM-dd format, if the date is specified incorrectly, output the error text."),
                new CommandParameter("--order-by", false, "--order-by=[title|author|date|count] sort the list of products by the specified field, if an incorrect field is specified, then output the error text."),
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
