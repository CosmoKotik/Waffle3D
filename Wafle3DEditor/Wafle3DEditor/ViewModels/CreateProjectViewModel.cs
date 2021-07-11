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
    public class CreateProjectViewModel : Document
    {
        public override string DisplayName
        {
            get { return "New project"; }
        }

        private string _prjName;
        public string PrjName
        {
            get { return _prjName; }
            set
            {
                _prjName = value;
                NotifyOfPropertyChange(() => PrjName);
            }
        }

        private string _prjPath;
        public string PrjPath
        {
            get { return _prjPath; }
            set
            {
                _prjPath = value;
                NotifyOfPropertyChange(() => PrjPath);
            }
        }

        public CreateProjectViewModel()
        {
            _prjName = "123";
            _prjPath = "";
        }
    }
}
