﻿using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using SkiaSharp;
using Xunit;

namespace Microsoft.Maui.Resizetizer.Tests
{
	public class GenerateSplashAssetsTests : MSBuildTaskTestFixture<GenerateSplashAssets>
	{
		protected GenerateSplashAssets GetNewTask(ITaskItem splash) =>
			new()
			{
				IntermediateOutputPath = DestinationDirectory,
				MauiSplashScreen = new[] { splash },
				BuildEngine = this,
			};

		void AssertFile(string file, int width, int height)
		{
			file = Path.Combine(DestinationDirectory, file);

			Assert.True(File.Exists(file), $"File did not exist: {file}");

			using var codec = SKCodec.Create(file);
			Assert.Equal(width, codec.Info.Width);
			Assert.Equal(height, codec.Info.Height);
		}

		[Theory]
		[InlineData("dotnet_logo", "#512BD4")]
		[InlineData("appiconfg", "#0000FF")]
		public void FileIsGenerated(string image, string color)
		{
			var splash = new TaskItem($"images/{image}.svg");
			splash.SetMetadata("Color", color);
			var task = GetNewTask(splash);

			var success = task.Execute();
			Assert.True(success, $"{task.GetType()}.Execute() failed.");

			AssertFile($"{image}SplashScreen.scale-100.png", 620, 300);
			AssertFile($"{image}SplashScreen.scale-125.png", 775, 375);
			AssertFile($"{image}SplashScreen.scale-200.png", 1240, 600);
		}
	}
}
