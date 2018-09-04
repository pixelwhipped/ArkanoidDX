using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ArkanoidDXUniverse.Input
{
    public class UnifiedInput
    {
        public bool Action;
        public Vector2 DragFrom = Vector2.Zero;
        public List<Operation<Vector2>> DraggedListeners;
        public List<Operation<Vector2>> DraggingListeners;

        public Arkanoid Game;
        public Vector2 Location = Vector2.Zero;
        public List<Procedure<Vector2>> MoveListeners;

        public List<Procedure<Vector2>> TapListeners;

        private TimeSpan _volatileTapTime = TimeSpan.Zero;
        private Vector2 _volatileTap = Vector2.Zero;

        public Vector2 VolatileTap
        {
            get
            {
                if(_volatileTapTime < TimeSpan.Zero) _volatileTap = Vector2.Zero;
                var t = _volatileTap;
                _volatileTap = Vector2.Zero;
                return t;
            }
        }

        public UnifiedInput(Arkanoid game)
        {
            Game = game;
            TapListeners = new List<Procedure<Vector2>>();
            MoveListeners = new List<Procedure<Vector2>>();
            DraggingListeners = new List<Operation<Vector2>>();
            DraggedListeners = new List<Operation<Vector2>>();

            Game.MouseInput.LeftClickListeners.Add(Tap);
            Game.MouseInput.MoveListeners.Add(Move);
            Game.MouseInput.DraggedListeners.Add(Dragged);
            Game.MouseInput.DraggingListeners.Add(Dragging);
            Game.TouchInput.TapListeners.Add(Tap);
            Game.TouchInput.MoveListeners.Add(Move);
            Game.TouchInput.DraggedListeners.Add(Dragged);
            Game.TouchInput.DraggingListeners.Add(Dragging);
        }

        public bool Hidden
        {
            get
            {
                if (Game.TouchInput.Location == Vector2.Zero && Game.MouseInput.Hidden) return true;
                return Action;
            }
            set
            {
                if (value)
                    Game.TouchInput.Location = Vector2.Zero;
                Game.MouseInput.Hidden = value;
            }
        }

        private void Dragging(Vector2 a, Vector2 b)
        {
            var remove = new List<Operation<Vector2>>();
            foreach (var draggingListener in DraggingListeners)
            {
                try
                {
                    draggingListener(a, b);
                }
                catch
                {
                    remove.Add(draggingListener);
                }
            }
            DraggingListeners.RemoveAll(remove.Contains);
        }

        private void Dragged(Vector2 a, Vector2 b)
        {
            var remove = new List<Operation<Vector2>>();
            foreach (var draggedListener in DraggedListeners)
            {
                try
                {
                    draggedListener(a, b);
                }
                catch
                {
                    remove.Add(draggedListener);
                }
            }
            DraggedListeners.RemoveAll(remove.Contains);
        }

        private void Move(Vector2 value)
        {
            var remove = new List<Procedure<Vector2>>();
            foreach (var moveListener in MoveListeners)
            {
                try
                {
                    moveListener(value);
                }
                catch
                {
                    remove.Add(moveListener);
                }
            }
            MoveListeners.RemoveAll(remove.Contains);
        }

        private void Tap(Vector2 value)
        {
            var remove = new List<Procedure<Vector2>>();
            _volatileTap = value;
            _volatileTapTime = new TimeSpan(0,0,0,0,200);
            foreach (var tapListener in TapListeners)
            {
                try
                {
                    tapListener(value);
                }
                catch
                {
                    remove.Add(tapListener);
                }
            }
            TapListeners.RemoveAll(remove.Contains);
        }

        public void Update(GameTime gameTime)
        {
            _volatileTapTime -= gameTime.ElapsedGameTime;
            if (!Game.MouseInput.LeftClickListeners.Contains(Tap)) Game.MouseInput.LeftClickListeners.Add(Tap);
            if (!Game.MouseInput.MoveListeners.Contains(Move)) Game.MouseInput.MoveListeners.Add(Move);
            if (!Game.MouseInput.DraggedListeners.Contains(Dragged)) Game.MouseInput.DraggedListeners.Add(Dragged);
            if (!Game.MouseInput.DraggingListeners.Contains(Dragging)) Game.MouseInput.DraggingListeners.Add(Dragging);
            if (!Game.TouchInput.TapListeners.Contains(Tap)) Game.TouchInput.TapListeners.Add(Tap);
            if (!Game.TouchInput.MoveListeners.Contains(Move)) Game.TouchInput.MoveListeners.Add(Move);
            if (!Game.TouchInput.DraggedListeners.Contains(Dragged)) Game.TouchInput.DraggedListeners.Add(Dragged);
            if (!Game.TouchInput.DraggingListeners.Contains(Dragging)) Game.TouchInput.DraggingListeners.Add(Dragging);
            Location = Game.TouchInput.Location == Vector2.Zero ? Game.MouseInput.Location : Game.TouchInput.Location;
            DragFrom = Game.TouchInput.DragFrom == Vector2.Zero ? Game.MouseInput.DragFrom : Game.TouchInput.DragFrom;
            Action = Game.TouchInput.Location == Vector2.Zero
                ? Game.MouseInput.State.LeftButton == ButtonState.Pressed
                : Game.TouchInput.Location != Vector2.Zero;
        }
    }
}