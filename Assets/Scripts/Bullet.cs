using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    public int Damage => _damage;

    [SerializeField] private bool _isRotate = false;

    private void OnEnable()
    {
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (_isRotate)
        {
            transform.Rotate(Vector3.forward * 10f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BorderBullet")
        {
            gameObject.SetActive(false);
        }
    }
}
