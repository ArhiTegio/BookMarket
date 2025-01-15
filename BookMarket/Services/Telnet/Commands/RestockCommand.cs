using BookMarket.Services.Entities;
using BookMarket.Services.Entities.Base;
using Entities.DateBase;
using Entities.Telnet;
using Microsoft.EntityFrameworkCore;
using System.Text;
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
        private DbContextOptions<DbContextBase>? _DbOptions { get; set; }

        public RestockCommand(DbContextOptions<DbContextBase>? dbOptions)
        {
            _DbOptions = dbOptions;
            m_Parameters = new CommandParameter[]
            {
                new CommandParameter("--id", false, "--id=\"%%\" Id of the product, the quantity of which needs to be replenished."),
                new CommandParameter("--count", false, "--count=\"%%\" The number by which you need to increase the quantity of the product"),
            };
        }

        public TelnetCommandResult Execute(Dictionary<string, string> parameters)
        {
            StringBuilder _responseMessage = new();
            var rnd = new Random();
            if (_DbOptions != null)
            {
                try
                {
                    using (var db = new ApplicationContext(_DbOptions))
                    {
                        _responseMessage.Append($"{DateTime.Now} - List of products with the revealed quantity:\r\n");
                        if (parameters.ContainsKey("--id"))
                        {
                            if (int.TryParse(new string(parameters["--id"].Where(char.IsDigit).ToArray()), out int id))
                            {
                                var p = db.Products.Where(x => x.Id == id);
                                if (p.Count() > 0)
                                {
                                    if (parameters.ContainsKey("--count"))
                                    {
                                        if (int.TryParse(new string(parameters["--count"].Where(char.IsDigit).ToArray()), out int count))
                                        {
                                            foreach(var e in p)
                                            {
                                                e.Quantity += count;
                                                _responseMessage.Append($"\tId:{e.Id} Title:'{e.Title}' Author:'{e.Author}' Year:{e.YearPublication} Quantity:{e.Quantity}\r\n");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (var e in p)
                                        {
                                            e.Quantity += rnd.Next(1, 5);
                                            _responseMessage.Append($"\tId:{e.Id} Title:'{e.Title}' Author:'{e.Author}' Year:{e.YearPublication} Quantity:{e.Quantity}\r\n");
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var countProducts = rnd.Next(1, 10);
                            var getProducts = new HashSet<int>();
                            var countInTable = db.Products.Count() - 1;
                            var idsProducts = new HashSet<int>(db.Products.Select(x => x.Id).ToList());
                            var isRandom = true;
                            var add = -1;
                            if (parameters.ContainsKey("--count"))
                            {
                                if (int.TryParse(new string(parameters["--count"].Where(char.IsDigit).ToArray()), out int count))
                                {
                                    add = count;
                                    isRandom = false;
                                }
                            }

                            for (int i = 0; i < countProducts; ++i)
                            {
                                var id = -1;
                                do
                                {
                                    id = rnd.Next(0, countInTable);
                                }
                                while (getProducts.Contains(id) || !idsProducts.Contains(id));
                                var e = db.Products.Where(x => x.Id == id).First();
                                if (isRandom)
                                {
                                    e.Quantity += rnd.Next(1, 5);
                                }
                                else
                                {
                                    e.Quantity += add;
                                }
                                _responseMessage.Append($"\tId:{e.Id} Title:'{e.Title}' Author:'{e.Author}' Year:{e.YearPublication} Quantity:{e.Quantity}\r\n");
                            }
                        }

                        db.SaveChanges();
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


            return TelnetCommandResult.Success(_responseMessage.ToString());
        }
    }
}
