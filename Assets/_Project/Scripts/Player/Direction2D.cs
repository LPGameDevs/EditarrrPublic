
using UnityEngine;

[System.Serializable]
public class Direction2D<T>
{
    [field: SerializeField] public T Up { get; set; }
    [field: SerializeField] public T Down { get; set; }
    [field: SerializeField] public T Left { get; set; }
    [field: SerializeField] public T Right { get; set; }

    public Direction2D(T value)
    {
        this.SetAll(value);
    }

    public Direction2D(T u, T d, T l, T r)
    {
        this.Up = u;
        this.Down = d;
        this.Left = l;
        this.Right = r;
    }

    public void SetAll(T value)
    {
        this.Up = this.Down = this.Left = this.Right = value;
    }
}