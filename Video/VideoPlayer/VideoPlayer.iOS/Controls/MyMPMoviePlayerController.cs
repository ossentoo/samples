using System;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using MediaPlayer;
using ObjCRuntime;
using VideoSamples.Controls;
using VideoSamples.Library;

namespace VideoSamples.iOS.Controls
{
	public class MyMPMoviePlayerController : MPMoviePlayerController
	{
		private WeakReference _parentElement;

		private readonly CancellationTokenSource _token;

		private bool _EndOfVideo = true;


		public MyMPMoviePlayerController (MyVideoPlayer parent)
		{
			ParentElement = parent;

			NSNotificationCenter.DefaultCenter.AddObserver (this, 
				new Selector("LoadChangedCallback:"),
				LoadStateDidChangeNotification,
				null
			);

			_token = new CancellationTokenSource ();

			Task.Run (async() => {
				while(_token.Token.IsCancellationRequested == false)
				{
					await Task.Delay(TimeSpan.FromMilliseconds(200), _token.Token);

					var t = CurrentPlaybackTime;
					var d = PlayableDuration;

					// is there a better way to calculate the end of video???
					if (PlayableDuration > 1.000 && (Math.Abs(t - d) <= 0.0001 * double.Epsilon))
					{
						ParentElement.State = VideoState.ENDED;
						ParentElement.Info = new VideoData
						{
							State = VideoState.ENDED,
							At = t,
							Duration = d
						};
						_EndOfVideo = true;
					}
					else
					{
						_EndOfVideo = false;

						if (PlaybackState == MPMoviePlaybackState.Paused)
						{
							ParentElement.State = VideoState.PAUSE;
						}
						else if (PlaybackState == MPMoviePlaybackState.Stopped)
						{
							ParentElement.State = VideoState.STOP;

						}
						else if (PlaybackState == MPMoviePlaybackState.Playing)
						{
							ParentElement.State = VideoState.PLAY;
						}

						if (PlayableDuration > 1.000)
						{
							ParentElement.Info = new VideoData
							{
								State = ParentElement.State,
								At = (double.IsNaN(t) ? -1 : t),
								Duration = d
							};
						}
					}
				}

			}, _token.Token);
		}

		[Export("LoadChangedCallback:")]
		public void LoadChangedCallback(NSObject o)
		{			
			if (ErrorLog != null) {
				System.Diagnostics.Debug.WriteLine (ErrorLog.Description);
				ParentElement.HasError = true;
			}
		}
			
		protected internal void Load(NSUrl url, bool http = false)
		{
			ParentElement.HasError = false;
			if (http) {
				// may get error:
				// App Transport Security has blocked a cleartext HTTP (http://) resource load since it is insecure. Temporary exceptions can be configured via your app's Info.plist file.
				// need to modify Info.plist or use https
				SourceType = MPMovieSourceType.Streaming;
			} else {
				SourceType = MPMovieSourceType.File;
			}
			ContentUrl = url;
		}

	    private MyVideoPlayer ParentElement
		{
			get {
				return _parentElement.Target as MyVideoPlayer;
			}
			set {
				_parentElement = new WeakReference (value);
			}
		}

		protected override void Dispose (bool disposing)
		{
			NSNotificationCenter.DefaultCenter.RemoveObserver (LoadStateDidChangeNotification);


			_token.Cancel ();
			_token.Dispose ();

			base.Dispose (disposing);
		}
	}
}

