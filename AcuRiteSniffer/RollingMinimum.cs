using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcuRiteSniffer
{
	/// <summary>
	/// Provides methods to calculate minimum, maximum, and average wind values across a window of time.
	/// </summary>
	public class RollingMinMaxAvg
	{
		public class StoredValue
		{
			internal long timestamp;
			public double speed;
			public double directionDegrees;
			internal bool IsExpired(long now, long maxAge)
			{
				return timestamp < now - maxAge;
			}
		}

		Queue<StoredValue> q = new Queue<StoredValue>();

		public readonly long MillisecondWindowSize;
		Stopwatch sw;

		/// <summary>
		/// Constructs a RollingMinMaxAvg.
		/// </summary>
		/// <param name="millisecondWindowSize">Maximum number of milliseconds to store a value for before deleting it.</param>
		public RollingMinMaxAvg(long millisecondWindowSize)
		{
			this.MillisecondWindowSize = millisecondWindowSize;
			sw = Stopwatch.StartNew();
		}

		private void Cleanup()
		{
			long now = sw.ElapsedMilliseconds;
			while (true)
			{
				var o = q.Peek();
				if (o != null && o.IsExpired(now, MillisecondWindowSize))
					q.Dequeue();
				else
					break;
			}
		}
		/// <summary>
		/// Adds a value to the internal collection.  It will be deleted after <see cref="MillisecondWindowSize"/> milliseconds.
		/// </summary>
		/// <param name="val">Value to add to the internal collection.</param>
		public void Add(double speed, double directionDegrees)
		{
			q.Enqueue(new StoredValue() { timestamp = sw.ElapsedMilliseconds, speed = speed, directionDegrees = directionDegrees });
			Cleanup();
		}
		/// <summary>
		/// Returns the smallest value that is currently being stored.
		/// </summary>
		/// <returns></returns>
		public StoredValue GetMinimum()
		{
			Cleanup();
			StoredValue min = null;
			foreach (StoredValue v in q)
			{
				if (min == null || v.speed <= min.speed)
					min = v;
			}
			return min;
		}
		/// <summary>
		/// Returns the largest value that is currently being stored.
		/// </summary>
		/// <returns></returns>
		public StoredValue GetMaximum()
		{
			Cleanup();
			StoredValue max = null;
			foreach (StoredValue v in q)
			{
				if (max == null || v.speed >= max.speed)
					max = v;
			}
			return max;
		}
		/// <summary>
		/// Returns the average of the values that are currently being stored.
		/// </summary>
		/// <returns></returns>
		public StoredValue GetAverage()
		{
			Cleanup();
			double avgSpeed = q.Select(s => s.speed).Average();
			double avgDirection = q.Select(s => s.directionDegrees).Average();
			return new StoredValue() { speed = avgSpeed, directionDegrees = avgDirection };
		}
	}
}
