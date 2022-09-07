using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Alarm : MonoBehaviour
{
    [SerializeField] private float spendTimeToMaxVolume = 2f;

    private AudioSource _audioSource;
    private Coroutine _currentCoroutine;

    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0;
    }

    public void TurnOnOff(bool state)
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = StartCoroutine(ChangeVolumeSmoothly(state));
    }

    private IEnumerator ChangeVolumeSmoothly(bool toUp)
    {
        float deltaRate = 1 / spendTimeToMaxVolume;
        float targetValue = 0;

        if (toUp)
        {
            targetValue = 1f;

            if (_audioSource.isPlaying == false)
            {
                _audioSource.Play();
            }
        }

        while (_audioSource.volume != targetValue)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, targetValue, deltaRate * Time.deltaTime);
            yield return null;
        }

        if (toUp == false && _audioSource.volume == 0f)
        {
            _audioSource.Stop();
        }
    }
}
