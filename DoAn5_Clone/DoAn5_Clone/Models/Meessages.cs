using System;
using System.Collections.Generic;

namespace DoAn5_Clone.Models
{
    public partial class Meessages
    {
        public Meessages()
        {
            MessageGroupMedia = new HashSet<MessageGroupMedia>();
        }

        public Guid MessageId { get; set; }
        public Guid? MessageGroupId { get; set; }
        public Guid? FromUserId { get; set; }
        public Guid? ToUserId { get; set; }
        public string NameMessage { get; set; }
        public int? MediaFlag { get; set; }
        public string Content { get; set; }
        public string MediaFilePath { get; set; }
        public int? ActiveFlag { get; set; }
        public DateTime? CreatedDateTime { get; set; }

        public virtual Users FromUser { get; set; }
        public virtual MessageGroup MessageGroup { get; set; }
        public virtual Users ToUser { get; set; }
        public virtual ICollection<MessageGroupMedia> MessageGroupMedia { get; set; }
    }
}
