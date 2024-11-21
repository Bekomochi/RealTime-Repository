using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class BaseModel : MonoBehaviour
    {
        /*MagicOnion通信用のベースクラスを作っておく。
         * 現状、やることは無いがURLはここ1箇所にする。*/

        public const string ServerURL = "http://localhost:7000";
    }
}
