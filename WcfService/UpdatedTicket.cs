using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService
{
    public class UpdatedTicket
    {

        public string SolvedBy { get; set; }

        public bool Solved { get; set; }

        public int? UserNameKey { get; set; }
    }
}