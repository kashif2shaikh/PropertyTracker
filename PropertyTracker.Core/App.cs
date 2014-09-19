using System;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.ViewModels;

namespace PropertyTracker.Core
{
	public class App : MvxApplication
	{
		public App ()
		{
            CreatableTypes()
            .EndingWith("Service")
            .AsInterfaces()
            .RegisterAsLazySingleton();

			//Mvx.RegisterType<ICalculation, Calculation> ();

			// Bootstrap and load the initial root view.
			RegisterAppStart<PropertyTracker.Core.ViewModels.LoginViewModel>();
		}

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            base.LoadPlugins(pluginManager);
            //pluginManager.EnsurePluginLoaded<Acr.MvvmCross.Plugins.BarCodeScanner.PluginLoader>();
            //pluginManager.EnsurePluginLoaded<Acr.MvvmCross.Plugins.DeviceInfo.PluginLoader>();
            //pluginManager.EnsurePluginLoaded<Acr.MvvmCross.Plugins.Settings.PluginLoader>();
            pluginManager.EnsurePluginLoaded<Acr.MvvmCross.Plugins.UserDialogs.PluginLoader>();

            pluginManager.EnsurePluginLoaded<Cirrious.MvvmCross.Plugins.Messenger.PluginLoader>();
			pluginManager.EnsurePluginLoaded<Cirrious.MvvmCross.Plugins.DownloadCache.PluginLoader>();
			pluginManager.EnsurePluginLoaded<Cirrious.MvvmCross.Plugins.PictureChooser.PluginLoader> ();
        }
	}
}

