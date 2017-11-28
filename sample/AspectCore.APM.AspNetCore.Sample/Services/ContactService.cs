using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspectCore.APM.AspNetCore.Sample.Services
{
    public class ContactService : IContactService
    {
        public string GetMessage()
        {
            return "Your contact page.";
        }
    }
}
