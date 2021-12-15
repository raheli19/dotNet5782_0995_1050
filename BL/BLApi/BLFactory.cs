using System;
using System.Collections.Generic;
using System.Text;

namespace BLApi
{
    public static class BLFactory
    {
        public static IBL GetBL() => BL.BL.Instance;
    }
}
