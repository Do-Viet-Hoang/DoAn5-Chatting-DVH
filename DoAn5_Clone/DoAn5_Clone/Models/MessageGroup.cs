using System;
using System.Collections.Generic;

namespace DoAn5_Clone.Models
{
    public partial class MessageGroup
    {
        public MessageGroup()
        {
            Meessages = new HashSet<Meessages>();
            MessageGroupMedia = new HashSet<MessageGroupMedia>();
        }

        public Guid MessageGroupId { get; set; }
        public Guid? FromUserId { get; set; }
        public Guid? ToUserId { get; set; }
        public string Title { get; set; }
        public DateTime? LastSendingDatetime { get; set; }
        public string LastMessage { get; set; }
        public bool? MarkReading { get; set; }
        public bool? ActiveFlag { get; set; }
        public DateTime? CreatedDateTime { get; set; }

        public virtual Users FromUser { get; set; }
        public virtual Users ToUser { get; set; }
        public virtual ICollection<Meessages> Meessages { get; set; }
        public virtual ICollection<MessageGroupMedia> MessageGroupMedia { get; set; }
    }
}
