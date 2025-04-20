using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VMS.Helper;
using VMS.Models;

namespace VMS.ViewModel
{
    public class TransactionMessageVM : BaseModel
    {
        public TransactionMessageVM()
        {
            //Message = string.Empty;
        }

        public string Title { get; set; }

        public string Message { get; set; }

        public TransactionStatus Status { get; set; }

        public object Data { get; set; }
    }
}