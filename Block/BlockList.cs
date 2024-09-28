using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame.Block
{
    public class BlockList
    {
        private static BlockList instance;

        private List<string> blockList;
        private int currentBlock;
        private Sprite currentSprite;
        private readonly int firstBlock = 0;
        private int oldBlock;
        private int lastBlock;

        public static BlockList Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
                else
                {
                    instance = new BlockList();
                }
                return instance;
            }
        }

        // init because Instance init must be empty. 
        public void SetBlockList(List<string> myBlocks)
        {
            blockList = myBlocks;
            currentBlock = firstBlock;
            oldBlock = currentBlock;
            lastBlock = myBlocks.Count - 1;
            currentSprite = SpriteFactory.Instance.createSprite(blockList[currentBlock]);
        }

        // if we aren't at the end of the list, increment. Otherwise, go to start. 
        public void ViewNext()
        {
            if (currentBlock < lastBlock)
            {
                currentBlock++;
            }
            else
            {
                currentBlock = firstBlock;
            }
        }

        // if we aren't at the beginning of the list, decrement. Otherwise, go to end. 
        public void ViewPrevious()
        {
            if (currentBlock > firstBlock)
            {
                currentBlock--;
            }
            else
            {
                currentBlock = lastBlock;
            }
        }

        // Draw the current block.
        public void Draw(Vector2 location, SpriteBatch spriteBatch)
        {
            if (currentBlock >= 0 && currentBlock < blockList.Count)
            {
                currentSprite.Draw(location, spriteBatch);
            }
        }

        public void Update()
        {
            // don't want to make a new sprite with every update.
            // only make a new sprite if we are looking at a new block.
            // this if block allows waterfall tile to animate. 
            if (oldBlock != currentBlock)
            {
                currentSprite = SpriteFactory.Instance.createSprite(blockList[currentBlock]);
                oldBlock = currentBlock;
            }
            // do nothing for most tile, animates waterfall tiles.
            currentSprite.Update();
        }

    }

}
