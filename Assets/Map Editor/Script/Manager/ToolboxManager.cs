using UnityEngine;
using System.Collections;

namespace FlooMapEditor
{
	public class ToolboxManager : MonoBehaviour 
	{
		private static ToolboxManager instance;

		public static ToolboxManager Instance
		{
			get
			{
				return instance;
			}
		}

		public GameObject toolboxGrid;
		public GameObject toolboxMapFeature;
		public GameObject toolboxSpawner;

		public Transform parentTemplateTile;
		public Transform parentTemplateFeature;
		public Transform parentTemplateSpawner;

		public ToolBoxTileType selectedTileType;
		public ToolboxTileTemplate tileTemplate;

		// Use this for initialization
		void Start () 
		{
			instance = this;
		}

		#region Button Control
		public void ShowGridTab()
		{
			EditorTileManager.Instance.DisableMapFeatureSpawnDetection();
			EditorTileManager.Instance.DisableMapFeatureCollider();
			EditorTileManager.Instance.DisableSpawnerSpawnDetection();
			EditorTileManager.Instance.DisableSpawnerCollider();
			toolboxGrid.SetActive(true);
			toolboxMapFeature.SetActive(false);
			toolboxSpawner.SetActive(false);

			selectedTileType = ToolBoxTileType.Tile;
			SelectToolboxTile(parentTemplateTile.GetChild(0).GetComponent<ToolboxTileTemplate>());
		}

		public void ShowMapFeatureTab()
		{
			selectedTileType = ToolBoxTileType.MapFeature;
			EditorTileManager.Instance.EnableMapFeatureCollider();
			EditorTileManager.Instance.DisableSpawnerSpawnDetection();
			EditorTileManager.Instance.DisableSpawnerCollider();

			toolboxGrid.SetActive(false);
			toolboxMapFeature.SetActive(true);
			toolboxSpawner.SetActive(false);
			SelectToolboxMapFeature(parentTemplateFeature.GetChild(0).GetComponent<ToolboxMapFeatureTemplate>());
		}

		public void ShowSpawnerTab()
		{
			EditorTileManager.Instance.DisableMapFeatureSpawnDetection();
			EditorTileManager.Instance.DisableMapFeatureCollider();

			EditorTileManager.Instance.EnableSpawnerCollider();

			selectedTileType = ToolBoxTileType.Spawner;
			toolboxGrid.SetActive(false);
			toolboxMapFeature.SetActive(false);
			toolboxSpawner.SetActive(true);

			SelectToolboxSpawner(parentTemplateSpawner.GetChild(0).GetComponent<ToolBoxSpawnerTemplate>());

		}
		#endregion

		#region Tile Selector
		public void SelectToolboxTile(ToolboxTileTemplate selectedTile)
		{
			HideProperties();
			EditorTileManager.Instance.HideEditorHighlight();
			HighlightSelectedTileTemplate(selectedTile.assetCode);
			tileTemplate = selectedTile;
		}

		void HighlightSelectedTileTemplate(string assetCode)
		{
			int count = parentTemplateTile.childCount;
			for (int i = 0; i < count; i++)
			{
				ToolboxTileTemplate tileTemplate = parentTemplateTile.GetChild(i).gameObject.GetComponent<ToolboxTileTemplate>();
				if (tileTemplate != null)
				{
					if (tileTemplate.assetCode.Equals(assetCode))
					{
						tileTemplate.SetImageSelect();
					}
					else
					{
						tileTemplate.SetImageUnselect();
					}
				}
			}
		}
		#endregion

		#region Map Feature Selector
		public void SelectToolboxMapFeature(ToolboxMapFeatureTemplate selectedMF)
		{
			HideProperties();
			EditorTileManager.Instance.HideEditorHighlight();
			HighlightSelectedMFTemplate(selectedMF.assetCode);
			tileTemplate = selectedMF;

			if(string.IsNullOrEmpty( selectedMF.assetCode ))
			{
				EditorTileManager.Instance.DisableMapFeatureSpawnDetection();
			}
			else
			{
				EditorTileManager.Instance.EnableMapFeatureSpawnDetection();
			}
		}
		
		void HighlightSelectedMFTemplate(string assetCode)
		{
			int count = parentTemplateFeature.childCount;
			for (int i = 0; i < count; i++)
			{
				ToolboxTileTemplate featureTemplate = parentTemplateFeature.GetChild(i).gameObject.GetComponent<ToolboxTileTemplate>();
				if (featureTemplate != null)
				{
					if (featureTemplate.assetCode.Equals(assetCode))
					{
						featureTemplate.SetImageSelect();
					}
					else
					{
						featureTemplate.SetImageUnselect();
					}
				}
			}
		}
		#endregion

		#region Spawner Selector
		public void SelectToolboxSpawner(ToolBoxSpawnerTemplate selectedSpawner)
		{
			HideProperties();
			EditorTileManager.Instance.HideEditorHighlight();
			HighlightSelectedSpawnerTemplate(selectedSpawner.assetCode);
			tileTemplate = selectedSpawner;

			if(string.IsNullOrEmpty( selectedSpawner.assetCode ))
			{
				EditorTileManager.Instance.DisableSpawnerSpawnDetection();
			}
			else
			{
				EditorTileManager.Instance.EnableSpawnerSpawnDetection();
			}
		}

		void HighlightSelectedSpawnerTemplate(string assetCode)
		{
			int count = parentTemplateSpawner.childCount;
			for (int i = 0; i < count; i++)
			{
				ToolboxTileTemplate spawnerTemplate = parentTemplateSpawner.GetChild(i).gameObject.GetComponent<ToolboxTileTemplate>();
				if (spawnerTemplate != null)
				{
					if (spawnerTemplate.assetCode.Equals(assetCode))
					{
						spawnerTemplate.SetImageSelect();
					}
					else
					{
						spawnerTemplate.SetImageUnselect();
					}
				}
			}
		}
		#endregion

		public void HideProperties()
		{
			MapEditorTileProperty.Instance.HideProperty();
			MapEditorFeatureProperty.Instance.HideProperty();
			MapEditorSpawnerProperty.Instance.HideProperty();
		}
	}
}
