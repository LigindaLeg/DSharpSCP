using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using InventorySystem.Items.ThrowableProjectiles;
using Discord.WebSocket;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using Scp018Projectile = LabApi.Features.Wrappers.Scp018Projectile;

namespace DSharpSCP.Logs;

public class EventHandlers
{
    private static readonly SemaphoreSlim LogLock = new(1, 1);

    public static class PlayerEvents
    {
        public static void Joined(PlayerJoinedEventArgs e)
        {
            if (e.Player == null || e.Player.IsDummy || e.Player.IsNpc || e.Player.IsHost)
                return;

            _ = SendLog(
                Main.Instance.Config.JoinedLog
                    .Replace("{player}", e.Player.Nickname)
                    .Replace("{steamid}", e.Player.UserId),
                Main.Instance.Config.JoinedLogChannelId);
        }

        public static void Banned(PlayerBannedEventArgs ev)
        {
            if (ev.Player == null || ev.Issuer == null)
                return;

            var now = DateTime.UtcNow;
            var isPermanent = ev.Duration <= 0;
            var expires = isPermanent ? DateTime.MaxValue : now.AddSeconds(ev.Duration);
            var duration = isPermanent ? TimeSpan.Zero : expires - now;
            var expiresText = isPermanent ? "Never" : $"<t:{((DateTimeOffset)expires).ToUnixTimeSeconds()}:R>";

            Types.V2Log log = new()
            {
                Header = Main.Instance.Config.BanLog.Header,
                Separator = Main.Instance.Config.BanLog.Separator,
                Log = Main.Instance.Config.BanLog.Log
                    .Replace("{player}", ev.Player.Nickname)
                    .Replace("{reason}", ev.Reason ?? "No reason")
                    .Replace("{admin}", ev.Issuer.Nickname)
                    .Replace("{durationDays}", duration.Days.ToString())
                    .Replace("{durationHours}", duration.Hours.ToString())
                    .Replace("{durationMinutes}", duration.Minutes.ToString())
                    .Replace("{expires}", expiresText)
            };
            _ = SendCompV2Log(log, Main.Instance.Config.BanLogChannel);

            log = new()
            {
                Header = Main.Instance.Config.ForAdminBanLog.Header,
                Separator = Main.Instance.Config.ForAdminBanLog.Separator,
                Log = Main.Instance.Config.ForAdminBanLog.Log
                    .Replace("{player}", ev.Player.Nickname)
                    .Replace("{playerid}", ev.Player.UserId)
                    .Replace("{reason}", ev.Reason ?? "No reason")
                    .Replace("{admin}", ev.Issuer.Nickname)
                    .Replace("{adminid}", ev.Issuer.UserId)
                    .Replace("{durationDays}", duration.Days.ToString())
                    .Replace("{durationHours}", duration.Hours.ToString())
                    .Replace("{durationMinutes}", duration.Minutes.ToString())
                    .Replace("{expires}", expiresText)
            };
            _ = SendCompV2Log(log, Main.Instance.Config.ForAdminBanLogChannel);
        }

        public static void Left(PlayerLeftEventArgs ev)
        {
            if (ev.Player == null || ev.Player.IsDummy || ev.Player.IsNpc || ev.Player.IsHost)
                return;

            _ = SendLog(
                Main.Instance.Config.LeftLog
                    .Replace("{player}", ev.Player.Nickname)
                    .Replace("{steamid}", ev.Player.UserId),
                Main.Instance.Config.LeftLogChannelId);
        }

        public static void Muted(PlayerMutedEventArgs ev)
        {
            if (ev.Player == null || ev.Issuer == null)
                return;

            Types.V2Log log = new()
            {
                Header = Main.Instance.Config.MuteLog.Header,
                Separator = Main.Instance.Config.MuteLog.Separator,
                Log = Main.Instance.Config.MuteLog.Log
                    .Replace("{player}", ev.Player.Nickname)
                    .Replace("{admin}", ev.Issuer.Nickname)
            };
            _ = SendCompV2Log(log, Main.Instance.Config.MuteLogChannel);

            log = new()
            {
                Header = Main.Instance.Config.ForAdminMuteLog.Header,
                Separator = Main.Instance.Config.ForAdminMuteLog.Separator,
                Log = Main.Instance.Config.ForAdminMuteLog.Log
                    .Replace("{player}", ev.Player.Nickname)
                    .Replace("{playerid}", ev.Player.UserId)
                    .Replace("{admin}", ev.Issuer.Nickname)
                    .Replace("{adminid}", ev.Issuer.UserId)
            };
            _ = SendCompV2Log(log, Main.Instance.Config.ForAdminMuteLogChannel);
        }

