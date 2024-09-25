using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame.Block
{ 
    public class BlockList
    {
        private static BlockList instance;

        private List<Sprite> blockList;
        private int currentBlock;
        private int firstBlock = 0;
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

        public void setBlockList(List<Sprite> myBlocks)
        {
            blockList = myBlocks;
            currentBlock = firstBlock;
            lastBlock = myBlocks.Count - 1;
        }

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
                blockList[currentBlock].Draw(location,spriteBatch);
            }
        }

        public void Update()
        {
            // do nothing
            blockList[currentBlock].Update();

        }

    }

}
