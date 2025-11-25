using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic
{
    public class AnimationManager
    {
        public AnimationType ActiveAnimation;
        private bool IsDeadAnimationEnabled;
        public Animation[] Animations;

        public AnimationManager(int type, float blockDelay = 1.0f)
        {
            SetAnimations(type, blockDelay);
            ActiveAnimation = AnimationType.Walking;
            IsDeadAnimationEnabled = false;
        }

        public void SetAnimation(AnimationType type)
        {
            int animationIndex = (int) type;

            if(ActiveAnimation == AnimationType.Walking)
            {
                if (type == AnimationType.Dying && Animations[animationIndex] != null)
                {
                    Animations[animationIndex].Reset();
                    ActiveAnimation = type;
                }
                else if(type == AnimationType.Dying && Animations[animationIndex] == null)
                {
                    IsDeadAnimationEnabled = true;
                }
                else if (type == AnimationType.Attacking && Animations[animationIndex] != null && !Animations[animationIndex].IsBlocked)
                {
                    Animations[(int) ActiveAnimation].Reset();
                    Animations[animationIndex].BlockAnimation();
                    ActiveAnimation = type;
                }
            }
            else if(ActiveAnimation == AnimationType.Attacking)
            {
                if(type == AnimationType.Dying && Animations[animationIndex] != null)
                {
                    Animations[(int) ActiveAnimation].Reset();
                    ActiveAnimation = type;
                }
                else if (type == AnimationType.Dying && Animations[animationIndex] == null)
                {
                    IsDeadAnimationEnabled = true;
                }
                else if (type == AnimationType.Attacking && Animations[(int) ActiveAnimation].IsAnimationEnded)
                {
                    Animations[(int) ActiveAnimation].Reset();
                    ActiveAnimation = AnimationType.Walking;
                }
                else if (type == AnimationType.Walking && Animations[(int) ActiveAnimation].IsAnimationEnded)
                {
                    Animations[(int) ActiveAnimation].Reset();
                    ActiveAnimation = AnimationType.Walking;
                }

            }
            else if (ActiveAnimation == AnimationType.Dying)
            {

            }
        }

        public int GetActiveFrame()
        {
            return Animations[(int) ActiveAnimation].ActiveFrame;
        }

        public bool CheckDeadAnimation()
        {
            if (ActiveAnimation == AnimationType.Dying || IsDeadAnimationEnabled)
            {
                if (Animations[2] != null) return Animations[2].IsAnimationEnded;
                else return true;
            }
            else return false;
        }

        public void Update(GameTime gameTime)
        {
             Animations[(int) ActiveAnimation].Update();

             if (Animations[1] != null && Animations[1].IsBlocked) Animations[1].UpdateBlock(gameTime);    
        }

        public void SetAnimations(int type, float blockDelay)
        {
            Animations = new Animation[3];

            switch(type)
            {
                //Orc1
                case 0:
                    Animations[0] = new Animation(4, 8); //idle
                    Animations[1] = new Animation(8, 5, blockDelay); //attack
                    Animations[2] = new Animation(8, 5); //death
                    break;
                //Orc2
                case 1:
                    Animations[0] = new Animation(4, 8);
                    Animations[1] = new Animation(8, 5, blockDelay);
                    Animations[2] = new Animation(8, 5);
                    break;
                //Orc3
                case 2:
                    Animations[0] = new Animation(4, 8);
                    Animations[1] = new Animation(8, 5, blockDelay);
                    Animations[2] = new Animation(8, 5);
                    break;
                //Plant1
                case 3:
                    Animations[0] = new Animation(4, 8);
                    Animations[1] = new Animation(7, 5, blockDelay);
                    Animations[2] = new Animation(10, 5);
                    break;
                //Plant2
                case 4:
                    Animations[0] = new Animation(4, 8);
                    Animations[1] = new Animation(7, 5, blockDelay);
                    Animations[2] = new Animation(10, 5);
                    break;
                //Plant3
                case 5:
                    Animations[0] = new Animation(4, 8);
                    Animations[1] = new Animation(7, 5, blockDelay);
                    Animations[2] = new Animation(10, 5);
                    break;
                //Slime1
                case 6:
                    Animations[0] = new Animation(6, 7);
                    Animations[1] = new Animation(10, 4, blockDelay);
                    Animations[2] = new Animation(10, 5);
                    break;
                //Slime2
                case 7:
                    Animations[0] = new Animation(6, 7);
                    Animations[1] = new Animation(11, 4, blockDelay);
                    Animations[2] = new Animation(10, 5);
                    break;
                //Slime3
                case 8:
                    Animations[0] = new Animation(6, 6);
                    Animations[1] = new Animation(9, 4, blockDelay);
                    Animations[2] = new Animation(10, 5);
                    break;
                //Vampire1
                case 9:
                    Animations[0] = new Animation(4, 8);
                    Animations[1] = new Animation(12, 4, blockDelay);
                    Animations[2] = new Animation(11, 5);
                    break;
                //Vampire2
                case 10:
                    Animations[0] = new Animation(4, 8);
                    Animations[1] = new Animation(12, 4, blockDelay);
                    Animations[2] = new Animation(11, 5);
                    break;
                //Vampire3
                case 11:
                    Animations[0] = new Animation(4, 8);
                    Animations[1] = new Animation(12, 4, blockDelay);
                    Animations[2] = new Animation(11, 5);
                    break;
                //Boar
                case 12:
                    Animations[0] = new Animation(4, 8); 
                    Animations[1] = new Animation(6, 5, blockDelay); 
                    Animations[2] = null;
                    break;
                //Player
                case 13:
                    Animations[0] = new Animation(8, 6);
                    Animations[1] = new Animation(8, 5, blockDelay);
                    Animations[2] = null;
                    break;
                //Pig
                case 14:
                    Animations[0] = new Animation(4, 8);
                    Animations[1] = null;
                    Animations[2] = null;
                    break;
                //BrownRabbit
                case 15:
                    Animations[0] = new Animation(4, 8);
                    Animations[1] = null;
                    Animations[2] = null;
                    break;
                //WhiteRabbit
                case 16:
                    Animations[0] = new Animation(4, 8);
                    Animations[1] = null;
                    Animations[2] = null;
                    break;

            }
        }

    }
}