        public static void Death(PlayerDeathEventArgs ev)
        {
            if (ev.Attacker == null || ev.Player == null || ev.Player.IsNpc || ev.Player.IsHost || ev.Attacker.IsNpc || ev.Attacker.IsHost)
                return;

            _ = SendLog(
                Main.Instance.Config.KillLog
                    .Replace("{player}", ev.Attacker.Nickname)
                    .Replace("{playerid}", ev.Attacker.UserId)
                    .Replace("{target}", ev.Player.Nickname)
                    .Replace("{targetid}", ev.Player.UserId)
                    .Replace("{reason}", ev.DamageHandler.DeathScreenText ?? "Unknown"),
                Main.Instance.Config.KillLogChannel);
        }

        public static void Cuffed(PlayerCuffedEventArgs ev)
        {
            if (ev.Target == null || ev.Player == null || ev.Player.IsNpc || ev.Player.IsHost || ev.Target.IsNpc || ev.Target.IsHost)
                return;

            _ = SendLog(
                Main.Instance.Config.CuffLog
                    .Replace("{player}", ev.Player.Nickname)
                    .Replace("{playerid}", ev.Player.UserId)
                    .Replace("{target}", ev.Target.Nickname)
                    .Replace("{targetid}", ev.Target.UserId),
                Main.Instance.Config.CuffLogChannel);
        }

        public static void ReportedPlayer(PlayerReportedPlayerEventArgs ev)
        {
            if (ev.Player == null || ev.Target == null)
                return;

            Types.V2Log log = new()
            {
                Header = Main.Instance.Config.ReportLog.Header,
                Separator = Main.Instance.Config.ReportLog.Separator,
                Log = Main.Instance.Config.ReportLog.Log
                    .Replace("{player}", ev.Player.Nickname)
                    .Replace("{target}", ev.Target.Nickname)
                    .Replace("{reason}", ev.Reason ?? "No reason"),
            };
            _ = SendCompV2Log(log, Main.Instance.Config.ReportLogChannel);

            log = new()
            {
                Header = Main.Instance.Config.ForAdminReportLog.Header,
                Separator = Main.Instance.Config.ForAdminReportLog.Separator,
                Log = Main.Instance.Config.ForAdminReportLog.Log
                    .Replace("{player}", ev.Player.Nickname)
                    .Replace("{playerid}", ev.Player.UserId)
                    .Replace("{target}", ev.Target.Nickname)
                    .Replace("{targetid}", ev.Target.UserId)
                    .Replace("{reason}", ev.Reason ?? "No reason"),
            };
            _ = SendCompV2Log(log, Main.Instance.Config.ForAdminReportLogChannel);
        }

        public static void Hurt(PlayerHurtEventArgs ev)
        {
            if (ev.Attacker == null || ev.Player == null || ev.Player.IsNpc || ev.Player.IsHost || ev.Attacker.IsNpc || ev.Attacker.IsHost)
                return;

            var damageText = ev.DamageHandler is StandardDamageHandler standardDamageHandler
                ? standardDamageHandler.Damage.ToString()
                : ev.DamageHandler?.GetType().Name ?? "Unknown";

            _ = SendLog(
                Main.Instance.Config.HurtLog
                    .Replace("{player}", ev.Player.Nickname)
                    .Replace("{playerid}", ev.Player.UserId)
                    .Replace("{attacker}", ev.Attacker.Nickname)
                    .Replace("{attackerid}", ev.Attacker.UserId)
                    .Replace("{damage}", damageText),
                Main.Instance.Config.HurtLogChannel);
        }

        public static void ThrewProjectile(PlayerThrewProjectileEventArgs ev)
        {
            if (ev.Player == null || ev.Player.IsNpc || ev.Player.IsHost || ev.Projectile == null || ev.Projectile.LastOwner != ev.Player)
                return;

            if (ev.Projectile is not (ExplosiveGrenadeProjectile or FlashbangProjectile or Scp018Projectile))
                return;

            _ = SendLog(
                Main.Instance.Config.GrenadeLog
                    .Replace("{player}", ev.Player.Nickname)
                    .Replace("{playerid}", ev.Player.UserId)
                    .Replace("{grenade}", ev.Projectile.Type.ToString()),
                Main.Instance.Config.GrenadeLogChannel);
        }

        public static void ChangedRole(PlayerChangedRoleEventArgs ev)
        {
            if (ev.Player == null || ev.Player.IsNpc || ev.Player.IsHost)
                return;

            _ = SendLog(
                Main.Instance.Config.ChangeRoleLog
                    .Replace("{player}", ev.Player.Nickname)
                    .Replace("{playerid}", ev.Player.UserId)
                    .Replace("{oldRole}", ev.OldRole.ToString())
                    .Replace("{newRole}", ev.NewRole.RoleTypeId.ToString()),
                Main.Instance.Config.ChangeRoleLogChannel);
        }
    }

