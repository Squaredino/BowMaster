using Random = UnityEngine.Random;

public struct SecureInt
{
    private readonly int _value;
    private readonly int _offset;

    public SecureInt(int value)
    {
        _offset = Random.Range(-1000, 1000);
        _value = value + _offset;
    }

    public int GetValue()
    {
        return _value - _offset;
    }

    public override string ToString()
    {
        return GetValue().ToString();
    }

    public static SecureInt operator +(SecureInt i1, SecureInt i2)
    {
        return new SecureInt(i1.GetValue() + i2.GetValue());
    }
    
    public static SecureInt operator -(SecureInt i1, SecureInt i2)
    {
        return new SecureInt(i1.GetValue() - i2.GetValue());
    }
}
