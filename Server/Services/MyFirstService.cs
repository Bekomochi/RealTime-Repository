using MagicOnion;
using MagicOnion.Server;
using Shared.Interfaces.Services;

namespace Server.Services
{
    public class MyFirstService : ServiceBase<IMyFirstService>, IMyFirstService
    {
        public async UnaryResult<int> SumAsync(int x, int y)
        {
            Console.WriteLine("Received" + x + ", " + y);
            return x + y;
        }

        public async UnaryResult<int> SumAllAsync(int[] numList)
        {
            int total=0;

            foreach (int x in numList)
            {
                total += x;
                Console.WriteLine("total");
                
            }
            return total;
        }

        public async UnaryResult<float> SumAllNumberAsync(Number numArray)
        {
            Console.WriteLine("Received" +numArray.x+numArray.y);
            return numArray.x + numArray.y;
        }

        //public async UnaryResult<int> CalcForOperationAsync(int x, int y)
        //{
        //    int 
        //}
    }
}
