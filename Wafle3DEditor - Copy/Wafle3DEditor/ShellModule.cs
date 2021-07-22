using Caliburn.Micro;
using Gemini.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid;
using System.ComponentModel.Composition;
using Gemini.Framework.Services;
using Gemini.Framework.Menus;
using Gemini.Modules.MainMenu;
using Wafle3DEditor.Commands;
using Wafle3DEditor.ViewModels;

namespace Wafle3DEditor
{
    [Export(typeof(IModule))]
    public class ShellModule : ModuleBase
    {
        [Export]
        public static ExcludeMenuItemDefinition RemoveNewMenu = new ExcludeMenuItemDefinition(Gemini.Modules.Shell.MenuDefinitions.FileNewMenuItem);

        [Export]
        public static MenuItemGroupDefinition FileMenuItem = new MenuItemGroupDefinition(
                    Gemini.Modules.MainMenu.MenuDefinitions.FileMenu, 0);

        [Export]
        public static MenuItemDefinition NewItem = new CommandMenuItemDefinition<FileNewCommandDefinition>(
            FileMenuItem, 0);

        [Import]
        public IMainWindow MainWindow;

        [Export]
        public static MenuBarDefinition MainMenuBar;

        public override void Initialize()
        {
            //Shell.ShowTool(new HierarchyViewModel());
            MainWindow.Title = "Wafle3D Editor";

            base.Initialize();
        }
    }

}
