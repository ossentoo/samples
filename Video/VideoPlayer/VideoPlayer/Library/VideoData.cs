﻿namespace VideoSamples.Library
{
	public class VideoData
	{
		public VideoData()
		{

		}

		/// <summary>
		/// Current position
		/// </summary>
		/// <value>At.</value>
		public double At {get;set;}

		/// <summary>
		/// In Milliseconds
		/// </summary>
		/// <value>The duration.</value>
		public double Duration {get;set;}

		public VideoState State { get; set; }

		public override bool Equals (object obj)
		{
			var t = obj as VideoData;
			if (t == null)
				return false;

			if (t.At == At && t.Duration == Duration && State == t.State) {
				return true;
			}

			return false;
		}

		public override string ToString ()
		{
			return string.Format ("[VideoData: At={0}, Duration={1}, State={2}]", At, Duration, State);
		}

		public override int GetHashCode ()
		{
			return ToString ().GetHashCode ();
		}
	}
}


