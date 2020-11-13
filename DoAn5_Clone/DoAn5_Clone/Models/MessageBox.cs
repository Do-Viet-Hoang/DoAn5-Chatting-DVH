using System;
using System.Collections.Generic;

namespace DoAn5_Clone.Models
{
    public partial class MessageBox
    {
        public Guid MessageBoxId { get; set; }
        public Guid? FromUserId { get; set; }
        public Guid? ToUserId { get; set; }

        public virtual Users FromUser { get; set; }
        public virtual Users ToUser { get; set; }
    }
}
