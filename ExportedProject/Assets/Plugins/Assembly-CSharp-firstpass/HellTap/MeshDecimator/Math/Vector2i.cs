using System;
using System.Globalization;

namespace HellTap.MeshDecimator.Math
{
	public struct Vector2i : IEquatable<Vector2i>
	{
		public static readonly Vector2i zero = new Vector2i(0, 0);

		public int x;

		public int y;

		public int Magnitude
		{
			get
			{
				return (int)System.Math.Sqrt(x * x + y * y);
			}
		}

		public int MagnitudeSqr
		{
			get
			{
				return x * x + y * y;
			}
		}

		public int this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return x;
				case 1:
					return y;
				default:
					throw new IndexOutOfRangeException("Invalid Vector2i index!");
				}
			}
			set
			{
				switch (index)
				{
				case 0:
					x = value;
					break;
				case 1:
					y = value;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid Vector2i index!");
				}
			}
		}

		public Vector2i(int value)
		{
			x = value;
			y = value;
		}

		public Vector2i(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public static Vector2i operator +(Vector2i a, Vector2i b)
		{
			return new Vector2i(a.x + b.x, a.y + b.y);
		}

		public static Vector2i operator -(Vector2i a, Vector2i b)
		{
			return new Vector2i(a.x - b.x, a.y - b.y);
		}

		public static Vector2i operator *(Vector2i a, int d)
		{
			return new Vector2i(a.x * d, a.y * d);
		}

		public static Vector2i operator *(int d, Vector2i a)
		{
			return new Vector2i(a.x * d, a.y * d);
		}

		public static Vector2i operator /(Vector2i a, int d)
		{
			return new Vector2i(a.x / d, a.y / d);
		}

		public static Vector2i operator -(Vector2i a)
		{
			return new Vector2i(-a.x, -a.y);
		}

		public static bool operator ==(Vector2i lhs, Vector2i rhs)
		{
			if (lhs.x == rhs.x)
			{
				return lhs.y == rhs.y;
			}
			return false;
		}

		public static bool operator !=(Vector2i lhs, Vector2i rhs)
		{
			if (lhs.x == rhs.x)
			{
				return lhs.y != rhs.y;
			}
			return true;
		}

		public static explicit operator Vector2i(Vector2 v)
		{
			return new Vector2i((int)v.x, (int)v.y);
		}

		public static explicit operator Vector2i(Vector2d v)
		{
			return new Vector2i((int)v.x, (int)v.y);
		}

		public void Set(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public void Scale(ref Vector2i scale)
		{
			x *= scale.x;
			y *= scale.y;
		}

		public void Clamp(int min, int max)
		{
			if (x < min)
			{
				x = min;
			}
			else if (x > max)
			{
				x = max;
			}
			if (y < min)
			{
				y = min;
			}
			else if (y > max)
			{
				y = max;
			}
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() ^ (y.GetHashCode() << 2);
		}

		public override bool Equals(object other)
		{
			if (!(other is Vector2i))
			{
				return false;
			}
			Vector2i vector2i = (Vector2i)other;
			if (x == vector2i.x)
			{
				return y == vector2i.y;
			}
			return false;
		}

		public bool Equals(Vector2i other)
		{
			if (x == other.x)
			{
				return y == other.y;
			}
			return false;
		}

		public override string ToString()
		{
			return string.Format("({0}, {1})", x.ToString(CultureInfo.InvariantCulture), y.ToString(CultureInfo.InvariantCulture));
		}

		public string ToString(string format)
		{
			return string.Format("({0}, {1})", x.ToString(format, CultureInfo.InvariantCulture), y.ToString(format, CultureInfo.InvariantCulture));
		}

		public static void Scale(ref Vector2i a, ref Vector2i b, out Vector2i result)
		{
			result = new Vector2i(a.x * b.x, a.y * b.y);
		}
	}
}
