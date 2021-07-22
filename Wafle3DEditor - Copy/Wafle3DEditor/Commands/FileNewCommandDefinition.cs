using Gemini.Framework.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wafle3DEditor.Commands
{
	[CommandDefinition]
	public class FileNewCommandDefinition : CommandDefinition
	{
		public const string CommandName = "File.New";

		public override string Name
		{
			get { return CommandName; }
		}

		public override string Text
		{
			get { return "_New"; }
		}

		public override string ToolTip
		{
			get { return "NEW"; }
		}
	}
}
