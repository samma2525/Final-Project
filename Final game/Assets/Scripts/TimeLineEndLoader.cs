using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimeLineEndLoader : MonoBehaviour
{
    public PlayableDirector director;
    public string nextSceneName = "InsideCave";
    void Start()
    {
        if (director == null)
        {
            director = GetComponent<PlayableDirector>();
        }
        director.stopped += OnTimelineFinished;
    }

    void OnTimelineFinished(PlayableDirector pd)
    {
        SceneManager.LoadScene(nextSceneName);
    }

    private void OnDestory()
    {
        if (director != null)
        {
            director.stopped -= OnTimelineFinished;
        }
    }
}
