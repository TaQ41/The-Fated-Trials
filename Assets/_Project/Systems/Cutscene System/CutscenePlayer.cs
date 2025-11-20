using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenericSceneLoader.Locale;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CutsceneSystem
{
    /// <summary>
    /// Place anywhere in a scene to allow the temporary storage of cutscenes and playing them back when needed.
    /// This acts as a Queue and so the first cutscene put in, is the first cutscene put out.
    /// </summary>
    public sealed class CutscenePlayer : MonoBehaviour
    {
        [SerializeField]
        private ProjectFileHeader projectFile;

        private readonly Queue<CutsceneReference> m_cutscenes = new();
        private readonly List<CutsceneReference> m_loadedCutscenes = new();
        private int m_currentSceneHandle;
        private bool m_isCutscenePlaying;

        /// <summary>
        /// Called when the object detects that there are no more cutscenes to be played.
        /// </summary>
        public Action completed;

        void OnEnable()
        {
            SceneManager.sceneUnloaded += EndOfCutscene;
        }

        void OnDisable()
        {
            m_cutscenes.Clear();
            m_loadedCutscenes.Clear();
            m_isCutscenePlaying = false;
            SceneManager.sceneUnloaded -= EndOfCutscene;
        }

        /// <summary>
        /// Verification method to play the next cutscene if the scene is the last played cutscene.
        /// Called on the SceneManager.sceneLoaded UnityAction.
        /// </summary>
        /// <param name="scene"></param>
        void EndOfCutscene(Scene scene)
        {
            if (scene.handle == m_currentSceneHandle)
            {
                m_isCutscenePlaying = false;
                Play();
            }
        }

        /// <summary>
        /// Assumes the projectFile is set in this object and proceeds to reset the cutscene queue to all the items from
        /// PendingProgressEventsData.InitialCutscenes; these are enqueued in the order of the list.
        /// </summary>
        public void UseInitialCutscenes()
        {
            if (projectFile == null)
                return;

            m_cutscenes.Clear();
            foreach (CutsceneReference cutscene in projectFile.PendingProgressEventsData.InitialCutscenes)
                AddCutscene(cutscene);
        }

        /// <summary>
        /// Adds the provided cutscene to the queue so long as the cutscene isn't already within the queue.
        /// </summary>
        /// <param name="cutscene"></param>
        public void AddCutscene(CutsceneReference cutscene)
        {
            foreach (CutsceneReference storedCutscene in m_cutscenes)
            {
                if (cutscene.CutsceneName.Equals(storedCutscene.CutsceneName))
                    Debug.Log("Found same name!");
            }

            m_cutscenes.Enqueue(cutscene);
        }

        /// <summary>
        /// Try to play the next cutscene in the queue. Fails if another cutscene is currently alive or if there are no more cutscenes.
        /// </summary>
        public void Play()
        {
            if (m_isCutscenePlaying)
                return;

            if (m_cutscenes.Count == 0)
            {
                TriggerCutscenePlayerCompletion();
                return;
            }

            m_isCutscenePlaying = true;
            _ = LoadCutscene();
            m_cutscenes.Dequeue();
        }

        /// <summary>
        /// Load the cutscene at the current point of the queue, verify its constraints pass and update the current scene handle member.
        /// This also appends the scene to the loaded cutscene list.
        /// </summary>
        private async Task LoadCutscene()
        {
            CutsceneReference cutscene = m_cutscenes.Peek();
            if (!AreCutsceneConstraintsPassed(cutscene))
                return;

            m_loadedCutscenes.Add(cutscene);
            await SceneManager.LoadSceneAsync(cutscene.CutsceneName, LoadSceneMode.Additive);
            m_currentSceneHandle = SceneManager.GetSceneByName(cutscene.CutsceneName).handle;
        }

        /// <summary>
        /// Checks all the constraints of the provided CutsceneReference and determines if any fail.
        /// 1. Is the cutscene allowed to be played in the player's current scene?
        /// </summary>
        /// <returns>True on all constraints passing and false on any failing.</returns>
        private bool AreCutsceneConstraintsPassed(CutsceneReference cutscene)
        {
            return LocaleFinder.GetMultipleVillageLocaleSceneNames(cutscene.PlayableLocalesConstraint)
                                    .Contains(projectFile.PlayerData.CurrentVillageLocaleSceneName);

        }

        /// <summary>
        /// A cleanup method for when the cutscene player has finished, finally invokes the completed Action.
        /// </summary>
        private void TriggerCutscenePlayerCompletion()
        {
            foreach (CutsceneReference cutscene in m_loadedCutscenes)
                projectFile.PendingProgressEventsData.InitialCutscenes.Remove(cutscene);
            
            m_loadedCutscenes.Clear();
            completed.Invoke();
        }
    }
}