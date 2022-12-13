
public class LinkNode<T> : IPoolObject
{
    internal LinkList<T> m_List;
    public LinkNode<T> Pre { get; internal set; }
    public LinkNode<T> Next { get; internal set; }
    public T Value { get; internal set; }

    public void Delete()
    {
        if (Pre != null)
            Pre.Next = Next;
        else
            m_List.m_First = Next;

        if (Next != null)
            Next.Pre = Pre;
        else
            m_List.m_Last = Pre;

        m_List.m_Count--;
        m_List.m_NodePool.Release(this);
        Value = default;
        m_List = default;
    }

    public void OnCreate(IPool from)
    {
        m_List = null;
        Pre = null;
        Next = null;
        Value = default;
    }

    public void OnRelease(IPool from)
    {
        m_List = null;
        Pre = null;
        Next = null;
        Value = default;
    }

    public void OnDestroy(IPool from)
    {

    }
}