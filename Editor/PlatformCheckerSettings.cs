using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
	/// <summary>
	/// 設定を管理するクラス
	/// </summary>
	internal sealed class PlatformCheckerSettings : ScriptableObject
	{
		//================================================================================
		// 定数
		//================================================================================
		private const string PATH = "ProjectSettings/UniPlatformCheckerSettings.json";

		//================================================================================
		// 変数(SerializeField)
		//================================================================================
		[SerializeField] private BuildTargetGroup[] m_buildTargetGroups = new BuildTargetGroup[0];
		[SerializeField] private LogLevel           m_logLevel          = LogLevel.ERROR;
		[SerializeField] private string             m_message           = "[UniPlatformCheckerSettings] 選択中のプラットフォームが適切ではありません。Build Settings で適切なプラットフォームに Switch Platform してください。";

		//================================================================================
		// 変数(static)
		//================================================================================
		private static PlatformCheckerSettings m_instance;

		//================================================================================
		// プロパティ
		//================================================================================
		internal IReadOnlyList<BuildTargetGroup> BuildTargetGroups => m_buildTargetGroups;
		internal LogLevel                        LogLevel          => m_logLevel;
		internal string                          Message           => m_message;

		//================================================================================
		// 関数(static)
		//================================================================================
		/// <summary>
		/// 設定を ProjectSettings フォルダから読み込みます
		/// </summary>
		internal static PlatformCheckerSettings Load()
		{
			if ( m_instance != null ) return m_instance;

			m_instance = CreateInstance<PlatformCheckerSettings>();

			if ( !File.Exists( PATH ) ) return m_instance;

			var json = File.ReadAllText( PATH, Encoding.UTF8 );

			JsonUtility.FromJsonOverwrite( json, m_instance );

			if ( m_instance == null )
			{
				m_instance = CreateInstance<PlatformCheckerSettings>();
			}

			return m_instance;
		}

		/// <summary>
		/// 設定を ProjectSettings フォルダに保存します
		/// </summary>
		internal static void Save()
		{
			var json = JsonUtility.ToJson( m_instance, true );

			File.WriteAllText( PATH, json, Encoding.UTF8 );
		}
	}
}