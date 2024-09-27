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
        private int firstBlock = 0;
        private int oldBlock;
        private int lastBlock;

        public static BlockList Instance
        {
            get
            {
                if( instance == null)
                {
                    instance = new BlockList();
                }
                return instance; 
            }
        }

        public void setBlockList(List<string> myBlocks)
        {
            blockList = myBlocks;
            currentBlock = firstBlock;
            oldBlock = currentBlock;
            lastBlock = myBlocks.Count - 1;
            currentSprite = SpriteFactory.Instance.createSprite(blockList[currentBlock]);
        }

        // if we aren't at the end of the list, increment. Otherwise, go to start. 
        public void viewNext()
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

        // if we aren't at the beginning of the list, decrement. Otherwise, got to end. 
        public void viewPrevious()
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



        // Draws the current block. 
        public void Draw(Vector2 location, SpriteBatch spriteBatch)
        {
            if (currentBlock >= 0 && currentBlock < blockList.Count)
            {
                currentSprite.Draw(location,spriteBatch);
            }
        }

        public void Update()
        {
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
