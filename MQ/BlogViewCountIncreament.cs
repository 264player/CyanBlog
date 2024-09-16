using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ
{
    public class BlogViewCountIncreament : IMessageHandler
    {
        public async Task ExecuteAsync()
        {
            await Task.Run(() => Console.WriteLine("temp")) ;
        }
    }
}
