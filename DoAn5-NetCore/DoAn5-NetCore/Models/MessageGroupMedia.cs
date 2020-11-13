using System;
using System.Collections.Generic;

namespace DoAn5_NetCore.Models
{
    public partial class MessageGroupMedia
    {
        public Guid MessageMediaId { get; set; }
        public Guid? MessageGroupId { get; set; }
        public Guid? MessageId { get; set; }
        public DateTime? LifeDateTime { get; set; }
        public long? FileLength { get; set; }
        public DateTime? CreatedDateTime { get; set; }

        public virtual Meessages Message { get; set; }
        public virtual MessageGroup MessageGroup { get; set; }
    }
}
