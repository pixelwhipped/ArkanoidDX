using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ArkanoidDXUniverse
{
    public class Sounds : ResourceCompoment
    {
        public SoundEffect BallBounce;
        public SoundEffect BallMetalBounce;
        public SoundEffect Coin;
        public SoundEffect GameOver;
        public SoundEffect GameStart;
        public SoundEffect IntroMusic;
        public SoundEffect Menu;
        public SoundEffect VausCollect;
        public SoundEffect VausDeath;
        public SoundEffect VausDie;
        public SoundEffect VausEnter;
        public SoundEffect VausLaser;
        public SoundEffect VausLong;

        public Sounds(Game game) : base(game)
        {
        }


        public override void LoadContent()
        {
            Menu = Content.Load<SoundEffect>("Audio\\Menu");
            VausLong = Content.Load<SoundEffect>("Audio\\Ark_Longship");
            VausLaser = Content.Load<SoundEffect>("Audio\\Ark_Lazer");
            VausDeath = Content.Load<SoundEffect>("Audio\\Ark_die");
            VausDie = Content.Load<SoundEffect>("Audio\\Ark_die_2");
            VausEnter = Content.Load<SoundEffect>("Audio\\Ark_Enter");
            GameOver = Content.Load<SoundEffect>("Audio\\Ark_Game_Over_Music_2");
            GameStart = Content.Load<SoundEffect>("Audio\\Ark_Game_Start_Music");
            IntroMusic = Content.Load<SoundEffect>("Audio\\Ark_Intro_Music");
            VausCollect = Content.Load<SoundEffect>("Audio\\Ark_Collect");
            Coin = Content.Load<SoundEffect>("Audio\\Ark_Coin");
            BallBounce = Content.Load<SoundEffect>("Audio\\Ark_Bounce_2");
            BallMetalBounce = Content.Load<SoundEffect>("Audio\\Ark_Metal_Bounce");
        }
    }
}