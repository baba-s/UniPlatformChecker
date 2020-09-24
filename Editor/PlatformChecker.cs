using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
	/// <summary>
	/// Unity エディタの現在のプラットフォームが正しいか確認するエディタ拡張
	/// </summary>
	[InitializeOnLoad]
	internal static class PlatformChecker
	{
		//================================================================================
		// 関数(static)
		//================================================================================
		/// <summary>
		/// コンストラクタ
		/// </summary>
		static PlatformChecker()
		{
			EditorApplication.playModeStateChanged += OnChange;

			Check();
		}

		/// <summary>
		/// Unity のプレイモードの状態が変化した時に呼び出されます
		/// </summary>
		private static void OnChange( PlayModeStateChange state )
		{
			if ( state != PlayModeStateChange.EnteredPlayMode ) return;

			Check();
		}

		/// <summary>
		/// Unity エディタの現在のプラットフォームが正しいか確認します
		/// </summary>
		private static void Check()
		{
			var settings = PlatformCheckerSettings.Load();

			if ( settings == null ) return;

			var buildTargetGroups = settings.BuildTargetGroups;

			if ( buildTargetGroups == null || buildTargetGroups.Count <= 0 ) return;

			var activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
			var buildTargetGroup  = BuildPipeline.GetBuildTargetGroup( activeBuildTarget );

			if ( buildTargetGroups.Contains( buildTargetGroup ) ) return;

			var logLevel = settings.LogLevel;
			var message  = settings.Message;

			switch ( logLevel )
			{
				case LogLevel.INFO:
					Debug.Log( message );
					break;
				case LogLevel.WARNING:
					Debug.LogWarning( message );
					break;
				case LogLevel.ERROR:
					Debug.LogError( message );
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}
	}
}