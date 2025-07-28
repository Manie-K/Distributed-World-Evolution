using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Frames
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DataFrameBase
    {
        public int Size { get; protected set; }
    }
}
