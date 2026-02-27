using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;

namespace DSharpSCP.Logs;

public class EventHandlers
{
    private static readonly System.Threading.SemaphoreSlim LogLock = new System.Threading.SemaphoreSlim(1, 1);
    public static class PlayerEvents
    {
        public static void Joined(PlayerJoinedEventArgs e)
        {
            if (e.Player == null || e.Player.IsDummy || e.Player.IsNpc || e.Player.IsHost)
                return;
            _ = SendLog(Main.Instance.Config.JoinedLog.Replace("{player}", e.Player.Nickname).Replace("{steamid}", e.Player.UserId), Main.Instance.Config.JoinedLogChannelId);
        }

        public static void Banned(PlayerBannedEventArgs ev)
        {
            var now = DateTime.Now;
            var expires = now.AddSeconds(ev.Duration);
            var duration = expires - now;
            Types.V2Log log = Main.Instance.Config.BanLog;
            log.Log = log.Log.Replace("{player}", ev.Player.Nickname).Replace("{reason}", ev.Reason).Replace("{admin}", ev.Issuer.Nickname).Replace("{durationDays}", duration.Days.ToString()).Replace("{durationHours}", duration.Hours.ToString()).Replace("{durationMinutes}", duration.Minutes.ToString()).Replace("{expires}", ((DateTimeOffset)expires).ToUnixTimeSeconds().ToString());
            _ = SendCompV2Log(log, Main.Instance.Config.BanLogChannel);
            log = Main.Instance.Config.ForAdminBanLog;
            log.Log = log.Log.Replace("{player}", ev.Player.Nickname).Replace("{playerid}", ev.Player.UserId).Replace("{reason}", ev.Reason).Replace("{admin}", ev.Issuer.Nickname).Replace("{adminid}", ev.Issuer.UserId).Replace("{durationDays}", duration.Days.ToString()).Replace("{durationHours}", duration.Hours.ToString()).Replace("{durationMinutes}", duration.Minutes.ToString()).Replace("{expires}", ((DateTimeOffset)expires).ToUnixTimeSeconds().ToString());
            _ = SendCompV2Log(log, Main.Instance.Config.ForAdminBanLogChannel);
        }

        public static void Left(PlayerLeftEventArgs ev)
        {
            if (ev.Player == null || ev.Player.IsDummy || ev.Player.IsNpc || ev.Player.IsHost)
                return;
            _ = SendLog(Main.Instance.Config.LeftLog.Replace("{player}", ev.Player.Nickname).Replace("{steamid}", ev.Player.UserId), Main.Instance.Config.LeftLogChannelId);
        }

        public static void Muted(PlayerMutedEventArgs ev)
        {
            Types.V2Log log = Main.Instance.Config.MuteLog;
            log.Log = log.Log.Replace("{player}", ev.Player.Nickname).Replace("{admin}", ev.Issuer.Nickname);
            _ = SendCompV2Log(log, Main.Instance.Config.MuteLogChannel);
            log = Main.Instance.Config.ForAdminMuteLog;
            log.Log = log.Log.Replace("{player}", ev.Player.Nickname).Replace("{playerid}", ev.Player.UserId).Replace("{admin}", ev.Issuer.Nickname).Replace("{adminid}", ev.Issuer.UserId);
            _ = SendCompV2Log(log, Main.Instance.Config.ForAdminMuteLogChannel);
        }

        public static void Death(PlayerDeathEventArgs ev)
        {
            if (ev.Attacker == null || ev.Player == null || ev.Player.IsNpc || ev.Player.IsHost || ev.Attacker.IsNpc || ev.Attacker.IsHost)
                return;
            _ = SendLog(Main.Instance.Config.KillLog.Replace("{player}", ev.Attacker.Nickname).Replace("{playerid}", ev.Attacker.UserId).Replace("{target}", ev.Player.Nickname).Replace("{targetid}", ev.Player.UserId).Replace("{reason}", ev.DamageHandler.DeathScreenText), Main.Instance.Config.KillLogChannel);
        }

        public static void Cuffed(PlayerCuffedEventArgs ev)
        {
            if (ev.Target == null || ev.Player == null || ev.Player.IsNpc || ev.Player.IsHost || ev.Target.IsNpc || ev.Target.IsHost)
                return;
            _ = SendLog(Main.Instance.Config.CuffLog.Replace("{player}", ev.Player.Nickname).Replace("{playerid}", ev.Player.UserId).Replace("{target}", ev.Target.Nickname).Replace("{targetid}", ev.Target.UserId), Main.Instance.Config.CuffLogChannel);
        }

        public static void ReportedPlayer(PlayerReportedPlayerEventArgs ev)
        {
            Types.V2Log log = Main.Instance.Config.ReportLog;
            log.Log = log.Log.Replace("{player}", ev.Player.Nickname).Replace("{target}", ev.Target.Nickname).Replace("{reason}", ev.Reason);
            _ = SendCompV2Log(log, Main.Instance.Config.ReportLogChannel);
            log = Main.Instance.Config.ForAdminReportLog;
            log.Log = log.Log.Replace("{player}", ev.Player.Nickname).Replace("{playerid}", ev.Player.UserId).Replace("{target}", ev.Target.Nickname).Replace("{targetid}", ev.Target.UserId).Replace("{reason}", ev.Reason);
            _ = SendCompV2Log(log, Main.Instance.Config.ForAdminReportLogChannel);
        }

