using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Domain.Interface
{
    public interface IUserLogin
    {
        public void CheckUserCardNumberAndPassword();
    }
}
