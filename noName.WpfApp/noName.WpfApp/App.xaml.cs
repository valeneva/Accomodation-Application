// ----------------------------------------------------------------------
// <copyright file="MainModel.cs" company="noName">
//     Copyright (c) noName s. r. o..  All rights reserved.
// </copyright>
// ----------------------------------------------------------------------



namespace noName.WpfApp
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using noName.WpfApp.Model;
    using noName.WpfApp.ViewModel;
    using Serilog;
    using Serilog.Core;
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Members
        private Logger? _serviceLogger;
        private IServiceProvider? _serviceProvider;
        private Process? _flaskProcess;
        //private IConfigurationRoot? _clientConfiguration;
        #endregion

        #region Constructors


        public App()
        {
            //inicializace nastavení z appsetting.json
            var builder = new ConfigurationBuilder();
            var configurationFileName = $"appsettings.json";
            _ = builder.AddJsonFile(configurationFileName);

            var configurationRoot = builder.Build();

            // Načtení clientsettings.json
            var configurationClientName = "clientsettings.json";
            _ = builder.AddJsonFile(configurationClientName);

            var configurationClientRoot = builder.Build();

            var serviceCollection = new ServiceCollection();

            string[] paths;
            paths =  ConfigureServices(serviceCollection, configurationRoot, configurationClientRoot);

            string pythonPath = paths[0];
            string scriptPath = paths[1];

            _serviceProvider = serviceCollection.BuildServiceProvider();

            // Start Python server
            StartPythonServer(pythonPath, scriptPath);

        }

        #endregion

        #region Overrides

        /// <summary>
        /// Spuštěno při startu aplikace
        /// </summary>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);


            Debug.Assert(_serviceProvider != null);

            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            //nastavení datakontextu na viewModel
            MainWindow.DataContext = _serviceProvider.GetRequiredService<MainViewModel>();

            //otevření okna aplikace
            MainWindow.Show();

            Debug.Assert(_serviceLogger != null);
            _serviceLogger.Information("App started.");

        }

        /// <summary>
        /// Vypnutí serveru při vypnutí aplikace
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            // Zastavte Flask server při ukončení aplikace
            Debug.Assert(_serviceLogger != null);

            if (_flaskProcess != null && !_flaskProcess.HasExited)
            {
                try
                {
                    _serviceLogger.Information($"Killing Flask server process with ID: {_flaskProcess.Id}");
                    // zabití procesu
                    _flaskProcess.Kill();
                    _flaskProcess.WaitForExit(3000); // Wait for the process to exit
                    _serviceLogger.Information("Flask server process killed.\n\n\n\n");
                }
                catch (Exception ex)
                {
                    _serviceLogger.Error($"Exception occurred while killing Python server: {ex.Message}");
                }
            }
            else
            {
                _serviceLogger.Information("Flask server process is already exited or not started.");
            }
        }

        #endregion
        #region Methods

        private string[] ConfigureServices(
     IServiceCollection serviceCollection,
            IConfigurationRoot configurationRoot, IConfigurationRoot configurationClientRoot)
        {

            var serilogSection = configurationRoot.GetSection("serilog");

            _serviceLogger = new LoggerConfiguration()
                .ReadFrom.Configuration(configurationRoot)
                .CreateLogger();

            serviceCollection.AddLogging((loggingBuilder) =>
            {
                loggingBuilder.AddSerilog(_serviceLogger);
            });



            // Registrace HttpClient
            var clientSection = configurationClientRoot.GetSection("client");
            var baseUrl = clientSection.GetValue<string>("baseUrl");
            var timeoutSeconds = clientSection.GetValue<int>("timeoutSeconds");
            var pythonPath = clientSection.GetValue<string>("pythonPath");
            var scriptPath = clientSection.GetValue<string>("scriptPath");

            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new InvalidOperationException("Base URL is not configured.");
            }

            if (string.IsNullOrEmpty(pythonPath))
            {
                throw new InvalidOperationException("pythonPath is not configured.");
            }

            if (string.IsNullOrEmpty(scriptPath))
            {
                throw new InvalidOperationException("scriptPath is not configured.");
            }

            // Nastavení HttpClient
            serviceCollection.AddHttpClient<MainModel>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
            });


            _ = serviceCollection.AddSingleton<MainWindow>();
            _ = serviceCollection.AddSingleton<MainViewModel>();

            // add app
            _ = serviceCollection.AddSingleton(this);


            return [pythonPath, scriptPath];
        }


        /// <summary>
        /// Nastartuje Server pythonu flaskServer.py
        /// </summary>
        /// <remarks>
        /// RedirectStandardOutput:Popis: Pokud je nastavena na true, výstup ze standardního výstupu(stdout) procesu bude přesměrován, což umožní čtení výstupu programu z vašeho kódu.Příklad: Umožňuje číst výstup Flask serveru.
        /// RedirectStandardError:Popis: Pokud je nastavena na true, výstup ze standardního chybového výstupu(stderr) procesu bude přesměrován, což umožní čtení chybového výstupu programu z vašeho kódu.Příklad: Umožňuje číst chyby Flask serveru.
        /// UseShellExecute: Popis: Pokud je nastavena na false, proces bude spuštěn bez použití shellu.To je nutné, pokud chcete přesměrovat vstup nebo výstup procesu.Příklad: Umožňuje přesměrování vstupu a výstupu.
        /// CreateNoWindow:Popis: Pokud je nastavena na true, nově spuštěný proces nebude mít žádné okno.To je užitečné pro spuštění procesů na pozadí.Příklad: Flask server nebude mít žádné viditelné okno.
        /// </remarks>
        /// <param name="pythonPath"></param>
        /// <param name="scriptPath"></param>
        private void StartPythonServer(string pythonPath, string scriptPath)
        {
            //// nastavení vlastností procesu
            var startInfo = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = scriptPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                ErrorDialog = false,
                StandardErrorEncoding = Encoding.UTF8,
                StandardOutputEncoding = Encoding.UTF8,
            };

            ////přiřazení nastavení vlastností procesu
            _flaskProcess = new Process
            {
                StartInfo = startInfo,
                EnableRaisingEvents = true // To raise events when the process exits
                //EnableRaisingEvents: Tato vlastnost je nastavena na true, což umožňuje procesům vyvolávat události.
            };

            Debug.Assert(_serviceLogger != null);

            try
            {
                // Start the Flask process asynchronously
                _flaskProcess.Start();

            }
            catch (Exception ex)
            {
                _serviceLogger.Error($"Exception occurred while starting Python server: {ex.Message}");

            };


        }


        #endregion
    }

}
