# QuakeReloaded
<p align="center">A library that provides API to interface <a href="https://github.com/Reloaded-Project/Reloaded-II">Reloaded II</a> mods with Quake Enhanced</p>
<p align="center">
  <img width="256" height="256" alt="Logo" src="https://github.com/jpiolho/QuakeReloaded/blob/main/QuakeReloaded/Preview.png">
</p>

<p align="center">This is just a base mod that contains a lot of useful code for other mods. See of a list of mods using QuakeReloaded <a href="https://jpiolho.github.io/QuakeReloaded/mods.html">here</a></p>

## For players
* Install [Reloaded-II](https://github.com/Reloaded-Project/Reloaded-II) if you don't have it
* Click <a href="https://jpiolho.github.io/QuakeReloaded/installmod.html?username=jpiolho&repo=QuakeReloaded&file=QuakeReloaded{tag}.7z&latestVersion=1" target="_blank">**here**</a> to install the mod

#### Manual install
* Head over to the [Releases](https://github.com/jpiolho/QuakeReloaded/releases) and download the latest QuakeReloaded 7z file (NOT the interfaces file)
* Extract the 7zip file into `<Reloaded II path>/Mods/QuakeReloaded` (alternatively: `%RELOADEDIIMODS%/QuakeReloaded`)
* Make sure it shows up in the Reloaded mod list

## For modders

### How to create a mod using QuakeReloaded
1. Setup a project with the Reloaded template. You can find [documentation here](https://reloaded-project.github.io/Reloaded-II/DevelopmentEnvironmentSetup/)
2. In your project, include [QuakeReloaded.Interfaces nuget package](https://www.nuget.org/packages/QuakeReloaded.Interfaces).
3. Edit your ModConfig.json:
   1. Set the `ModID` with the following name convention (remember all lower case and no spaces): `quakeenhanced.mod.<your mod name>`
   2. Add `quakeenhanced.mod.quakereloaded` to `ModDependencies`
   3. Add `quake_x64_steam.exe` to `SupportedAppId`
4. To get access to QuakeReloaded API use the following code in your Mod constructor:
   ```csharp
   if (!(_modLoader.GetController<IQuakeReloaded>()?.TryGetTarget(out var qreloaded) ?? false))
       throw new Exception("Could not get QuakeReloaded API. Are you sure QuakeReloaded is installed & loaded before this mod?");
   ```
5. Refer to [other mods](https://jpiolho.github.io/QuakeReloaded/mods.html) source code and the [QuakeReloaded API documentation](https://jpiolho.github.io/QuakeReloaded/api.html).