        public static void Hurt(PlayerHurtEventArgs ev)
        {
            if (ev.Attacker == null || ev.Player == null || ev.Player.IsNpc || ev.Player.IsHost || ev.Attacker.IsNpc || ev.Attacker.IsHost)
                return;
            _ = SendLog(Main.Instance.Config.HurtLog.Replace("{player}", ev.Player.Nickname).Replace("{playerid}", ev.Player.UserId).Replace("{attacker}", ev.Attacker.Nickname).Replace("{attackerid}", ev.Attacker.UserId).Replace("{damage}", ((StandardDamageHandler)ev.DamageHandler).Damage.ToString()), Main.Instance.Config.HurtLogChannel);
        }

        public static void ThrewProjectile(PlayerThrewProjectileEventArgs ev)
        {
            if (ev.Player == null || ev.Player.IsNpc || ev.Player.IsHost || ev.Projectile.Type == null || ev.Projectile.Type == ItemType.None || ev.Projectile is not (ExplosiveGrenadeProjectile or FlashbangProjectile or Scp018Projectile) || ev.Projectile.LastOwner != ev.Player)
                return;
            _ = SendLog(Main.Instance.Config.GrenadeLog.Replace("{player}", ev.Player.Nickname).Replace("{playerid}", ev.Player.UserId).Replace("{grenade}", ev.Projectile.Type.ToString()), Main.Instance.Config.GrenadeLogChannel);
        }

        public static void ChangedRole(PlayerChangedRoleEventArgs ev)
        {
            if (ev.Player.IsNpc || ev.Player.IsHost || ev.Player == null)
                return;
            _ = SendLog(Main.Instance.Config.ChangeRoleLog.Replace("{player}", ev.Player.Nickname).Replace("{playerid}", ev.Player.UserId).Replace("{oldRole}", ev.OldRole.ToString()).Replace("{newRole}", ev.NewRole.RoleTypeId.ToString()), Main.Instance.Config.ChangeRoleLogChannel);
        }
    }

    public static class ServerEvents
    {
        public static void CommandExecuted(CommandExecutedEventArgs ev)
        {
            if (Player.Get(ev.Sender) == null || Player.Get(ev.Sender).IsNpc|| Player.Get(ev.Sender).IsHost)
                return;
            _ = SendLog(Main.Instance.Config.CommandLog.Replace("{player}", Player.Get(ev.Sender).Nickname).Replace("{playerid}", Player.Get(ev.Sender).UserId).Replace("{command} ", ev.Command.Command  + string.Join(" ", ev.Arguments.Array, ev.Arguments.Offset, ev.Arguments.Count)), Main.Instance.Config.CommandLogChannel);
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
        if (channelId == 0) return;

        // Ждем своей очереди, чтобы не спамить запросами одновременно
        await LogLock.WaitAsync();
        try
        {
            var guild = DSharpSCP.Main.Instance.Client.GetGuild(DSharpSCP.Main.Instance.DefaultGuild);
            var channel = guild?.GetTextChannel(channelId);
            if (channel == null) return;

            string timestampedLog = DateTime.Now.ToString("[HH:mm:ss] ") + log;

            // Проверяем, нужно ли создать новое сообщение или дополнить старое
            bool hasLastMessage = Main.lastLogMessages.TryGetValue(channel, out var lastMsg);
            bool needsNewMessage = !hasLastMessage || lastMsg == null || (lastMsg.Content.Length + log.Length + 2) >= 2000;

            if (needsNewMessage)
            {
                var sentMsg = await channel.SendMessageAsync(timestampedLog);
                Main.lastLogMessages[channel] = sentMsg;
            }
            else
            {
                // Пытаемся редактировать. Если сообщение удалили — создаем новое.
                try 
                {
                    await lastMsg.ModifyAsync(p => p.Content = lastMsg.Content + "\n" + timestampedLog);
                }
                catch
                {
                    var sentMsg = await channel.SendMessageAsync(timestampedLog);
                    Main.lastLogMessages[channel] = sentMsg;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error($"[Discord Log Error]: {ex.Message}");
        }
        finally
        {
            // Обязательно освобождаем блокировку
            LogLock.Release();
        }
    }

    
    private static async Task SendCompV2Log(Types.V2Log log, ulong  channelId)
    {
        if (channelId == 0)
        {
            return;
        }
        MessageComponent components;
        var channel = DSharpSCP.Main.Instance.Client.GetGuild(DSharpSCP.Main.Instance.DefaultGuild).GetTextChannel(channelId);
        if (log.Separator)
        {
            components = new ComponentBuilderV2()
                .WithContainer(container => container
                    .WithTextDisplay($"{log.Header}")
                    .WithSeparator(SeparatorSpacingSize.Small)
                    .WithTextDisplay(
                        $"{log.Log}"
                    )
                )
                .Build();
        }
        else
        {
            components = new ComponentBuilderV2()
                .WithContainer(container => container
                    .WithTextDisplay($"{log.Header}")
                    .WithTextDisplay(
                        $"{log.Log}"
                    )
                )
                .Build();
        }
        try
        {
            await channel.SendMessageAsync(components: components);
        }
        catch 
        {
            await Task.Delay(1000);
            try
            {
                await channel.SendMessageAsync(components: components);
            }
            catch (Exception ex)
            {
                Logger.Error($"Discord send log failed: {ex}");
            }
        }
    }
}