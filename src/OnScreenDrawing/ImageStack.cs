using System.Collections.Generic;
using System.Drawing;

namespace Gears
{
	public class BitmapStack
	{
		private Stack<Bitmap> bitmapStack;

		public int Count
		{
			get
			{
				return this.bitmapStack.Count;
			}
		}

		public BitmapStack()
		{
			this.bitmapStack = new Stack<Bitmap>();
		}

		public Bitmap Push(Bitmap bitmap)
		{
			bitmapStack.Push(bitmap);
			return bitmap;
		}

		public void Clear()
		{
			this.bitmapStack.Clear();
		}

		public Bitmap Pop()
		{
			if (this.Count == 1)
			{
				return this.bitmapStack.Peek();
			}
			else
			{
				return this.bitmapStack.Pop();
			}
		}

		public Bitmap Peek()
		{
			return this.bitmapStack.Peek();
		}

		public Bitmap PushFirst(Bitmap bitmap)
		{
			bitmapStack.Clear();
			bitmapStack.Push(bitmap);
			return bitmap;
		}
	}
}
