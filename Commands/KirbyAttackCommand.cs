using KirbyNightmareInDreamLand.Entities.Players;
using Microsoft.Xna.Framework.Audio;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyAttackCommand : ICommand
    {
        private SoundInstance soundInstance = SoundManager.CreateInstance("inhale_intro");
        
        private bool test = true;

        public void Execute()
        {
            ObjectManager.Instance.Players[0].Attack();
            SoundManager.Play("kirbyhurt1");
            //if (test)
            //{
            //    soundInstance.Play();
            //}
            //else
            //{
            //    soundInstance.Stop();
            //}
            //test = !test;
        }
    }
}
