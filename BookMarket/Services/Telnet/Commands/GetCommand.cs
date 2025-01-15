using BookMarket.Services.Entities;
using BookMarket.Services.Entities.Base;
using Entities.DateBase;
using Entities.Telnet;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;
using System.Text;
using TelnetServer;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private DbContextOptions<DbContextBase>? _DbOptions { get; set; }

        public GetCommand(DbContextOptions<DbContextBase>? dbOptions)
        {
            _DbOptions = dbOptions;
            m_Parameters = new CommandParameter[]
            {
                new CommandParameter("--title", false, "--title=\"%%\" consider the product in the output only if the specified substring occurs in the name, if there are no such products, write a message."),
                new CommandParameter("--author", false, "--author=\"%%\" consider the product in the output only if the specified substring occurs in the author field, if there are no such products, write a message."),
                new CommandParameter("--date", false, "--date=\"%%\", but for the date, the date is in the yyyy-MM-dd format, if the date is specified incorrectly, output the error text."),
                new CommandParameter("--order-by", false, "--order-by=\"[title|author|date|count]\" sort the list of products by the specified field, if an incorrect field is specified, then output the error text."),
            };
        }

        public TelnetCommandResult Execute(Dictionary<string, string> parameters)
        {
            StringBuilder _responseMessage = new();
            var isError = false;
            if (_DbOptions != null)
            {
                using (var db = new ApplicationContext(_DbOptions))
                {
                    var data = db.Products.ToArray();

                    if(parameters.ContainsKey("--title"))
                    {
                        data = data.Where(x => x.Title.Contains(parameters["--title"])).ToArray();
                    }

                    if(parameters.ContainsKey("--author"))
                    {
                        data = data.Where(x => x.Author.Contains(parameters["--author"])).ToArray();
                    }

                    if (parameters.ContainsKey("--date"))
                    {
                        string[] formats = { "yyyy-MM-dd" };
                        DateTime parsedDate;
                        var isValidFormat = DateTime.TryParseExact(parameters["--date"], formats, new CultureInfo("en-US"), DateTimeStyles.None, out parsedDate);
                        if (isValidFormat)
                        {
                            data = data.Where(x => x.YearPublication == parsedDate.Year).ToArray();
                        }
                        else
                        {
                            isError = true;
                            _responseMessage.Append($"{DateTime.Now} - Error. The date format is incorrect.");
                        }
                    }
                    if (!isError)
                    {
                        if (!parameters.ContainsKey("--order-by"))
                        {
                            data = data.OrderBy(x => x.Id).ToArray();
                        }
                        else
                        {
                            if (parameters["--order-by"].Contains("title"))
                            {
                                data = data.OrderBy(x => x.Title).ToArray();
                            }
                            else if (parameters["--order-by"].Contains("author"))
                            {
                                data = data.OrderBy(x => x.Author).ToArray();
                            }
                            else if (parameters["--order-by"].Contains("count"))
                            {
                                data = data.OrderBy(x => x.Quantity).ToArray();
                            }
                            else if (parameters["--order-by"].Contains("date"))
                            {
                                data = data.OrderBy(x => x.YearPublication).ToArray();
                            }
                        }

                        _responseMessage.Append($"{DateTime.Now} - The productsin stock:\r\n");
                        foreach (var p in data)
                        {
                            _responseMessage.Append($"\tId:{p.Id} Title:'{p.Title}' Author:'{p.Author}' Year:{p.YearPublication} Quantity:{p.Quantity}\r\n");
                        }
                    }
                }
            }
            else
            {
                _responseMessage.Append($"{DateTime.Now} - DataBase options not found.");
            }

            return TelnetCommandResult.Success(_responseMessage.ToString());
        }
    }
}
