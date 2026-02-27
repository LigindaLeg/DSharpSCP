# DSharpSCP

[![Version](https://img.shields.io/github/v/release/LigindaLeg/DSharpSCP?color=blue&label=version)](https://github.com/YourGitHubUsername/DSharpSCP/releases)  
[![Downloads](https://img.shields.io/github/downloads/LigindaLeg/DSharpSCP/total?color=brightgreen&label=downloads)](https://github.com/YourGitHubUsername/DSharpSCP/releases)  
[![Releases](https://img.shields.io/github/v/release/LigindaLeg/DSharpSCP?color=orange&label=GitHub+Releases)](https://github.com/YourGitHubUsername/DSharpSCP/releases)

**DSharpSCP** — Discord bot integration plugin for **SCP: Secret Laboratory** using LabAPI. The bot starts automatically alongside the plugin and allows logging, status updates, and modular features for your server.

---

## 📦 Features

- Automatic Discord bot startup with the plugin  
- Modular system: load additional modules from separate releases  
- Configurable logging and status modules  
- Supports Discord embed components for logs  
- Works on Linux and Windows servers  

---

## ⚙ Installation

### 1. Copy the plugin

Place the main plugin file in:
LabApi/plugins/(port/global)

### 2. Copy optional modules

Modules/releases also go into the same folder:
LabApi/plugins/(port/global)

Modules include:

- `DSharpSCP.Status` → manages player count server status updates  
- `DSharpSCP.Logs` → handles event logging to Discord  

---

### 3. Dependencies

Unpack `dependencies.zip` to:
LabApi/dependencies/(port/global)

This ensures the bot has all required DLLs to run.

---

## 🛠 Configuration

The main plugin config is located at:
LabApi/configs/(port)/DSharpSCP

Module configs:
LabApi/configs/(port)/DSharpSCP.Status
LabApi/configs/(port)/DSharpSCP.Logs

Each config can be customized to set:

- Discord bot token  
- Guild ID  
- Channel IDs for logs  
- Status settings  

---

## 🚀 Usage

- The Discord bot starts automatically when the plugin is loaded.  
- Logs and status updates are sent to Discord based on your configured channels.  
- For stable operation on Linux, consider running the bot in a separate process if needed for large servers.  

---

## 📂 Directory Structure
LabApi/
│
├─ plugins/(port/global)/ <- Plugin and modules go here
│ ├─ DSharpSCP.dll
│ ├─ DSharpSCP.Status.dll
│ └─ DSharpSCP.Logs.dll
│
├─ dependencies/(port/global)/ <- Extract dependencies.zip here
│ └─ *.dll
│
└─ configs/(port)/ <- Plugin configs

---

## 📌 Notes

- Modules are optional; the main bot plugin runs without them.  
- Do not block the main server thread with long-running async operations — the plugin handles async tasks internally.  
- Ensure your Discord bot token has the correct intents enabled (Message Content Intent, Server Members Intent if needed).  

---

## 📄 License

[MIT License](https://opensource.org/licenses/MIT)
