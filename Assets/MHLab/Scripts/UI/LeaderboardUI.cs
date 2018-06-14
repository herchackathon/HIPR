using MHLab.UI;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    public RectTransform LeaderboardContainer;

    private bool _isExpanded = false;

    public void OnInteractionButtonPressed()
    {
        if (_isExpanded)
        {
            LeaderboardContainer.GetComponent<TriggerAnimation>().Trigger("LeaderboardEaseOut");
            _isExpanded = false;
        }
        else
        {

            LeaderboardContainer.GetComponent<TriggerAnimation>().Trigger("LeaderboardEaseIn");
            _isExpanded = true;
        }
    }
}
