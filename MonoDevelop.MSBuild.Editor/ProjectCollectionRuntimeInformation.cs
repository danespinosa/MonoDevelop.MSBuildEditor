// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;

using MonoDevelop.MSBuild.Language;
using MonoDevelop.MSBuild.SdkResolution;

namespace MonoDevelop.MSBuild.Editor.Completion
{
	class ProjectCollectionRuntimeInformation : IRuntimeInformation
	{
		readonly Toolset toolset;
		readonly string binDir;
		readonly string sdksDir;
		readonly Dictionary<SdkReference, string> resolvedSdks = new Dictionary<SdkReference, string> ();
		readonly MSBuildSdkResolver sdkResolver;

		public ProjectCollectionRuntimeInformation (ProjectCollection projectCollection)
		{
			toolset = projectCollection.GetToolset (projectCollection.DefaultToolsVersion);
			sdksDir = Path.GetFullPath (Path.Combine (toolset.ToolsPath, "..", "..", "Sdks"));
			sdkResolver = new MSBuildSdkResolver (binDir, sdksDir);
		}

		public string GetBinPath () => binDir;
		public IEnumerable<string> GetExtensionsPaths () => Enumerable.Empty<string> ();
		public IList<SdkInfo> GetRegisteredSdks () => Array.Empty<SdkInfo> ();
		public string GetSdksPath () => toolset.ToolsPath;
		public string GetToolsPath () => toolset.ToolsPath;

		/*
	public IEnumerable<string> GetExtensionsPaths ()
	{
		yield break;
		yield return target.GetMSBuildExtensionsPath ();
		if (Platform.IsMac) {
			yield return "/Library/Frameworks/Mono.framework/External/xbuild";
	}
		}*/

		public string GetSdkPath (SdkReference sdk, string projectFile, string solutionPath)
		{
			if (!resolvedSdks.TryGetValue (sdk, out string path)) {
				try {
					path = sdkResolver.GetSdkPath (sdk, new NoopLoggingService (), null, projectFile, solutionPath);
				} catch (Exception ex) {
					LoggingService.LogError ("Error in SDK resolver", ex);
				}
				resolvedSdks[sdk] = path;
			}
			return path;
		}

		class NoopLoggingService : ILoggingService
		{
			public void LogCommentFromText (MSBuildContext buildEventContext, MessageImportance messageImportance, string message)
			{
			}

			public void LogErrorFromText (MSBuildContext buildEventContext, object subcategoryResourceName, object errorCode, object helpKeyword, string file, string message)
			{
			}

			public void LogFatalBuildError (MSBuildContext buildEventContext, Exception e, string projectFile)
			{
			}

			public void LogWarning (string message)
			{
			}

			public void LogWarningFromText (MSBuildContext bec, object p1, object p2, object p3, string projectFile, string warning)
			{
			}
		}
	}
}