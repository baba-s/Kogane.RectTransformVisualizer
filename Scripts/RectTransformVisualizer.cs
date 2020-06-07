using Kogane.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kogane
{
	/// <summary>
	/// RectTransform の範囲を可視化するクラス
	/// </summary>
	public sealed class RectTransformVisualizer
	{
		//==============================================================================
		// 変数(readonly)
		//==============================================================================
		private readonly float                  m_outlineSize;
		private readonly Color                  m_outlineColor;
		private readonly Func<GameObject, bool> m_predicate;

		private readonly List<GameObject> m_list = new List<GameObject>();

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public RectTransformVisualizer
		(
			float                  outlineSize,
			Color                  outlineColor,
			Func<GameObject, bool> predicate
		)
		{
			m_outlineSize  = outlineSize;
			m_outlineColor = outlineColor;
			m_predicate    = predicate;
		}

		/// <summary>
		/// RectTransform の範囲を可視化します
		/// </summary>
		public void Show()
		{
			// すでに可視化している場合はいったん非表示にします
			Hide();

			// 範囲を表示するすべての RectTransform を取得します
			var list = GetAllScenes()
					.SelectMany( x => x.GetRootGameObjects() )
					.SelectMany( x => x.GetComponentsInChildren<RectTransform>( true ) )
					.Where( x => m_predicate( x.gameObject ) )
				;

			foreach ( var n in list )
			{
				// アウトラインを表示するためのゲームオブジェクトを作成します
				var go = new GameObject( "Outline" );

				// RectTransform の範囲を表示するゲームオブジェクトの子供にします
				go.transform.SetParent( n.transform );

				// アウトラインのゲームオブジェクトは Hierarchy に表示しないかつ
				// シーンに保存しない設定にします
				go.hideFlags = HideFlags.HideAndDontSave;

				// アウトラインを表示するためのコンポーネントをアタッチします
				go.AddComponent<CanvasRenderer>();

				var squareUI = go.AddComponent<OutlineSquareUI>();

				// アウトラインのサイズと色を設定します
				// また、タップ判定から除外します
				squareUI.raycastTarget = false;
				squareUI.OutlineSize   = m_outlineSize;
				squareUI.color         = m_outlineColor;

				// アウトラインのサイズを親に合わせます
				var rectTransform = go.GetComponent<RectTransform>();

				rectTransform.localPosition    = Vector3.zero;
				rectTransform.localScale       = Vector3.one;
				rectTransform.anchoredPosition = Vector3.zero;
				rectTransform.anchorMin        = Vector2.zero;
				rectTransform.anchorMax        = Vector2.one;
				rectTransform.offsetMin        = Vector2.zero;
				rectTransform.offsetMax        = Vector2.zero;

				// 非表示にするためのリストに追加します
				m_list.Add( go );
			}
		}

		/// <summary>
		/// テキストの表示範囲を非表示にします
		/// </summary>
		public void Hide()
		{
			for ( int i = 0; i < m_list.Count; i++ )
			{
				var go = m_list[ i ];

				if ( go == null ) continue;

				GameObject.Destroy( go );
			}

			m_list.Clear();
		}

		/// <summary>
		/// 読み込まれているシーンの一覧を返します
		/// </summary>
		private static IEnumerable<Scene> GetAllScenes()
		{
			for ( int i = 0; i < SceneManager.sceneCount; i++ )
			{
				yield return SceneManager.GetSceneAt( i );
			}
		}
	}
}