using UnityEngine;

public class TargetWZ : MonoBehaviour
{
    [SerializeField] private WaitingZone _waitingZone;

    public bool IsFree { get; private set; }

    private void Start()
    {
        IsFree = true;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.TryGetComponent<Customer>(out Customer customer))
        {
            if (customer.CheckPosition(transform))
            {
                customer.StopMove();
                _waitingZone.SetTargetCustomer(transform, customer);
                customer.ReadyDrive();
            }
        }
    }

    public void Unlock()
    {
        IsFree = true;
    }

    public void Block()
    {
        IsFree = false;
    }
}
