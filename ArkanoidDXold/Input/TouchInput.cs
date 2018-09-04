using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;

namespace ArkanoidDX.Input
{
    public class TouchInput
    {
        public List<Procedure<Vector2>> TapListeners;
        public List<Procedure<Vector2>> MoveListeners;
        public List<Vector2> TapLocations;
        public List<Vector2> TouchLocations;
        public MouseInput MouseInput { get; private set; }
        public TouchInput()
        {
            TapListeners = new List<Procedure<Vector2>>();
            MoveListeners = new List<Procedure<Vector2>>();
            TouchLocations = new List<Vector2>();
            TapLocations = new List<Vector2>();
            MouseInput = new MouseInput();
            MouseClick = null;
            MouseInput.LeftClickListeners.Add((v)=>MouseClick=v);
        }

        public Vector2? MouseClick { get; set; }

        public void Update(GameTime gameTime)
        {
            
            var currentTouchState = TouchPanel.GetState();
            TouchLocations.Clear();
            TapLocations.Clear();
            if (MouseClick != null)
            {
                TouchLocations.Add(new Vector2(MouseClick.Value.X, MouseClick.Value.Y));
                MouseClick = null;
            }
            
            foreach (var touch in currentTouchState)
            {
                TouchLocations.Add(new Vector2(touch.Position.X, touch.Position.Y));

                TouchLocation prevLoc;
                if (!touch.TryGetPreviousLocation(out prevLoc))
                {
                    continue;
                }
                var remove = new List<Procedure<Vector2>>();

                if (prevLoc.State == TouchLocationState.Moved)
                {
                    var delta = touch.Position - prevLoc.Position;
                    if (Math.Abs(delta.X) < 4 || Math.Abs(delta.Y) < 4)
                    {
                        foreach (var moveListener in MoveListeners)
                        {
                            try
                            {
                                moveListener(new Vector2(touch.Position.X, touch.Position.Y));
                            }
                            catch
                            {
                                remove.Add(moveListener);
                            }
                        }
                        MoveListeners.RemoveAll(remove.Contains);
                    }
                }
                remove.Clear();
                if (touch.State == TouchLocationState.Released)
                {
                    foreach (var tapListener in TapListeners)
                    {
                        try
                        {
                            var tl = new Vector2(touch.Position.X, touch.Position.Y);
                            tapListener(tl);
                            TapLocations.Add(tl);
                        }
                        catch
                        {
                            remove.Add(tapListener);
                        }
                    }
                    TapListeners.RemoveAll(remove.Contains);
                }
            }
            MouseInput.Update(gameTime);
        }

    }
}