    public static class ServerEvents
    {
        public static void CommandExecuted(CommandExecutedEventArgs ev)
        {
            var player = Player.Get(ev.Sender);
            if (player == null || player.IsNpc || player.IsHost)
                return;

            var argsText = ev.Arguments.Array == null
                ? string.Empty
                : string.Join(" ", ev.Arguments.Array, ev.Arguments.Offset, ev.Arguments.Count);

            _ = SendLog(
                Main.Instance.Config.CommandLog
                    .Replace("{player}", player.Nickname)
                    .Replace("{playerid}", player.UserId)
                    .Replace("{command}", $"{ev.Command.Command} {argsText}".Trim()),
                Main.Instance.Config.CommandLogChannel);
        }

        public static void RoundStarted()
        {
            _ = SendLog(Main.Instance.Config.RoundStartedLog, Main.Instance.Config.RoundStartedLogChannel);
        }

        public static void RoundEnded(RoundEndedEventArgs ev)
        {
            _ = SendLog(Main.Instance.Config.RoundEndedLog, Main.Instance.Config.RoundEndedLogChannel);
        }

        public static void RoundRestarted()
        {
            _ = SendLog(Main.Instance.Config.RoundRestartedLog, Main.Instance.Config.RoundRestartedLogChannel);
        }
    }

    private static async Task SendLog(string log, ulong channelId)
    {
        if (channelId == 0 || string.IsNullOrWhiteSpace(log))
            return;

        await LogLock.WaitAsync();
        try
        {
            var core = DSharpSCP.Main.Instance;
            var client = core?.Client;
            if (client == null)
            {
                Logger.Error("Discord bot is not started yet.");
                return;
            }

            var guild = client.GetGuild(core.DefaultGuild);
            var channel = guild?.GetTextChannel(channelId);
            if (channel == null)
                return;

            string timestampedLog = DateTime.Now.ToString("[HH:mm:ss] ") + SanitizeDiscordMentions(log);

            bool hasLastMessage = Main.LastLogMessages.TryGetValue(channelId, out var lastMsg);
            bool needsNewMessage = !hasLastMessage || lastMsg == null || (lastMsg.Content.Length + timestampedLog.Length + 1) >= 2000;

            if (needsNewMessage)
            {
                var sentMsg = await channel.SendMessageAsync(timestampedLog);
                Main.LastLogMessages[channelId] = sentMsg;
                return;
            }

            try
            {
                await lastMsg.ModifyAsync(props => props.Content = lastMsg.Content + "\n" + timestampedLog);
            }
            catch
            {
                var sentMsg = await channel.SendMessageAsync(timestampedLog);
                Main.LastLogMessages[channelId] = sentMsg;
            }
        }
        catch (Exception ex)
        {
            Logger.Error($"[Discord Log Error]: {ex}");
        }
        finally
        {
            LogLock.Release();
        }
    }

    private static async Task SendCompV2Log(Types.V2Log log, ulong channelId)
    {
        if (channelId == 0 || log == null)
            return;

        await LogLock.WaitAsync();
        try
        {
            var core = DSharpSCP.Main.Instance;
            var client = core?.Client;
            if (client == null)
            {
                Logger.Error("Discord bot is not started yet.");
                return;
            }

            var guild = client.GetGuild(core.DefaultGuild);
            var channel = guild?.GetTextChannel(channelId);
            if (channel == null)
                return;

            var sanitizedHeader = SanitizeDiscordMentions(log.Header);
            var sanitizedBody = SanitizeDiscordMentions(log.Log);

            MessageComponent components = log.Separator
                ? new ComponentBuilderV2()
                    .WithContainer(container => container
                        .WithTextDisplay(sanitizedHeader)
                        .WithSeparator(SeparatorSpacingSize.Small)
                        .WithTextDisplay(sanitizedBody))
                    .Build()
                : new ComponentBuilderV2()
                    .WithContainer(container => container
                        .WithTextDisplay(sanitizedHeader)
                        .WithTextDisplay(sanitizedBody))
                    .Build();

            await channel.SendMessageAsync(components: components);
        }
        catch (Exception ex)
        {
            Logger.Error($"Discord send log failed: {ex}");
        }
        finally
        {
            LogLock.Release();
        }
    }

    private static string SanitizeDiscordMentions(string text)
    {
        return (text ?? string.Empty)
            .Replace("@everyone", "`@everyone`")
            .Replace("@here", "`@here`");
    }
}
