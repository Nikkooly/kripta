using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class Rnd
    {
        private Int64 _x;
        public Rnd()
        {
            _x = Int64.Parse(DateTime.Now.Ticks.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Int64 Next()
        {
            const int n = 11979;
            const int a = 430;
            const int c = 2531;
            return (a* _x + c) % n;
        }
    }
}
