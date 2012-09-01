using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ana.Contracts.Business
{
    public interface IEmailProvider
    {
        void SendEmail(string to, string subject, string body);
    }
}
