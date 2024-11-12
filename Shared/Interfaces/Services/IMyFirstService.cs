using MagicOnion;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Interfaces.Services
{
    /// <summary>
    /// 初めてのRPCサービス
    /// </summary>
    public interface IMyFirstService:IService<IMyFirstService>
    {
        UnaryResult<int> SumAsync(int x, int y);

        /// <summary>
        /// 足し算処理を行う
        /// </summary>
        /// <param name="numList"></param>
        /// <returns></returns>
        UnaryResult<int> SumAllAsync(int[] numList);

        ////x+yを0に、x-yを1に、x*yを2に、x/yを3に入れて、配列で返す
        //UnaryResult<int[]>CalcForOperationAsync(int x, int y);

        //xとyの小数をフィールドに持つNumverクラスを渡して、x+yの結果を返す
        UnaryResult<float> SumAllNumberAsync(Number numArray);
    }

    [MessagePackObject] public class Number
    {
        [Key(0)] public float x;
        [Key(1)] public float y;
    }
}
