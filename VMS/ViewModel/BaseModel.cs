using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VMS.ViewModel;

namespace VMS.ViewModel
{
    public abstract class BaseModel
    {
        public BaseModel()
        {
        }
        public DateTime CreatedOn { get; set; }
        //public long CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        //public long UpdateBy { get; set; }

        public TransactionMessageVM TransactionMessage { get; set; }
    }
}
