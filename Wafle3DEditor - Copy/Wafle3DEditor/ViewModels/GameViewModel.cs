using Gemini.Framework;
using Gemini.Framework.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wafle3DEditor.ViewModels
{
    [Export(typeof(GameViewModel))]
    public class GameViewModel : PersistedDocument
    {

        private string _name;
        public override string DisplayName
        {
            get { return "Game"; }
        }

        public override object GetView(object context = null)
        {
            return PaneLocation.Bottom;
        }

        public GameViewModel(string projectPath)
        {
            
        }
            
        protected override Task DoNew()
        {
            throw new NotImplementedException();
        }

        protected override Task DoLoad(string filePath)
        {
            throw new NotImplementedException();
        }

        protected override Task DoSave(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
