using System;
using AppKit;
using CoreGraphics;
using FigmaSharp;
using FigmaSharp.Services;
using FigmaSharp.Cocoa;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using LiteForms;
using LiteForms.Cocoa;

namespace Game.Cocoa
{
    public class GameWindow : Window
    {
        const int WalkModifier = 13;
      
        ImageView[] spikesTiles;
		ImageView[] wallTiles;

        List<ImageView> gemsTiles;
        List<ImageView> heartTiles;
        Label pointsLabel;
		ImageView playerTile;
        int points = 0;

		void MovePlayer(Point point)
		{
			if (PlayerPositionCollidesWithWall(point))
				return;
			playerTile.SetPosition( point.X, point.Y);

			Refresh();
		}

		bool PlayerPositionCollidesWithWall(Point point)
		{
			var playerPosition = new Rectangle (point, playerTile.Size);
			foreach (var gem in wallTiles)
			{
				if (gem.Allocation.IntersectsWith(playerPosition))
					return true;
			}
			return false;
		}

		void Refresh()
		{
			var playerPosition = playerTile.Allocation;
			//if user is in a 
			foreach (var spike in spikesTiles)
			{
				if (spike.Allocation.IntersectsWith(playerPosition))
				{
					PlayerDied();
					return;
				}
			}

			foreach (var gem in gemsTiles)
			{
				if (gem.Allocation.IntersectsWith(playerTile.Allocation))
				{
					gemsTiles.Remove(gem);
					gem.Parent.RemoveChild(gem);
					points++;
					coinSound.Play();
					break;
				}
			}
			pointsLabel.Text = points.ToString();
		}

		void PlayerDied()
		{
			playerTile.SetPosition (startingPoint);
			var lastLive = heartTiles.FirstOrDefault();
			if (lastLive != null)
			{
				heartTiles.Remove(lastLive);
				lastLive.Parent.RemoveChild(lastLive);
			}
			gameOverSound.Play();
		}

		protected override void OnKeyDownPressed(object sender, NSEvent theEvent)
		{
			if (theEvent.KeyCode == (ushort)NSKey.LeftArrow)
			{
				var frame = playerTile.Allocation;
				MovePlayer(new Point(frame.X - WalkModifier, frame.Y));
				return;
			}

			if (theEvent.KeyCode == (ushort)NSKey.RightArrow)
			{
				var frame = playerTile.Allocation;
				MovePlayer(new Point(frame.X + WalkModifier, frame.Y));
				return;
			}

			if (theEvent.KeyCode == (ushort)NSKey.UpArrow)
			{
				var frame = playerTile.Allocation;
				MovePlayer(new Point(frame.X, frame.Y - WalkModifier));
				return;
			}

			if (theEvent.KeyCode == (ushort)NSKey.DownArrow)
			{
				var frame = playerTile.Allocation;
				MovePlayer(new Point(frame.X, frame.Y + WalkModifier));
				return;
			}
		}

		Point startingPoint;
        AVFoundation.AVAudioPlayer backgroundMusic;
        AVFoundation.AVAudioPlayer coinSound;
        AVFoundation.AVAudioPlayer gameOverSound;

        public GameWindow(Rectangle rect) : base(rect)
        {
            //we get the default basic view converters from the current loaded toolkit
            var converters = FigmaSharp.AppContext.Current.GetFigmaConverters();

            //in this case we want use a remote file provider (figma url from our document)
            var fileProvider = new FigmaRemoteFileProvider();
            fileProvider.Load("Jv8kwhoRsrmtJDsSHcTgWGYu");

            //we initialize our renderer service, this uses all the converters passed
            //and generate a collection of NodesProcessed which is basically contains <FigmaModel, IView, FigmaParentModel>
            var rendererService = new FigmaViewRendererService(fileProvider, converters);

            //play background music
            var backgroundMusicPath = new NSUrl(NSBundle.MainBundle.PathForResource("Background", "mp3"));
            backgroundMusic = AVFoundation.AVAudioPlayer.FromUrl(backgroundMusicPath, out NSError error);
            backgroundMusic.NumberOfLoops = -1;
            backgroundMusic.Play();

            var gameOverSoundPath = new NSUrl(NSBundle.MainBundle.PathForResource("GameOver", "mp3"));
            gameOverSound = AVFoundation.AVAudioPlayer.FromUrl(gameOverSoundPath, out error);
         
            var coinMusicPath = new NSUrl(NSBundle.MainBundle.PathForResource("Coin", "mp3"));
            coinSound = AVFoundation.AVAudioPlayer.FromUrl(coinMusicPath, out error);

			//we want load the entire level 1
			IView view = rendererService.RenderByName <IView>("Level1");
			Content = view;

            playerTile = rendererService.FindViewStartsWith<ImageView>("Player");

            startingPoint = playerTile.Allocation.Origin;

            pointsLabel = rendererService.FindViewByName<Label>("Points");

            gemsTiles = rendererService.FindViewsStartsWith<ImageView>("Gem")
                .ToList ();

            wallTiles = rendererService.FindViewsStartsWith<ImageView>("Tile")
                .ToArray();

			spikesTiles = rendererService.FindViewsStartsWith<ImageView>("Spikes")
				.ToArray();

			heartTiles = rendererService.FindViewsStartsWith<ImageView>("Heart")
				.OrderBy(s => s.Allocation.X)
				.ToList();

			Refresh();
		}
    }
}
