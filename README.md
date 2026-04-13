[![Downloads](https://img.shields.io/github/downloads/LigindaLeg/DSharpSCP/total?color=brightgreen&label=downloads)](https://github.com/LigindaLeg/DSharpSCP/releases)  
[![Releases](https://img.shields.io/github/v/release/LigindaLeg/DSharpSCP?color=orange&label=GitHub+Releases)](https://github.com/LigindaLeg/DSharpSCP/releases)

# DSharpSCP

A modular Discord integration suite for **SCP: Secret Laboratory** built on **LabAPI**.

`DSharpSCP` connects your server to Discord, `DSharpSCP.Logs` sends game events to Discord channels, and `DSharpSCP.Status` updates the bot presence based on the current server population.

## Features

### Core module (`DSharpSCP`)
- Starts and manages a Discord bot client
- Connects using a bot token and a default guild ID
- Supports debug logging
- Provides the shared Discord client used by the other modules

### Logs module (`DSharpSCP.Logs`)
Sends server activity to Discord channels, including:
- player join / leave
- bans
- mutes
- kills
- cuffs
- reports
- remote admin commands
- round start / end / restart
- damage events
- grenade throws
- role changes

It also supports:
- separate public and admin log formats for moderation events
- configurable message templates
- configurable channel IDs
- Discord component-based log messages for richer moderation logs

### Status module (`DSharpSCP.Status`)
Updates the Discord bot status when players join or leave:
- custom status when the server is empty
- custom status when players are online
- optional idle status while the server is empty

## Project structure

```text
DSharpSCP/
├─ DSharpSCP/         # Core Discord bot module
├─ DSharpSCP.Logs/    # Discord logging module
└─ DSharpSCP.Status/  # Discord presence/status module
```

## Requirements

- **SCP: Secret Laboratory** dedicated server
- **LabAPI**
- **.NET Framework 4.8**
- A **Discord bot token**
- Your **Discord server (guild) ID**
- Proper bot permissions for the channels you want to use

## Installation

1. Build the projects.
2. Put the compiled DLLs into your server's plugin directory.
3. Make sure the core module is loaded:
   - `DSharpSCP`
4. Add optional modules as needed:
   - `DSharpSCP.Logs`
   - `DSharpSCP.Status`
5. Start the server and configure the generated config files.

## Configuration

### Core config (`DSharpSCP`)
```yaml
discord_bot_token: "your-bot-token"
discord_guild_id: 123456789012345678
debug: false
```

### Status config (`DSharpSCP.Status`)
```yaml
empty_status: "Nobody on server!"
server_status: "{players}/{max} on server."
idle: true
debug: false
```

### Logs config (`DSharpSCP.Logs`)
Every log type has:
- a message template
- a Discord channel ID

Set a channel ID to `0` to disable that log.

Example:
```yaml
joined_log: "Player {player} ({steamid}) joined the server"
joined_log_channel_id: 123456789012345678

left_log: "Player {player} ({steamid}) left the server"
left_log_channel_id: 123456789012345678

command_log: "Player {player} ({playerid}) issued command `{command}`"
command_log_channel: 123456789012345678
```

Moderation logs use structured templates with:
- `header`
- `separator`
- `log`

Example:
```yaml
ban_log:
  header: "Player Banned!"
  separator: true
  log: "**Player:** {player}\n**Reason:** {reason}\n**Admin:** {admin}\n**Duration:** {durationDays} d. {durationHours} h. {durationMinutes} m.\n**Expires:** {expires}"
```

## Available placeholders

Depending on the event, the following placeholders are used by the plugin:

- `{player}`
- `{playerid}`
- `{steamid}`
- `{target}`
- `{targetid}`
- `{admin}`
- `{adminid}`
- `{reason}`
- `{command}`
- `{damage}`
- `{grenade}`
- `{oldRole}`
- `{newRole}`
- `{durationDays}`
- `{durationHours}`
- `{durationMinutes}`
- `{expires}`
- `{players}`
- `{max}`

## How it works

- The core module creates a shared `DiscordSocketClient`.
- The logs module subscribes to LabAPI events and forwards them to Discord.
- The status module updates the bot presence based on current online players.
- The logs module can append plain text logs into the same Discord message until the message approaches Discord's length limit, reducing channel spam.

## Notes

- `DSharpSCP.Logs` and `DSharpSCP.Status` depend on the core `DSharpSCP` module.
- The bot must have access to the configured guild and channels.
- For moderation or command logs, make sure the bot has permission to send messages in the target channels.
- Debug mode is useful during setup, but it is usually better to keep it disabled in production.

## Example use cases

- Send join/leave logs to a public staff-log channel
- Send bans, mutes, and reports to a private admin-log channel
- Show live player count in the bot custom status
- Track round flow and player activity directly from Discord

## Author

**liginda**
