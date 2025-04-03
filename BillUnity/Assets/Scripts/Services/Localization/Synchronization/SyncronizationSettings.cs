using UnityEditor;
using UnityEngine;

namespace Kborod.Services.Localization
{
    internal class SyncronizationSettings : ScriptableObject
    {
        private static SyncronizationSettings instance;

        private const string LocalizationSettingsFile = "Assets/Scripts/Services/Localization/SynchronizationSettings.asset";

        /// <summary>
		/// Table id on Google Spreadsheet.
		/// Let's say your table has the following url https://docs.google.com/spreadsheets/d/1RvKY3VE_y5FPhEECCa5dv4F7REJ7rBtGzQg9Z_B_DE4/edit#gid=331980525
		/// So your table id will be "1RvKY3VE_y5FPhEECCa5dv4F7REJ7rBtGzQg9Z_B_DE4" and sheet id will be "331980525" (gid parameter)
		/// </summary>
		[SerializeField] private string tableId;

        /// <summary>
        /// Table sheet contains sheet name and id. First sheet has always zero id. Sheet name is used when saving.
        /// </summary>
        [SerializeField] private Sheet[] sheets;

        /// <summary>
        /// Folder to save spreadsheets. Must be inside Resources folder.
        /// </summary>
        [SerializeField] private UnityEngine.Object saveFolder;

		public string TableId => tableId;

		public Sheet[] Sheets => sheets;

		public UnityEngine.Object SaveFolder => saveFolder;


		public static SyncronizationSettings Instance
        {
            get
            {
                if (instance == null)
                {

                    instance = (SyncronizationSettings)AssetDatabase.LoadAssetAtPath(LocalizationSettingsFile, typeof(SyncronizationSettings));

                    if (instance == null)
                    {
                        instance = ScriptableObject.CreateInstance<SyncronizationSettings>();
                        AssetDatabase.CreateAsset(instance, LocalizationSettingsFile);
                    }
                }
                return instance;
            }
        }
	}
}
/* Copyright: Made by Appfox */