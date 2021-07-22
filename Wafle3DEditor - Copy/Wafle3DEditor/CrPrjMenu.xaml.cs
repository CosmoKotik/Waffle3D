using Gemini.Framework.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Wafle3DEditor.ViewModels;

namespace Wafle3DEditor
{
    /// <summary>
    /// Interaction logic for CrPrjMenu.xaml
    /// </summary>
    public partial class CrPrjMenu : Window
    {
        public enum GameTemplate { Standart, HDG, FPS, Empty }
        public GameTemplate CurrentTemplate { get; set; }

        private IShell _shell;
        
        private string _name;
        private string _path;

        private bool _isNameDeletedTxt = false;
        private bool _isPathDeletedTxt = false;

        public CrPrjMenu(IShell shell)
        {
            InitializeComponent();

            _shell = shell;
        }

        private void PrjName_GotFocus(object sender, RoutedEventArgs e)
        {
            DoubleAnimation sizeAnim = new DoubleAnimation(240, 260, TimeSpan.FromSeconds(0.1d));
            DoubleAnimation opacityAnim = new DoubleAnimation(0.5, 1, TimeSpan.FromSeconds(0.1d));
            ColorAnimation colorAnim = new ColorAnimation(Colors.Black ,Color.FromRgb(46, 123, 255), TimeSpan.FromSeconds(0.1d));

            SolidColorBrush bgrClr = new SolidColorBrush();
            bgrClr.BeginAnimation(SolidColorBrush.ColorProperty, colorAnim);
            PrjNameBorder.Background = bgrClr;

            PrjName.BeginAnimation(Grid.WidthProperty, sizeAnim);
            PrjNameBorder.BeginAnimation(Grid.WidthProperty, sizeAnim);
            PrjNameBorder.BeginAnimation(Grid.OpacityProperty, opacityAnim);

            if (!_isNameDeletedTxt)
                PrjName.Text = "";
            _isNameDeletedTxt = true;
        }

        private void PrjName_LostFocus(object sender, RoutedEventArgs e)
        {
            DoubleAnimation sizeAnim = new DoubleAnimation(260, 240, TimeSpan.FromSeconds(0.1d));
            DoubleAnimation opacityAnim = new DoubleAnimation(1, 0.5, TimeSpan.FromSeconds(0.1d));
            ColorAnimation colorAnim = new ColorAnimation(Color.FromRgb(46, 123, 255), Colors.Black, TimeSpan.FromSeconds(0.1d));
            
            SolidColorBrush bgrClr = new SolidColorBrush();
            bgrClr.BeginAnimation(SolidColorBrush.ColorProperty, colorAnim);
            PrjNameBorder.Background = bgrClr;

            PrjName.BeginAnimation(Grid.WidthProperty, sizeAnim);
            PrjNameBorder.BeginAnimation(Grid.WidthProperty, sizeAnim);
            PrjNameBorder.BeginAnimation(Grid.OpacityProperty, opacityAnim);

            _name = PrjName.Text;
        }

        private void PrjPath_GotFocus(object sender, RoutedEventArgs e)
        {
            DoubleAnimation sizeAnim = new DoubleAnimation(240, 260, TimeSpan.FromSeconds(0.1d));
            DoubleAnimation opacityAnim = new DoubleAnimation(0.5, 1, TimeSpan.FromSeconds(0.1d));
            ColorAnimation colorAnim = new ColorAnimation(Colors.Black, Color.FromRgb(46, 123, 255), TimeSpan.FromSeconds(0.1d));

            SolidColorBrush bgrClr = new SolidColorBrush();
            bgrClr.BeginAnimation(SolidColorBrush.ColorProperty, colorAnim);
            PrjPathBorder.Background = bgrClr;

            PrjPath.BeginAnimation(Grid.WidthProperty, sizeAnim);
            PrjPathBorder.BeginAnimation(Grid.WidthProperty, sizeAnim);
            PrjPathBorder.BeginAnimation(Grid.OpacityProperty, opacityAnim);

            if (!_isPathDeletedTxt)
                PrjPath.Text = "";
            _isPathDeletedTxt = true;
        }

        private void PrjPath_LostFocus(object sender, RoutedEventArgs e)
        {
            DoubleAnimation sizeAnim = new DoubleAnimation(260, 240, TimeSpan.FromSeconds(0.1d));
            DoubleAnimation opacityAnim = new DoubleAnimation(1, 0.5, TimeSpan.FromSeconds(0.1d));
            ColorAnimation colorAnim = new ColorAnimation(Color.FromRgb(46, 123, 255), Colors.Black, TimeSpan.FromSeconds(0.1d));

            SolidColorBrush bgrClr = new SolidColorBrush();
            bgrClr.BeginAnimation(SolidColorBrush.ColorProperty, colorAnim);
            PrjPathBorder.Background = bgrClr;

            PrjPath.BeginAnimation(Grid.WidthProperty, sizeAnim);
            PrjPathBorder.BeginAnimation(Grid.WidthProperty, sizeAnim);
            PrjPathBorder.BeginAnimation(Grid.OpacityProperty, opacityAnim);

            _path = PrjPath.Text;
        }

        private void StandartBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentTemplate = GameTemplate.Standart;
            CreateBtn.IsEnabled = true;
        }

        private void HDGrphBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentTemplate = GameTemplate.HDG;
            CreateBtn.IsEnabled = true;
        }

        private void FPSBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentTemplate = GameTemplate.FPS;
            CreateBtn.IsEnabled = true;
        }

        private void StandartBtn_LostFocus(object sender, RoutedEventArgs e)
        {
            CurrentTemplate = GameTemplate.Empty;
            //CreateBtn.IsEnabled = false;
        }

        private void HDGrphBtn_LostFocus(object sender, RoutedEventArgs e)
        {
            CurrentTemplate = GameTemplate.Empty;
            //CreateBtn.IsEnabled = false;
        }

        private void FPSBtn_LostFocus(object sender, RoutedEventArgs e)
        {
            CurrentTemplate = GameTemplate.Empty;
            //CreateBtn.IsEnabled = false;
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            //var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NewProjectSetting.xml");
            _shell.ShowTool(new HierarchyViewModel());
            _shell.OpenDocument(new SceneViewModel(""));
            _shell.OpenDocument(new GameViewModel(""));

            this.Hide();
        }
    }
}
