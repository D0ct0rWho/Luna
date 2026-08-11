using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Luna.Models;

namespace Luna.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Chat> Chats => Set<Chat>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Contact> Contacts => Set<Contact>();
        public DbSet<ChatParticipant> ChatParticipants => Set<ChatParticipant>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Contact: два внешних ключа на AspNetUsers
            builder.Entity<Contact>(entity =>
            {
                entity.HasOne(c => c.User)
                      .WithMany(u => u.Contacts)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.ContactUser)
                      .WithMany()
                      .HasForeignKey(c => c.ContactUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(c => new { c.UserId, c.ContactUserId }).IsUnique();
            });

            // ChatParticipant: составной первичный ключ
            builder.Entity<ChatParticipant>(entity =>
            {
                entity.HasKey(cp => new { cp.ChatId, cp.UserId });

                entity.HasOne(cp => cp.Chat)
                      .WithMany(c => c.Participants)
                      .HasForeignKey(cp => cp.ChatId);

                entity.HasOne(cp => cp.User)
                      .WithMany(u => u.ChatParticipants)
                      .HasForeignKey(cp => cp.UserId);
            });

            // Message
            builder.Entity<Message>(entity =>
            {
                entity.HasOne(m => m.Chat)
                      .WithMany(c => c.Messages)
                      .HasForeignKey(m => m.ChatId);

                entity.HasOne(m => m.Sender)
                      .WithMany(u => u.Messages)
                      .HasForeignKey(m => m.SenderId);
            });
        }
    }
}