namespace ProjectFile
{
    /// <summary>
    /// All of the events that are waiting to happen to progress the game. These include the cutscenes that should play and where, the character
    /// dialogue stages and requests they have (This also means when characters give you vital missions), and missions that the player has completed along
    /// with all the custom missions they have made.
    /// </summary>
    [System.Serializable]
    public class PendingProgressEvents
    {
        // Initial Cutscenes

        // Character Pending Dialogue/requests, Character dialogue stages

        // Fixed Missions Completed or uncompleted (Only need to serialize a value that can hint at the fixed village mission in-game)

        // Custom missions (must serialize entire mission)
    }
}