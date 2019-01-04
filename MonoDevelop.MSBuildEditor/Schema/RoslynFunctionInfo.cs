﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CodeAnalysis;
using MonoDevelop.Ide.TypeSystem;
using System.Linq;
using MonoDevelop.MSBuildEditor.Language;

namespace MonoDevelop.MSBuildEditor.Schema
{
	class RoslynFunctionInfo : FunctionInfo
	{
		readonly IMethodSymbol symbol;

		static string GetName (IMethodSymbol symbol)
		{
			if (symbol.MethodKind == MethodKind.Constructor)
				return "new";
			return symbol.Name;
		}

		public RoslynFunctionInfo (IMethodSymbol symbol) : base (GetName (symbol), null)
		{
			this.symbol = symbol;
		}

		public override DisplayText Description => new DisplayText (Ambience.GetSummaryMarkup (symbol), true);

		public override FunctionParameterInfo [] Parameters => symbol.Parameters.Select (p => new RoslynFunctionArgumentInfo (p)).ToArray ();
		public override MSBuildValueKind ReturnType => FunctionCompletion.ConvertType (symbol.GetReturnType ());
	}

	class RoslynFunctionArgumentInfo : FunctionParameterInfo
	{
		readonly IParameterSymbol symbol;

		public RoslynFunctionArgumentInfo (IParameterSymbol symbol) : base (symbol.Name, null)
		{
			this.symbol = symbol;
		}

		public override DisplayText Description => new DisplayText (Ambience.GetSummaryMarkup (symbol), true);
		public override string Type => string.Join (" ", FunctionCompletion.ConvertType (symbol.Type).GetTypeDescription ());
	}

	class RoslynClassInfo : ClassInfo
	{
		readonly ITypeSymbol symbol;

		public RoslynClassInfo (string name, ITypeSymbol symbol) : base (name, null)
		{
			this.symbol = symbol;
		}

		public override DisplayText Description => new DisplayText (Ambience.GetSummaryMarkup (symbol), true);
	}

	class RoslynPropertyInfo : FunctionInfo
	{
		readonly IPropertySymbol symbol;

		public RoslynPropertyInfo (IPropertySymbol symbol) : base (symbol.Name, null)
		{
			this.symbol = symbol;
		}

		public override DisplayText Description => new DisplayText (Ambience.GetSummaryMarkup (symbol), true);

		public override MSBuildValueKind ReturnType => FunctionCompletion.ConvertType (symbol.GetReturnType ());
		public override FunctionParameterInfo [] Parameters => null;
		public override bool IsProperty => true;
	}
}