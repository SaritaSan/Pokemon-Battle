using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private List<TimerData> _timerDataList;
    [SerializeField]
    private string _animationName = "TimerShowSeconds";
    [SerializeField]
    private UnityEvent _onTimerEnd;
 
    private Coroutine _timerCoroutine;
 
    public void StartTimer(int index)
    {
        _timerCoroutine = StartCoroutine(TimerCoroutine(index));
    }
 
    private IEnumerator TimerCoroutine(int index)
    {
        while (index >= 0)
        {
            _image.sprite = _timerDataList[index].sprite;
            _animator.Play(_animationName);
            SoundManager.instance.Play(_timerDataList[index].soundName);
            yield return new WaitForSeconds(1f);
            index--;
        }
 
        _onTimerEnd?.Invoke();
        _timerCoroutine = null;
    }
 
    public void StopTimer()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
 
        _image.sprite = null;
    }
}

[System.Serializable]
public class TimerData
{
    public Sprite sprite;
    public string soundName;
        

}
