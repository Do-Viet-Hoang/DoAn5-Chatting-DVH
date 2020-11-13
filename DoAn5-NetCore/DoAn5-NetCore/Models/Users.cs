using System;
using System.Collections.Generic;

namespace DoAn5_NetCore.Models
{
    public partial class Users
    {
        public Users()
        {
            MeessagesFromUser = new HashSet<Meessages>();
            MeessagesToUser = new HashSet<Meessages>();
            MessageBoxFromUser = new HashSet<MessageBox>();
            MessageBoxToUser = new HashSet<MessageBox>();
            MessageGroupFromUser = new HashSet<MessageGroup>();
            MessageGroupToUser = new HashSet<MessageGroup>();
        }

        public Guid UsersId { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Addresss { get; set; }
        public string Sdt { get; set; }
        public string Avatar { get; set; }
        public bool? Roles { get; set; }
        public int? FriendsCounter { get; set; }
        public bool? ActiveFlag { get; set; }
        public DateTime? CreatedDateTime { get; set; }

        public virtual ICollection<Meessages> MeessagesFromUser { get; set; }
        public virtual ICollection<Meessages> MeessagesToUser { get; set; }
        public virtual ICollection<MessageBox> MessageBoxFromUser { get; set; }
        public virtual ICollection<MessageBox> MessageBoxToUser { get; set; }
        public virtual ICollection<MessageGroup> MessageGroupFromUser { get; set; }
        public virtual ICollection<MessageGroup> MessageGroupToUser { get; set; }
    }
}
