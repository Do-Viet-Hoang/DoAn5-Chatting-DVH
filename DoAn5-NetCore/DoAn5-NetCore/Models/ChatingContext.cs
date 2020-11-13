using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DoAn5_NetCore.Models
{
    public partial class ChatingContext : DbContext
    {
        //public ChatingContext()
        //{
        //}

        public ChatingContext(DbContextOptions<ChatingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Meessages> Meessages { get; set; }
        public virtual DbSet<MessageBox> MessageBox { get; set; }
        public virtual DbSet<MessageGroup> MessageGroup { get; set; }
        public virtual DbSet<MessageGroupMedia> MessageGroupMedia { get; set; }
        public virtual DbSet<Users> Users { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=DESKTOP-R9KU03V\\DOVIETHOANG;Database=Chating;Integrated Security=True");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meessages>(entity =>
            {
                entity.HasKey(e => e.MessageId)
                    .HasName("PK__Meessage__F5A1327A131A4BDA");

                entity.Property(e => e.MessageId)
                    .HasColumnName("Message_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ActiveFlag).HasColumnName("active_flag");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnName("created_date_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.FromUserId).HasColumnName("from_user_id");

                entity.Property(e => e.MediaFilePath).HasColumnName("media_file_path");

                entity.Property(e => e.MediaFlag).HasColumnName("media_flag");

                entity.Property(e => e.MessageGroupId).HasColumnName("Message_group_id");

                entity.Property(e => e.NameMessage)
                    .HasColumnName("name_message")
                    .HasMaxLength(50);

                entity.Property(e => e.ToUserId).HasColumnName("to_user_id");

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.MeessagesFromUser)
                    .HasForeignKey(d => d.FromUserId)
                    .HasConstraintName("FK__Meessages__from___534D60F1");

                entity.HasOne(d => d.MessageGroup)
                    .WithMany(p => p.Meessages)
                    .HasForeignKey(d => d.MessageGroupId)
                    .HasConstraintName("FK__Meessages__Messa__52593CB8");

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.MeessagesToUser)
                    .HasForeignKey(d => d.ToUserId)
                    .HasConstraintName("FK__Meessages__to_us__5441852A");
            });

            modelBuilder.Entity<MessageBox>(entity =>
            {
                entity.Property(e => e.MessageBoxId)
                    .HasColumnName("Message_box_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.FromUserId).HasColumnName("from_user_id");

                entity.Property(e => e.ToUserId).HasColumnName("to_user_id");

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.MessageBoxFromUser)
                    .HasForeignKey(d => d.FromUserId)
                    .HasConstraintName("FK__MessageBo__from___5CD6CB2B");

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.MessageBoxToUser)
                    .HasForeignKey(d => d.ToUserId)
                    .HasConstraintName("FK__MessageBo__to_us__5DCAEF64");
            });

            modelBuilder.Entity<MessageGroup>(entity =>
            {
                entity.Property(e => e.MessageGroupId)
                    .HasColumnName("message_group_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ActiveFlag).HasColumnName("active_flag");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnName("created_date_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.FromUserId).HasColumnName("from_user_id");

                entity.Property(e => e.LastMessage).HasColumnName("last_message");

                entity.Property(e => e.LastSendingDatetime)
                    .HasColumnName("last_sending_datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.MarkReading).HasColumnName("mark_reading");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(250);

                entity.Property(e => e.ToUserId).HasColumnName("to_user_id");

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.MessageGroupFromUser)
                    .HasForeignKey(d => d.FromUserId)
                    .HasConstraintName("FK__MessageGr__from___4D94879B");

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.MessageGroupToUser)
                    .HasForeignKey(d => d.ToUserId)
                    .HasConstraintName("FK__MessageGr__to_us__4E88ABD4");
            });

            modelBuilder.Entity<MessageGroupMedia>(entity =>
            {
                entity.HasKey(e => e.MessageMediaId)
                    .HasName("PK__MessageG__6F0AF74140869051");

                entity.Property(e => e.MessageMediaId)
                    .HasColumnName("Message_media_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnName("created_date_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.FileLength).HasColumnName("file_length");

                entity.Property(e => e.LifeDateTime)
                    .HasColumnName("life_date_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.MessageGroupId).HasColumnName("Message_group_id");

                entity.Property(e => e.MessageId).HasColumnName("Message_id");

                entity.HasOne(d => d.MessageGroup)
                    .WithMany(p => p.MessageGroupMedia)
                    .HasForeignKey(d => d.MessageGroupId)
                    .HasConstraintName("FK__MessageGr__Messa__5812160E");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.MessageGroupMedia)
                    .HasForeignKey(d => d.MessageId)
                    .HasConstraintName("FK__MessageGr__Messa__59063A47");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.UsersId)
                    .HasColumnName("users_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ActiveFlag).HasColumnName("active_flag");

                entity.Property(e => e.Addresss)
                    .HasColumnName("addresss")
                    .HasMaxLength(50);

                entity.Property(e => e.Avatar)
                    .HasColumnName("avatar")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnName("created_date_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnName("date_of_birth")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(250);

                entity.Property(e => e.FriendsCounter).HasColumnName("friends_counter");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasColumnName("full_name")
                    .HasMaxLength(250);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnName("password_hash");

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasColumnName("password_salt");

                entity.Property(e => e.Roles).HasColumnName("roles");

                entity.Property(e => e.Sdt)
                    .HasColumnName("SDT")
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
