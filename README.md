# D2E Enhancer

![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/r0ute/d2e-enhancer/dotnet.yml)
![GitHub last commit](https://img.shields.io/github/last-commit/r0ute/d2e-enhancer)
![GitHub Release Date](https://img.shields.io/github/release-date/r0ute/d2e-enhancer)

**D2E Enhancer** is a [BepInEx](https://docs.bepinex.dev) plugin for the game _Dust to the End_. It enriches gameplay by providing a more informative map interface, detailed city information, and improved trade tracking.

## Features

- **Map UI Enhancements**:
  - Reveal cities, dialog starters, radiation zones, and relics on the map for easier navigation and discovery
  - Display building names within cities to provide a clearer view of available facilities
- **Trade Information Enhancements**:
  - Increase the trade log size limit, allowing for better tracking of recent transactions
- **Relic Insights**:
  - View a complete list of all specific products that a relic can produce before exploring it
- **Quick Save & Load Functionality**:
  - Quick save and load with F5 and F9 keys, featuring a configurable quick save slot option
- **Event Notifications**:
  - Disable level-up notifications for team members who are assigned to the back row non-combat slots
- **Keyboard Configuration**:
  - Allow configuration of additional keys for inventory and map with plugin support (defaults: Mouse3 and Mouse4)


## Getting Started

To start developing with BepInEx, please refer to the official setup guide [here](https://docs.bepinex.dev/articles/dev_guide/plugin_tutorial/1_setup.html).

## Building the Project

To build this project using the .NET CLI, follow these steps:

1. Open a command line prompt in the project folder.
2. Run the following command:

   ```bash
   dotnet build
   ```

## Output

After a successful build, you can find the plugin at `Debug\net46\com.bepinex.plugins.d2e.enhancer.dll`
