using System;
using Foundation;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Common.Graphics;
using System.IO;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using NavigationFlow.Navigation;

namespace NavigationFlow
{
    public partial class MainWindow : WaveEngine.Adapter.Application, WaveEngine.Common.IApplication
    {
        #region Computed Properties
        private SpriteBatch spriteBatch;
        private Texture2D splashScreen;
        private bool splashState = true;
        private TimeSpan time;
        private Vector2 position;
        private Color backgroundSplashColor;
        private KeyboardState lastKeyboardState;

        private NavigationFlow.Game game;
        #endregion

        #region Constructors

        public MainWindow(IntPtr handle) : base(handle)
        {
            this.WindowTitle = "NavigationFlow";
            this.ResizeScreen(1280, 720);
        }

        [Export("initWithCoder:")]
        public MainWindow(NSCoder coder) : base(coder)
        {
        }

        #endregion

        #region Override Methods
        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public override void Initialize()
        {
            this.game = new NavigationFlow.Game();
            this.game.Initialize(this);

            #region WAVE SOFTWARE LICENSE AGREEMENT
            this.backgroundSplashColor = new Color("#ebebeb");
            this.spriteBatch = new SpriteBatch(WaveServices.GraphicsDevice);

            var resourceNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();
            string name = string.Empty;

            foreach (string item in resourceNames)
            {
                if (item.Contains("SplashScreen.png"))
                {
                    name = item;
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidProgramException("License terms not agreed.");
            }

            using (Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
            {
                this.splashScreen = Texture2D.FromFile(WaveServices.GraphicsDevice, stream);	
            }
            #endregion
        }

        /// <summary>
        /// Update the specified elapsedTime.
        /// </summary>
        /// <param name="elapsedTime">Elapsed time.</param>
        public override void Update(TimeSpan elapsedTime)
        {
            if (this.game != null && !this.game.HasExited)
            {
                if (this.splashState)
                {
                    #region WAVE SOFTWARE LICENSE AGREEMENT
                    position.X = (this.Width / 2.0f) - (this.splashScreen.Width / 2.0f);
                    position.Y = (this.Height / 2.0f) - (this.splashScreen.Height / 2.0f);
                    this.time += elapsedTime;
                    if (time > TimeSpan.FromSeconds(2))
                    {
                        this.splashState = false;
                    }
                    #endregion
                }
                else
                {
                    var keyboardState = WaveServices.Input.KeyboardState;

                    if (keyboardState.IsKeyReleased(Keys.Escape) &&
                       this.lastKeyboardState.IsKeyPressed(Keys.Escape))
                    {
                        var navService = WaveServices.GetService<NavigationService>();

                        if (navService.CanNavigate(NavigateCommands.Back))
                        {
                            navService.Navigate(NavigateCommands.Back);
                        }
                    }
                    else
                    {
                        this.game.UpdateFrame(elapsedTime);
                    }
                    
                    this.lastKeyboardState = keyboardState;
                }
            }
        }

        /// <summary>
        /// Draw the specified elapsedTime.
        /// </summary>
        /// <param name="elapsedTime">Elapsed time.</param>
        public override void Draw(TimeSpan elapsedTime)
        {
            if (this.game != null && !this.game.HasExited)
            {
                if (this.splashState)
                {
                    #region WAVE SOFTWARE LICENSE AGREEMENT
                    WaveServices.GraphicsDevice.RenderTargets.SetRenderTarget(null);
                    WaveServices.GraphicsDevice.Clear(ref this.backgroundSplashColor, ClearFlags.Target, 1);
                    this.spriteBatch.Draw(this.splashScreen, this.position, Color.White);
                    this.spriteBatch.Render();
                    #endregion
                }
                else
                {
                    this.game.DrawFrame(elapsedTime);
                }
            }
        }
        #endregion
    }
}
