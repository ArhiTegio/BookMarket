using BookMarket.Services.Entities;
using BookMarket.Services.Entities.Base;
using Entities.DateBase;
using Entities.Telnet;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text;
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
        private DbContextOptions<DbContextBase>? _DbOptions { get; set; }

        public BuyCommand(DbContextOptions<DbContextBase>? dbOptions)
        {
            _DbOptions = dbOptions;
            m_Parameters = new CommandParameter[]
            {
                new CommandParameter("--id", true, "--id=\"%%\" Id of the product to be purchased.")
            };
        }

        public TelnetCommandResult Execute(Dictionary<string, string> parameters)
        {
            StringBuilder _responseMessage = new();
            if (parameters.ContainsKey("--id"))
            {
                if (_DbOptions != null)
                {
                    try
                    {
                        using (var db = new ApplicationContext(_DbOptions))
                        {
                            if (int.TryParse(new string(parameters["--id"].Where(char.IsDigit).ToArray()), out int id))
                            {
                                var toDelete = new List<Product>();
                                foreach (var p in db.Products.Where(x => x.Id == id))
                                {
                                    if (p.Quantity > 0)
                                    {
                                        p.Quantity--;
                                        db.SaveChanges();
                                        _responseMessage.Append($"{DateTime.Now} - Product status after buying\r\n\tId:{p.Id} Title:'{p.Title}' Author:'{p.Author}' Year:{p.YearPublication} Quantity:{p.Quantity}");
                                    }
                                    else
                                    {
                                        _responseMessage.Append($"{DateTime.Now} - The product is finished, not buying\r\n\tId:{p.Id} Title:'{p.Title}' Author:'{p.Author}' Year:{p.YearPublication} Quantity:{p.Quantity}");

                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _responseMessage.Append(ex.Message + "\r\n" + ex.StackTrace);
                    }
                }
                else
                {
                    _responseMessage.Append($"{DateTime.Now} - DataBase options not found.");
                }
            }

            return TelnetCommandResult.Success(_responseMessage.ToString());
        }
    }
}
