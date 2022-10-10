using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Domain;

namespace HookIn
{
    class Program
    {
        static void Main(string[] args)
        {
            Repository rep=new Repository();
            List<StockItem> itemsList = rep.GetStockItems();
            foreach(StockItem stk in itemsList)
            {
                Console.WriteLine(stk.Id+stk.Name);
            }
            Console.ReadKey();
        }
    }
}
