
public class Direction2D<T>
{
    public T up;
    public T down;
    public T left;
    public T right;

    public Direction2D(T value)
    {
        setAll(value);
    }

    public Direction2D(T u, T d, T l, T r)
    {
        up = u;
        left = l;
        right = r;
        down = d;
    }

    public void setAll(T value)
    {
        up = down = left = right = value;
    }
}
