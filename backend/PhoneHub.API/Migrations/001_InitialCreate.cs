using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

namespace PhoneHub.API.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                PasswordHash = table.Column<string>(type: "text", nullable: false),
                FirstName = table.Column<string>(type: "text", nullable: false),
                LastName = table.Column<string>(type: "text", nullable: false),
                IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                ProfilePictureUrl = table.Column<string>(type: "text", nullable: true),
                PhoneNumber = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Devices",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                DeviceName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                AndroidVersion = table.Column<string>(type: "text", nullable: false),
                Brand = table.Column<string>(type: "text", nullable: false),
                Model = table.Column<string>(type: "text", nullable: false),
                SerialNumber = table.Column<string>(type: "text", nullable: false),
                IMEI = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                IsOnline = table.Column<bool>(type: "boolean", nullable: false),
                LastSeen = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                DeviceToken = table.Column<string>(type: "text", nullable: true),
                IPAddress = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Devices", x => x.Id);
                table.ForeignKey(
                    name: "FK_Devices_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "DeviceStats",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigserial", nullable: false),
                DeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                BatteryLevel = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                IsCharging = table.Column<bool>(type: "boolean", nullable: false),
                MemoryUsage = table.Column<int>(type: "integer", nullable: false),
                TotalMemory = table.Column<int>(type: "integer", nullable: false),
                StorageUsage = table.Column<int>(type: "integer", nullable: false),
                TotalStorage = table.Column<int>(type: "integer", nullable: false),
                CpuUsage = table.Column<int>(type: "integer", nullable: false),
                WiFiSSID = table.Column<string>(type: "text", nullable: true),
                IsMobileDataEnabled = table.Column<bool>(type: "boolean", nullable: false),
                Latitude = table.Column<double>(type: "double precision", nullable: true),
                Longitude = table.Column<double>(type: "double precision", nullable: true),
                Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DeviceStats", x => x.Id);
                table.ForeignKey(
                    name: "FK_DeviceStats_Devices_DeviceId",
                    column: x => x.DeviceId,
                    principalTable: "Devices",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "FileTransfers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                DeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                FilePath = table.Column<string>(type: "text", nullable: false),
                FileName = table.Column<string>(type: "text", nullable: false),
                FileSize = table.Column<long>(type: "bigint", nullable: false),
                TransferType = table.Column<string>(type: "text", nullable: false),
                Status = table.Column<string>(type: "text", nullable: false),
                StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                ErrorMessage = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FileTransfers", x => x.Id);
                table.ForeignKey(
                    name: "FK_FileTransfers_Devices_DeviceId",
                    column: x => x.DeviceId,
                    principalTable: "Devices",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Notifications",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                DeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                ApplicationName = table.Column<string>(type: "text", nullable: false),
                Title = table.Column<string>(type: "text", nullable: false),
                Content = table.Column<string>(type: "text", nullable: false),
                PackageName = table.Column<string>(type: "text", nullable: false),
                ReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                IsRead = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Notifications", x => x.Id);
                table.ForeignKey(
                    name: "FK_Notifications_Devices_DeviceId",
                    column: x => x.DeviceId,
                    principalTable: "Devices",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ScreenFrames",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigserial", nullable: false),
                DeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                FrameData = table.Column<byte[]>(type: "bytea", nullable: true),
                Width = table.Column<int>(type: "integer", nullable: false),
                Height = table.Column<int>(type: "integer", nullable: false),
                FrameRate = table.Column<int>(type: "integer", nullable: false),
                Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                CompressionType = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ScreenFrames", x => x.Id);
                table.ForeignKey(
                    name: "FK_ScreenFrames_Devices_DeviceId",
                    column: x => x.DeviceId,
                    principalTable: "Devices",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "SilentModeSettings",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                DeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                KeepDisplayOff = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                MuteAudio = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                DisableVibration = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                DisableNotificationLED = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                HideSystemNotifications = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                DisableLogs = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                PIN = table.Column<string>(type: "text", nullable: true),
                PINHash = table.Column<string>(type: "text", nullable: true),
                ClearHistoryAfterSession = table.Column<bool>(type: "boolean", nullable: false),
                EnabledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SilentModeSettings", x => x.Id);
                table.ForeignKey(
                    name: "FK_SilentModeSettings_Devices_DeviceId",
                    column: x => x.DeviceId,
                    principalTable: "Devices",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "SilentModeSessions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                DeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                EndedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                OperationCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                Operations = table.Column<string>(type: "text", nullable: true),
                OperationLogs = table.Column<string>(type: "text", nullable: true),
                DisplayStateHistory = table.Column<string>(type: "text", nullable: true),
                IPAddress = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SilentModeSessions", x => x.Id);
                table.ForeignKey(
                    name: "FK_SilentModeSessions_Devices_DeviceId",
                    column: x => x.DeviceId,
                    principalTable: "Devices",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_SilentModeSessions_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "HiddenCommands",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                DeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                CommandType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Parameters = table.Column<string>(type: "text", nullable: true),
                IsExecuted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                ExecutedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                ErrorMessage = table.Column<string>(type: "text", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                OperationLogs = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HiddenCommands", x => x.Id);
                table.ForeignKey(
                    name: "FK_HiddenCommands_Devices_DeviceId",
                    column: x => x.DeviceId,
                    principalTable: "Devices",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_HiddenCommands_SilentModeSessions_SessionId",
                    column: x => x.SessionId,
                    principalTable: "SilentModeSessions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        // Create indices
        migrationBuilder.CreateIndex(
            name: "IX_Users_Email",
            table: "Users",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Devices_UserId",
            table: "Devices",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_DeviceStats_DeviceId_Timestamp",
            table: "DeviceStats",
            columns: new[] { "DeviceId", "Timestamp" },
            descending: new[] { false, true });

        migrationBuilder.CreateIndex(
            name: "IX_ScreenFrames_DeviceId_Timestamp",
            table: "ScreenFrames",
            columns: new[] { "DeviceId", "Timestamp" },
            descending: new[] { false, true });

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_DeviceId_ReceivedAt",
            table: "Notifications",
            columns: new[] { "DeviceId", "ReceivedAt" },
            descending: new[] { false, true });

        migrationBuilder.CreateIndex(
            name: "IX_SilentModeSettings_DeviceId",
            table: "SilentModeSettings",
            column: "DeviceId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_SilentModeSessions_DeviceId_StartedAt",
            table: "SilentModeSessions",
            columns: new[] { "DeviceId", "StartedAt" },
            descending: new[] { false, true });

        migrationBuilder.CreateIndex(
            name: "IX_SilentModeSessions_UserId",
            table: "SilentModeSessions",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_HiddenCommands_SessionId_CreatedAt",
            table: "HiddenCommands",
            columns: new[] { "SessionId", "CreatedAt" },
            descending: new[] { false, true });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("HiddenCommands");
        migrationBuilder.DropTable("SilentModeSessions");
        migrationBuilder.DropTable("SilentModeSettings");
        migrationBuilder.DropTable("ScreenFrames");
        migrationBuilder.DropTable("Notifications");
        migrationBuilder.DropTable("FileTransfers");
        migrationBuilder.DropTable("DeviceStats");
        migrationBuilder.DropTable("Devices");
        migrationBuilder.DropTable("Users");
    }
}
