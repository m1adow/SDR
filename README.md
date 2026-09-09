# SDR

SDR is a Windows desktop app for visualizing simulated signal data. It generates
random signal strengths across a frequency range and displays them in two views:

- A spectrum plot showing the current signal strength at each frequency.
- A waterfall showing how signal strength changes over time, using color.

The app uses generated data; no radio hardware is needed. The current data provider
does not receive real radio signals.

## Run the app

You need Windows and Visual Studio with .NET 8, WinUI development support, and the
Windows SDK. The app uses WinUI 3 and the Windows App SDK.

1. Open `SDR.sln` in Visual Studio.
2. Set `SDR` as the startup project.
3. Select **Debug** and the platform for your machine (`x64`, `x86`, or `ARM64`).
4. Select the **SDR (Package)** launch profile and allow NuGet packages to restore.
5. Press **F5** to build and launch.
6. Click **Start** to generate signals. Click **Stop** to pause updates.

Use the packaged profile because the app loads its settings from the installed
package location.

## Change the simulated signal

For a Debug build, edit [SDR/appsettings.Development.json](SDR/appsettings.Development.json),
then rebuild and restart the app. These settings override
[SDR/appsettings.json](SDR/appsettings.json).

| Setting | Debug value | What it controls |
| --- | --- | --- |
| `FrequencyMin` | `90` | Lower end of the displayed frequency range. |
| `FrequencyMax` | `110` | Upper end of the displayed frequency range. |
| `StrengthMin` | `-120` | Lower bound used when generating signal strengths. |
| `StrengthMax` | `-20` | Upper bound used when generating signal strengths. |
| `Count` | `1024` | Target number of frequency points per update. |
| `Frequency` | `20` | Number of signal updates per second. |

`Frequency` is the update rate, separate from the frequency range shown on the plot.
Keep `Count` and `Frequency` positive, and each minimum below its maximum.

Release builds only load `appsettings.json`. Its `DataProviders.Random` section is
currently empty, so copy the values from the development file into that section
before building Release.

## Find your way around the code

| Project | Purpose |
| --- | --- |
| `SDR` | App window, configuration, and spectrum and waterfall controls drawn with Win2D. |
| `SDR.Models` | Signal data types, settings, collections, and data provider interfaces. |
| `SDR.Services` | Random signal generation. |
| `SDR.ViewModels` | Connects incoming signals and the Start/Stop commands to the interface. |

## License

This project is licensed under the [MIT License](LICENSE).
