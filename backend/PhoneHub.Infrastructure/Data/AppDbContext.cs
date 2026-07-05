using PhoneHub.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace PhoneHub.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Existing DbSets
    public DbSet<User> Users { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<DeviceStats> DeviceStats { get; set; }
    public DbSet<ScreenFrame> ScreenFrames { get; set; }
    public DbSet<FileTransfer> FileTransfers { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    // Silent Mode DbSets
    public DbSet<SilentModeSettings> SilentModeSettings { get; set; }
    public DbSet<SilentModeSession> SilentModeSessions { get; set; }
    public DbSet<HiddenCommand> HiddenCommands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User Configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasMany(e => e.Devices).WithOne(d => d.User).HasForeignKey(d => d.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        // Device Configuration
        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DeviceName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.IMEI).HasMaxLength(20);
            entity.HasOne(e => e.User).WithMany(u => u.Devices).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.SilentModeSettings).WithOne(s => s.Device).HasForeignKey<SilentModeSettings>(s => s.DeviceId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.SilentModeSessions).WithOne(s => s.Device).HasForeignKey(s => s.DeviceId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.HiddenCommands).WithOne(c => c.Device).HasForeignKey(c => c.DeviceId).OnDelete(DeleteBehavior.Cascade);
        });

        // DeviceStats Configuration
        modelBuilder.Entity<DeviceStats>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.BatteryLevel).HasDefaultValue(0);
            entity.HasOne(e => e.Device).WithMany(d => d.DeviceStats).HasForeignKey(e => e.DeviceId).OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => new { e.DeviceId, e.Timestamp }).IsDescending(false, true);
        });

        // ScreenFrame Configuration
        modelBuilder.Entity<ScreenFrame>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Device).WithMany(d => d.ScreenFrames).HasForeignKey(e => e.DeviceId).OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => new { e.DeviceId, e.Timestamp }).IsDescending(false, true);
        });

        // FileTransfer Configuration
        modelBuilder.Entity<FileTransfer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FilePath).IsRequired();
            entity.HasOne(e => e.Device).WithMany(d => d.FileTransfers).HasForeignKey(e => e.DeviceId).OnDelete(DeleteBehavior.Cascade);
        });

        // Notification Configuration
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Device).WithMany(d => d.Notifications).HasForeignKey(e => e.DeviceId).OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => new { e.DeviceId, e.ReceivedAt }).IsDescending(false, true);
        });

        // SilentModeSettings Configuration
        modelBuilder.Entity<SilentModeSettings>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.KeepDisplayOff).HasDefaultValue(true);
            entity.Property(e => e.MuteAudio).HasDefaultValue(true);
            entity.Property(e => e.DisableVibration).HasDefaultValue(true);
            entity.Property(e => e.DisableNotificationLED).HasDefaultValue(true);
            entity.Property(e => e.HideSystemNotifications).HasDefaultValue(true);
            entity.Property(e => e.DisableLogs).HasDefaultValue(true);
            entity.HasOne(e => e.Device).WithOne(d => d.SilentModeSettings).HasForeignKey<SilentModeSettings>(e => e.DeviceId).OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => e.DeviceId).IsUnique();
        });

        // SilentModeSession Configuration
        modelBuilder.Entity<SilentModeSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OperationCount).HasDefaultValue(0);
            entity.HasOne(e => e.Device).WithMany(d => d.SilentModeSessions).HasForeignKey(e => e.DeviceId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany<HiddenCommand>().WithOne(c => c.SilentModeSession).HasForeignKey(c => c.SessionId).OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => new { e.DeviceId, e.StartedAt }).IsDescending(false, true);
            entity.HasIndex(e => e.UserId);
        });

        // HiddenCommand Configuration
        modelBuilder.Entity<HiddenCommand>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CommandType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.IsExecuted).HasDefaultValue(false);
            entity.HasOne(e => e.Device).WithMany(d => d.HiddenCommands).HasForeignKey(e => e.DeviceId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.SilentModeSession).WithMany().HasForeignKey(e => e.SessionId).OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => new { e.SessionId, e.CreatedAt }).IsDescending(false, true);
        });
    }
}
