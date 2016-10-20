using System;
using CoreGraphics;
using Foundation;
using MediaPlayer;
using UIKit;
using VideoSamples.Controls;
using static System.String;

namespace VideoSamples.iOS.Controls
{
	public class MyPlayerView : UIView
	{
	    readonly MyAVPlayerController _moviePlayer;
		private bool _UsingFullWidth;
		private bool _UsingFullHeight;


		public MyPlayerView (MyVideoPlayer Parent)
		{
			BackgroundColor = UIColor.Clear;

			_moviePlayer = new MyAVPlayerController (Parent);	

			// add to subview
			AddSubview (_moviePlayer.View);
		}

		public override void Draw (CGRect rect)
		{
			base.Draw (rect);

			var screenRect = UIScreen.MainScreen.Bounds;
			var w = screenRect.Size.Width;
			var h = screenRect.Size.Height;

			if (_UsingFullHeight) {
				// screen - top bar (nav + statusbar)
				rect.Height = h - TopBarSize();
			}
			if (_UsingFullWidth) {
				rect.Width = w;
			}

			_moviePlayer.View.Frame = rect;
			Frame = rect;
		}

		private int TopBarSize()
		{
			var s = 0;
			var h = 0;

			// find Navigationbar. could be null if no navigationbar.
			// xamarin support hack
			var hasNav = UIApplication.SharedApplication.KeyWindow.Subviews.FirstOrDefaultFromMany (item => item.Subviews, x => x is UINavigationBar);

			if (UIApplication.SharedApplication.StatusBarHidden == false) {
				s = Convert.ToInt32 (UIApplication.SharedApplication.StatusBarFrame.Height);
			}

			if (hasNav != null) {
				h = Convert.ToInt32 (hasNav.Frame.Height);
			}

			return s + h;

		}

		protected internal void UpdateFrame(MyVideoPlayer source)
		{
			UIApplication.SharedApplication.InvokeOnMainThread (() => {
				_UsingFullWidth = false;
				_UsingFullHeight = false;

				var screenRect = UIScreen.MainScreen.Bounds;
				var w = screenRect.Size.Width;
				var h = screenRect.Size.Height;

				// update frame
				var f = Frame;
				if (source.ContentWidth <= 0) {
					f.Width = w;
					_UsingFullWidth = true;
				}
				else
				{
					f.Width = Convert.ToSingle (source.ContentWidth);
				}

				if (source.ContentHeight <= 0) {
					f.Height = h;
					_UsingFullHeight = true;
				} else {
					f.Height = Convert.ToSingle (source.ContentHeight);
				}


				Frame = f;
				SetNeedsDisplay ();
			});
		}

//		protected internal void Start()
//		{
//			if (_moviePlayer.PlaybackState == MPMoviePlaybackState.Paused) {
//				// handle pause
//				_moviePlayer.Play ();
//			} else {
//				_moviePlayer.PrepareToPlay ();
//			}
//		}

//		protected internal void Stop()
//		{			
//			_moviePlayer.Stop ();
//		}
//
//		protected internal void Pause()
//		{
//			_moviePlayer.Pause ();
//		}

		protected internal void SeekTo(double pos)
		{
			// _moviePlayer.CurrentPlaybackTime = pos;
		}

		/// <summary>
		/// TimBarton fix 02/2016 (https://github.com/amccorma/xamarin-amccorma/issues/1)
		/// </summary>
		/// <param name="file">File.</param>
		protected internal void Load(NSString file)
		{
			// file must be set to Content
			// url starts with http
			if (!IsNullOrEmpty (file)) {
				if (file.ToString().ToLower().StartsWith ("http") == false) {
					_moviePlayer.Load (NSUrl.FromFilename (file));
				} else {
					_moviePlayer.Load (NSUrl.FromString (file), true);
				}
			}
		}


		protected internal bool FullScreen {			
			set {
				if (value) {
					// _moviePlayer.SetFullscreen (true, true);
				} else {
					// _moviePlayer.SetFullscreen (false, true);
				}
			}
		}

		protected internal bool FitToWindow
		{
			set {
				if (value) {
					_moviePlayer.View.ClipsToBounds = true;
					// _moviePlayer.ScalingMode = MPMovieScalingMode.AspectFill;
				} else {
					_moviePlayer.View.ClipsToBounds = false;
					// _moviePlayer.ScalingMode = MPMovieScalingMode.None;
				}
			}
		}

		protected internal bool AddController
		{
			set {
				if (value) {
					// _moviePlayer.ControlStyle = MPMovieControlStyle.Embedded;
				} else {
					// _moviePlayer.ControlStyle = MPMovieControlStyle.None;
				}
			}
		}

		protected override void Dispose (bool disposing)
		{
			_moviePlayer.Dispose ();
			base.Dispose (disposing);
		}
	}
}

