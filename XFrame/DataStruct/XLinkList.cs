
namespace XFrame.Collections
{
	public class LinkList<T>
	{
		internal Pool m_NodePool;
		internal LinkNode<T> m_First;
		internal LinkNode<T> m_Last;
		internal int m_Count;

		public LinkNode<T> First => m_First;
		public int Count => m_Count;

		public LinkList()
		{
			m_NodePool = new Pool(typeof(LinkNode<T>), 512);
			m_First = null;
			m_Last = null;
		}

		public void AddLast(T data)
		{
			m_NodePool.Require(out LinkNode<T> node);
			node.m_List = this;
			node.Value = data;
			if (m_First == null)
			{
				m_First = node;
				m_Last = node;
			}
			else
			{
				m_Last.Next = node;
				node.Pre = m_Last;
				node.Next = null;
				m_Last = node;
			}
			m_Count++;
		}
	}

}
