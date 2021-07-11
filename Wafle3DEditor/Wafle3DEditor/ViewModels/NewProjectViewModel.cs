using Gemini.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wafle3DEditor.ViewModels
{
    [Export(typeof(CreateProjectViewModel))]
    public class NewProjectViewModel : Document
    {

        private string _name;
        public override string DisplayName
        {
            get { return "New project"; }
        }

        public NewProjectViewModel(string projectPath)
        { 
            
        }
    }
}
