using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BKDAPI.Models
{
    public class Response
    {        
            public bool Status { get; set; }        
            public string ResponseValue { get; set; }
            public string ResponseMessage { get; set; }
    }
}