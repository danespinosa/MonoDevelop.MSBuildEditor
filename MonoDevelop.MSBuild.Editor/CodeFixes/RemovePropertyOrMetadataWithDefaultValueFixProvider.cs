// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

using MonoDevelop.MSBuild.Analysis;
using MonoDevelop.MSBuild.Language;
using MonoDevelop.MSBuild.Schema;
using MonoDevelop.Xml.Dom;

namespace MonoDevelop.MSBuild.Editor.CodeFixes
{
	[Export (typeof (MSBuildFixProvider))]
	class RemovePropertyOrMetadataWithDefaultValueFixProvider : MSBuildFixProvider
	{
		public override ImmutableArray<string> FixableDiagnosticIds { get; }
			= ImmutableArray.Create (CoreDiagnostics.HasDefaultValue.Id);

		public override Task RegisterCodeFixesAsync (MSBuildFixContext context)
		{
			foreach (var diag in context.Diagnostics) {
				if (!(diag.Properties.TryGetValue ("Info", out var val) && val is ValueInfo info)) {
					continue;
				}

				switch (context.XDocument.FindAtOffset (diag.Span.Start)) {
				case XElement el:
					context.RegisterCodeFix (new RemoveRedundantElementAction (el, DescriptionFormatter.GetKindNoun (info)), diag);
					break;
				case XAttribute att:
					context.RegisterCodeFix (new RemoveRedundantAttributeAction (att, DescriptionFormatter.GetKindNoun (info)), diag);
					break;
				}
			}
			return Task.CompletedTask;
		}

		class RemoveRedundantElementAction : SimpleMSBuildAction
		{
			readonly XElement element;
			readonly string kindNoun;
			public RemoveRedundantElementAction (XElement prop, string kindNoun)
			{
				this.element = prop;
				this.kindNoun = kindNoun;
			}
			public override string Title => $"Remove redundant {kindNoun} '{element.Name}'";
			protected override MSBuildActionOperation CreateOperation () => new EditTextActionOperation ().RemoveElement (element);
		}

		class RemoveRedundantAttributeAction : SimpleMSBuildAction
		{
			readonly XAttribute att;
			readonly string kindNoun;
			public RemoveRedundantAttributeAction (XAttribute att, string kindNoun)
			{
				this.att = att;
				this.kindNoun = kindNoun;
			}
			public override string Title => $"Remove redundant {kindNoun} '{att.Name}'";
			protected override MSBuildActionOperation CreateOperation () => new EditTextActionOperation ().RemoveAttribute (att);
		}
	}
}
