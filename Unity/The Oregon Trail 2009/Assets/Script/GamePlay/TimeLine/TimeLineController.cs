using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;

public class TimeLineController : MonoBehaviour
{
    [System.Serializable]
    public class TriggerData
    {
        public GameObject target;      // object Animator
        public float triggerTime = 0;  //  tính  giây
        public float destroyTime = -1;
        [HideInInspector] public bool triggered = false;
        [HideInInspector] public Animator animator;
        [HideInInspector] public bool destroyed;
    }

    public PlayableDirector director;
    public List<TriggerData> triggers = new List<TriggerData>();

    private bool timelineEnded = false;

    void Start()
    {
        foreach (var t in triggers)
        {
            if (t.target != null)
            {
                t.animator = t.target.GetComponent<Animator>();
                if (t.animator != null)
                    t.animator.enabled = false; // Tắt ban đầu
            }
        }
    }

    void Update()
    {
        double currentTime = director.time;

        foreach (var t in triggers)
        {
            if (!t.triggered && currentTime >= t.triggerTime)
            {
                if (t.animator != null)
                {
                    t.animator.enabled = true;
                    Debug.Log($"[TimelineTrigger] {t.target.name} started at {t.triggerTime}s");
                }
                t.triggered = true;
            }

            // Kiểm tra destroy riêng cho từng con
            if (!t.destroyed && t.destroyTime >= 0 && currentTime >= t.destroyTime)
            {
                if (t.target != null)
                {
                    Debug.Log($"[TimelineTrigger] Destroy {t.target.name} at {t.destroyTime}s");
                    Destroy(t.target);
                }
                t.destroyed = true;
            }
        }
    }
}


