using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wafle3DEditor.ViewModels;

namespace Wafle3DEditor.Commands
{
    [CommandHandler]
    public class FileNewCommandHandler : CommandHandlerBase<FileNewCommandDefinition>
    {
        private IShell _shell;

        [ImportingConstructor]
        public FileNewCommandHandler(IShell shell)
        {
            _shell = shell;
        }

        public override Task Run(Command command)
        {
            CrPrjMenu cpm = new CrPrjMenu(_shell);
            cpm.Show();

            //_shell.OpenDocument(new CreateProjectViewModel());
            return TaskUtility.Completed;
        }
    }
}
