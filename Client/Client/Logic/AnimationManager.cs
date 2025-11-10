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
        public int ActiveAnimation;
        public Animation[] Animations;

        public AnimationManager(int type, float blockDelay = 1.5f)
        {
            SetAnimations(type, blockDelay);
            ActiveAnimation = 0;
        }

        public void SetAnimation(int type)
        {
            if(ActiveAnimation == 0)
            {
                if (type == 2 && Animations[type] != null)
                {
                    Animations[ActiveAnimation].Reset();
                    ActiveAnimation = type;
                }
                else if (type == 1 && Animations[type] != null && !Animations[type].IsBlocked)
                {
                    Animations[ActiveAnimation].Reset();
                    Animations[type].BlockAnimation();
                    ActiveAnimation = type;
                }
            }
            else if(ActiveAnimation == 1)
            {
                if(type == 2 && Animations[type] != null)
                {
                    Animations[ActiveAnimation].Reset();
                    ActiveAnimation = type;
                }
                else if (type == 1 && Animations[ActiveAnimation].IsAnimationEnded)
                {
                    Animations[ActiveAnimation].Reset();
                    ActiveAnimation = 0;
                }
                else if (type == 0 && Animations[ActiveAnimation].IsAnimationEnded)
                {
                    Animations[ActiveAnimation].Reset();
                    ActiveAnimation = 0;
                }

            }
            else if (ActiveAnimation == 2)
            {
                if((type == 0 || type == 1) && Animations[type] != null && Animations[ActiveAnimation].IsAnimationEnded)
                {
                    Animations[ActiveAnimation].Reset();
                    ActiveAnimation = type;
                }
            }
        }

        public int GetActiveFrame()
        {
            return Animations[ActiveAnimation].ActiveFrame;
        }

        public bool AttackIsBlocked()
        {
            return Animations[1].IsBlocked;
        }

        public void Update(GameTime gameTime)
        {
             Animations[ActiveAnimation].Update();   

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
