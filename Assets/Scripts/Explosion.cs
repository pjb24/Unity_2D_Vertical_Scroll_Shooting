using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Invoke("Disable", 1f);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    public void StartExplosion(string target)
    {
        _animator.SetTrigger("OnExplosion");

        switch (target)
        {
            case "S":
                {
                    transform.localScale = Vector3.one * 0.7f;
                    break;
                }
            case "M":
            case "P":
                {
                    transform.localScale = Vector3.one * 1f;
                    break;
                }
            case "L":
                {
                    transform.localScale = Vector3.one * 2f;
                    break;
                }
            case "B":
                {
                    transform.localScale = Vector3.one * 3f;
                    break;
                }
            default:
                {
                    Debug.LogError("Requested Non Setted Parameter: " + target);
                    break;
                }
        }
    }
}
