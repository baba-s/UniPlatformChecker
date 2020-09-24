using UnityEditor;
using UnityEngine.UIElements;

namespace Kogane.Internal
{
	/// <summary>
	/// Project Settings における設定画面を管理するクラス
	/// </summary>
	internal sealed class PlatformCheckerSettingsProvider : SettingsProvider
	{
		//================================================================================
		// 変数
		//================================================================================
		private PlatformCheckerSettings m_settings;
		private SerializedObject        m_serializedObject;
		private SerializedProperty      m_buildTargetGroupsProperty;
		private SerializedProperty      m_logLevelProperty;
		private SerializedProperty      m_messageProperty;

		//================================================================================
		// 関数
		//================================================================================
		/// <summary>
		/// コンストラクタ
		/// </summary>
		internal PlatformCheckerSettingsProvider( string path, SettingsScope scope )
			: base( path, scope )
		{
		}

		/// <summary>
		/// アクティブになった時に呼び出されます
		/// </summary>
		public override void OnActivate( string searchContext, VisualElement rootElement )
		{
			m_settings                  = PlatformCheckerSettings.Load();
			m_serializedObject          = new SerializedObject( m_settings );
			m_buildTargetGroupsProperty = m_serializedObject.FindProperty( "m_buildTargetGroups" );
			m_logLevelProperty          = m_serializedObject.FindProperty( "m_logLevel" );
			m_messageProperty           = m_serializedObject.FindProperty( "m_message" );
		}

		/// <summary>
		/// GUI を描画する時に呼び出されます
		/// </summary>
		public override void OnGUI( string searchContext )
		{
			using ( var checkScope = new EditorGUI.ChangeCheckScope() )
			{
				EditorGUILayout.PropertyField( m_buildTargetGroupsProperty );
				EditorGUILayout.PropertyField( m_logLevelProperty );
				EditorGUILayout.PropertyField( m_messageProperty );

				if ( checkScope.changed )
				{
					m_serializedObject.ApplyModifiedProperties();
					PlatformCheckerSettings.Save();
				}
			}
		}

		//================================================================================
		// 関数(static)
		//================================================================================
		/// <summary>
		/// Project Settings にメニューを追加します
		/// </summary>
		[SettingsProvider]
		private static SettingsProvider Create()
		{
			var path     = "Kogane/UniPlatformChecker";
			var provider = new PlatformCheckerSettingsProvider( path, SettingsScope.Project );

			return provider;
		}
	}
}