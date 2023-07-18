using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Constant
{
    internal class PaymentStatusConstant
    {
        public static readonly int ACTIVE = 1;
        public static readonly int PENDING = 2;
        public static readonly int EXPIRE = 3;
        public static readonly int CANCEL = 4;
    }
}
